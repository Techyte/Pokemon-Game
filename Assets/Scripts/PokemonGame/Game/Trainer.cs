using System;
using System.Collections;
using PokemonGame.Dialogue;
using PokemonGame.ScriptableObjects;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

namespace PokemonGame.Game
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

        private bool hasFinishedStartText;

        [SerializeField] private Transform playerSpawnPos;
        
        /// <summary>
        /// Id of the battleStarter
        /// </summary>
        public int id;

        [SerializeField] private TextAsset StartBattleText;
        [SerializeField] private TextAsset DefeatedBattleText;

        private GameLoader _gameLoader;

        private bool _hasRecentlyDefeated;
        private Vector3 _defeatedNewPos;

        private void OnValidate()
        {
            if (!_gameLoader)
                _gameLoader = FindObjectOfType<GameLoader>();
            EditorUtility.SetDirty(gameObject);
            Register();
        }

        private void Register()
        {
            if (!Application.isPlaying) return;
            
            if (TrainerRegister.GetBattleStarter(id) == null)
            {
                id = TrainerRegister.AddBattleStarter(this);
            }
            else
            {
                TrainerInfo info = TrainerRegister.GetBattleStarter(id);
                isDefeated = info.isDefeated;
                transform.position = info.position;
            }
        }

        private void Start()
        {
            agent = GetComponent<NavMeshAgent>();

            DialogueFinished += StartingDialogueEnded;
            
            GetData();
        }

        private void GetData()
        {
            Debug.Log("Got data");
            if (TrainerRegister.GetBattleStarter(id) != null)
            {
                Debug.Log("Was not null");
                transform.position = TrainerRegister.GetBattleStarter(id).position;
                isDefeated = TrainerRegister.GetBattleStarter(id).isDefeated;

                if (_hasRecentlyDefeated)
                {
                    Debug.Log("Has recently defeated");
                    isDefeated = true;
                    DialogueFinished -= StartingDialogueEnded;
                    transform.position = _defeatedNewPos;
                    StartCoroutine(StartDefeatedDialogue());
                    Debug.Log(TrainerRegister.GetBattleStarter(id));
                    TrainerRegister.GetBattleStarter(id).UpdatePosition(_defeatedNewPos);
                    TrainerRegister.GetBattleStarter(id).UpdateIsDefeated(isDefeated);
                }
            }
        }

        /// <summary>
        /// Triggers the defeated dialogue
        /// </summary>
        public void Defeated(Vector3 newPos)
        {
            Debug.Log("Defeated");
            _hasRecentlyDefeated = true;
            _defeatedNewPos = newPos;
        }

        private IEnumerator StartDefeatedDialogue()
        {
            yield return new WaitForEndOfFrame();
            StartDialogue(DefeatedBattleText);
            _gameLoader.player.LookAtTrainer(transform.position);
        }

        private bool hasStartedWalking;
        private bool hasStartedTalkingStartText;

        /// <summary>
        /// Starts the battle starting sequence
        /// </summary>
        /// <param name="player">The player that walked in front of the battleStarter</param>
        public void StartBattleSequence(Player player)
        {
            if(!isDefeated)
            {
                PlayerMovement playerMovement = player.transform.gameObject.GetComponent<PlayerMovement>();

                playerMovement.battleStarterHasStartedWalking = true;
                agent.destination = player.transform.position;

                player.LookAtTrainer(transform.position);

                if (agent.velocity.magnitude > 0.15f)
                {
                    hasStartedWalking = true;
                }

                if (hasStartedWalking)
                {
                    if (agent.velocity.magnitude < 0.15f && !hasStartedTalkingStartText)
                    {
                        StartDialogue(StartBattleText);
                        hasStartedTalkingStartText = true;
                    }
                }

                if (hasFinishedStartText)
                {
                    LoadBattle();
                }
            }
        }

        private void StartingDialogueEnded(object sender, EventArgs args)
        {
            hasFinishedStartText = true;
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
            
            object[] vars = { playerParty, opponentParty, ai, playerSpawnPos.position, id, transform.position};
            SceneLoader.LoadScene("Battle", vars);
        }
    }
}