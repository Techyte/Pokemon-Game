using PokemonGame.Battle;
using UnityEngine;
using UnityEngine.AI;

namespace PokemonGame.Game
{
    public class BattleStarter : MonoBehaviour
    {
        public AllStatusEffects allStatusEffects;
        public AllMoves allMoves;
        public Party playerParty;
        public Party opponentParty;

        public NavMeshAgent agent;

        public EnemyAI ai;

        private bool _hasStartedWalking;
        public bool isDefeated;
        private bool _hasTalkedDefeatedText;

        [SerializeField] private Transform playerSpawnPos;

        private void Start()
        {
            agent = gameObject.GetComponent<NavMeshAgent>();
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward), Color.white, 100);
        }

        public void Defeated()
        {
            _hasTalkedDefeatedText = true;
        }

        private void Update()
        {
            if(Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out RaycastHit hit, Mathf.Infinity))
            {
                if (!isDefeated)
                {
                    hit.transform.gameObject.GetComponent<PlayerMovement>().canMove = false;
                    agent.destination = hit.transform.position;

                    if (hit.transform.gameObject.GetComponent<Player>() != null)
                    {
                        hit.transform.gameObject.GetComponent<Player>().LookAtTrainer(transform.position);
                    }

                    Invoke(nameof(HasStartedWalkingSetter), .1f);
                    
                    if (agent.velocity.magnitude < 0.15f && _hasStartedWalking)
                    {
                        LoadBattle();
                    }
                }
                else
                {
                    if(isDefeated && !_hasTalkedDefeatedText)
                        Defeated();
                }
            }
        }

        private void HasStartedWalkingSetter()
        {
            _hasStartedWalking = true;
        }

        private void LoadBattle()
        {
            GameWorldData.playerTransform = playerSpawnPos.position;
            GameWorldData.battleStarterName = name;
            object[] vars = { playerParty, opponentParty, ai};
            SceneLoader.LoadScene(1, vars);
        }
    }
}
