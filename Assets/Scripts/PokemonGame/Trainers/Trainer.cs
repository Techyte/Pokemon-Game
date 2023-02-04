using System;
using System.Collections;
using System.Collections.Generic;
using PokemonGame.General;
using PokemonGame.ScriptableObjects;
using UnityEngine;
using UnityEngine.AI;

namespace PokemonGame.Game.Trainers
{
    /// <summary>
    /// Initiates a battle based on certain inspector parameters 
    /// </summary>
    public class Trainer : NPC.Base.NPC
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

        [SerializeField] private TextAsset startBattleText;
        [SerializeField] private TextAsset defeatedBattleText;
        [SerializeField] private TextAsset idleDialogue;

        private GameLoader _gameLoader;

        private bool _isPlayingIdleDialogue;
        
        private void OnValidate()
        {
            if (!_gameLoader)
                _gameLoader = FindObjectOfType<GameLoader>();
            if (!player)
                player = GameObject.Find("Player").GetComponent<Player>();
            if (!agent)
                agent = GetComponent<NavMeshAgent>();
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

        /// <summary>
        /// Starts the battle starting sequence
        /// </summary>
        public void StartBattleStartSequence()
        {
            StartBattle();
        }

        public void StartBattle()
        {
            if (!isDefeated)
            {
                player.LookAtTarget(transform.position);
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
            
            Dictionary<string, object> vars = new Dictionary<string, object>
            {
                { "playerParty", playerParty },
                { "opponentParty", opponentParty },
                { "enemyAI", ai },
                { "opponentName", gameObject.name },
                { "playerPosition", player.transform.position },
                { "playerRotation", player.targetRot }
            };

            SceneLoader.LoadScene("Battle", vars);
        }
    }
}