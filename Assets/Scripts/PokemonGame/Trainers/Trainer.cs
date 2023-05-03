namespace PokemonGame.Trainers
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using General;
    using Global;
    using ScriptableObjects;
    using UnityEngine;
    using Game.Party;
    using Game;
    using Game.World;
    using NPC;

    /// <summary>
    /// Initiates a battle based on certain inspector parameters 
    /// </summary>
    public class Trainer : NPC
    {
        /// <summary>
        /// Is the trainer defeated
        /// </summary>
        [Space] [Header("Defeated")] public bool isDefeated;
        
        /// <summary>
        /// The party that the trainer load's into the battle for the player to fight
        /// </summary>
        [Space] [Header("Party")] public Party party;

        /// <summary>
        /// The ai that the trainer load's into the battle for the player to fight
        /// </summary>
        [Space] [Header("AI")] public EnemyAI ai;

        [Space]
        [Header("Dialogue")]
        [SerializeField] private TextAsset startBattleText;
        [SerializeField] private TextAsset defeatedBattleText;
        [SerializeField] private TextAsset idleDialogue;

        
        private GameLoader _gameLoader;

        private bool _isPlayingIdleDialogue;
        
        private bool _hasTalkedDefeatedText;
        
        private void OnValidate()
        {
            if (!_gameLoader)
                _gameLoader = FindObjectOfType<GameLoader>();
        }

        private void Awake()
        {
            isDefeated = TrainerRegister.IsDefeated(this);
        }

        private void Start()
        {
            interactable = isDefeated;
            DialogueFinished += DialogueEnded;
        }

        private void Update()
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity))
            {
                if (hit.transform.GetComponent<Player>())
                {
                    StartBattle();   
                }
            }
        }
        
        /// <summary>
        /// Triggers the defeated dialogue
        /// </summary>
        public void Defeated()
        {
            isDefeated = true;
            
            TrainerRegister.Defeated(this);
            
            StartCoroutine(StartDefeatedDialogue());
        }

        private IEnumerator StartDefeatedDialogue()
        {
            yield return new WaitForEndOfFrame();
            StartDialogue(defeatedBattleText);
        }

        protected override void OnPlayerInteracted()
        {
            StartDialogue(idleDialogue);
            base.OnPlayerInteracted();
        }

        private void StartBattle()
        {
            if (!isDefeated)
            {
                Player.Instance.LookAtTarget(transform.position);
                StartDialogue(startBattleText);
            }
        }

        private void DialogueEnded(object sender, EventArgs args)
        {
            if(!isDefeated)
            {
                LoadBattle();
            }
            else if(_isPlayingIdleDialogue)
            {
                _isPlayingIdleDialogue = false;
            }
            else
            {
                interactable = true;
            }
        }

        private void LoadBattle()
        {
            for (int i = 0; i < party.Count; i++)
            {
                if (party[i])
                {
                    Battler replacementBattler = Battler.CreateCopy(party[i]);
                    party[i] = replacementBattler;   
                }
            }

            Party playerParty = PartyManager.GetParty();

            Battler charmander = Registry.GetBattler("Player Charmander");

            if (playerParty == null)
            {
                Debug.Log("Player party does not exist");
                playerParty = new Party();
                
                playerParty.Add(Battler.CreateCopy(charmander));
            }else if (playerParty.Count == 0)
            {
                Debug.Log("Player party is at 0");
                playerParty.Add(Battler.CreateCopy(charmander));
            }
            
            Dictionary<string, object> vars = new Dictionary<string, object>
            {
                { "playerParty", playerParty},
                { "opponentParty", party },
                { "enemyAI", ai },
                { "opponentName", gameObject.name },
                { "playerPosition", Player.Instance.transform.position },
                { "playerRotation", Player.Instance.targetRot }
            };

            SceneLoader.LoadScene("Battle", vars);
        }
    }
}