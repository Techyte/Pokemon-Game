using System;
using System.Collections;
using System.Collections.Generic;
using PokemonGame.Dialogue;
using PokemonGame.ScriptableObjects;
using UnityEngine;
using UnityEngine.AI;

namespace PokemonGame.Game
{
    /// <summary>
    /// Initiates a battle based on certain inspector parameters 
    /// </summary>
    public class BattleStarter : DialogueTrigger
    {
        public AllStatusEffects allStatusEffects;
        public AllMoves allMoves;
        
        public Party playerParty;
        
        /// <summary>
        /// The party that the battleStarter load's into the battle for the player to fight
        /// </summary>
        public Party opponentParty;

        public NavMeshAgent agent;

        /// <summary>
        /// The ai that the battleStarter load's into the battle for the player to fight
        /// </summary>
        public EnemyAI ai;

        /// <summary>
        /// Is the battleStarter defeated
        /// </summary>
        public bool isDefeated;
        private bool _hasTalkedDefeatedText;

        private bool hasFinishedStartText;

        [SerializeField] private Transform playerSpawnPos;
        
        /// <summary>
        /// Id of the battleStarter
        /// </summary>
        public int battlerId;

        [SerializeField] private TextAsset StartBattleText;
        [SerializeField] private TextAsset DefeatedBattleText;

        private GameLoader _gameLoader;

        private void OnValidate()
        {
            Register();
            if (!_gameLoader)
                _gameLoader = FindObjectOfType<GameLoader>();
        }

        private void Awake()
        {
            Register();
        }

        private void Register()
        {
            if (battlerId == 0)
            {
                int newBattlerId = BattleStarterRegister.BattleStarters.Count+1;
                BattleStarterRegister.BattleStarters.Add(this);
                battlerId = newBattlerId;   
            }
        }

        private void Start()
        {
            agent = GetComponent<NavMeshAgent>();

            DialogueFinished += StartingDialogueEnded;
        }

        /// <summary>
        /// Triggers the defeated dialogue
        /// </summary>
        public void Defeated()
        {
            isDefeated = true;
            DialogueFinished -= StartingDialogueEnded;
            StartCoroutine(StartDefeatedDialogue());
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
            
            object[] vars = { playerParty, opponentParty, ai, playerSpawnPos.position, battlerId, transform.position};
            SceneLoader.LoadScene(1, vars);
        }
    }

    /// <summary>
    /// The register that holds all battle starters
    /// </summary>
    public static class BattleStarterRegister
    {
        public static readonly List<BattleStarter> BattleStarters = new List<BattleStarter>();
    }
}
