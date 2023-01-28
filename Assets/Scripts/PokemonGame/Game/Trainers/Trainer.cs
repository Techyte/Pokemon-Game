using System;
using System.Collections;
using PokemonGame.Dialogue;
using PokemonGame.ScriptableObjects;
using UnityEngine;
using UnityEngine.AI;

namespace PokemonGame.Game.Trainers
{
    /// <summary>
    /// Initiates a battle based on certain inspector parameters 
    /// </summary>
    public class Trainer : DialogueTrigger
    {
        public Party playerParty;
        
        /// <summary>
        /// The party that the trainer load's into the battle for the player to fight
        /// </summary>
        public Party opponentParty;

        public NavMeshAgent agent;

        /// <summary>
        /// The ai that the trainer load's into the battle for the player to fight
        /// </summary>
        public EnemyAI ai;

        /// <summary>
        /// Is the trainer defeated
        /// </summary>
        public bool isDefeated;
        private bool _hasTalkedDefeatedText;

        [SerializeField] private Player player;
        [SerializeField] private float walkSpeed;

        [SerializeField] private TextAsset StartBattleText;
        [SerializeField] private TextAsset DefeatedBattleText;

        private GameLoader _gameLoader;

        private void OnValidate()
        {
            if (!_gameLoader)
                _gameLoader = FindObjectOfType<GameLoader>();
            if (!player)
                player = GameObject.Find("Player").GetComponent<Player>();
        }

        private void Register()
        {
            TrainerRegister.RegisterTrainer(this);
        }

        private void Awake()
        {
            Register();
            isDefeated = TrainerRegister.GetIsDefeatedDataFrom(gameObject.name);
        }

        private void Start()
        {
            agent = GetComponent<NavMeshAgent>();

            if(isDefeated) return;
            DialogueFinished += StartingDialogueEnded;
        }

        /// <summary>
        /// Triggers the defeated dialogue
        /// </summary>
        public void Defeated(Vector3 newPos, Quaternion newRot)
        {
            Debug.Log("Defeated");
            isDefeated = true;
            transform.position = newPos;
            transform.rotation = newRot;

            StartCoroutine(StartDefeatedDialogue());
        }

        private IEnumerator StartDefeatedDialogue()
        {
            yield return new WaitForEndOfFrame();
            StartDialogue(DefeatedBattleText);
        }

        private bool isWalking;
        private Vector3 target;
        private bool hasFinishedWalking;
        
        private void Update()
        {
            if (isWalking)
            {
                Debug.Log("Walking");
                transform.position = Vector3.Lerp(transform.position, target, walkSpeed * Time.deltaTime);
            }
        }

        /// <summary>
        /// Starts the battle starting sequence
        /// </summary>
        /// <param name="player">The player that walked in front of the battleStarter</param>
        public void StartBattleStartSequence(Player player)
        {
            Debug.Log("Player walked in front");
            if(!isDefeated && !isWalking && !hasFinishedWalking)
            {
                target = player.transform.position;
                player.LookAtTrainer(transform.position);
                player.GetComponent<PlayerMovement>().canMove = false;
                isWalking = true;
            }
        }

        public void StartBattle()
        {
            if(!isDefeated && isWalking )
            {
                isWalking = false;
                hasFinishedWalking = true;
                
                StartDialogue(StartBattleText);
            }
        }

        private void StartingDialogueEnded(object sender, EventArgs args)
        {
            LoadBattle();
        }

        private void LoadBattle()
        {
            for (int i = 0; i < playerParty.party.Count; i++)
            {
                if (playerParty.party[i])
                {
                    Battler replacementBattler = Battler.CreateCopy(playerParty.party[i]);
                    playerParty.party[i] = replacementBattler;   
                }
            }
            
            for (int i = 0; i < opponentParty.party.Count; i++)
            {
                if (opponentParty.party[i])
                {
                    Battler replacementBattler = Battler.CreateCopy(opponentParty.party[i]);
                    opponentParty.party[i] = replacementBattler;   
                }
            }
            
            DialogueFinished -= StartingDialogueEnded;
            object[] vars = { playerParty, opponentParty, ai, gameObject.name, transform.position, transform.rotation, player.transform.position};
            SceneLoader.LoadScene("Battle", vars);
        }
    }
}