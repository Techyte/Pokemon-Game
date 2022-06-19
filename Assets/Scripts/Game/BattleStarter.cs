using PokemonGame.Battle;
using UnityEngine;
using UnityEngine.AI;

namespace PokemonGame.Game
{
    public class BattleStarter : MonoBehaviour
    {
        public BattlerTemplate[] playerPartyTemplate;
        public BattlerTemplate[] npcStarterPokemon;
        public AllStatusEffects allStatusEffects;
        public AllMoves allMoves;
        public Party playerParty;
        public Party apponentParty;

        public NavMeshAgent agent;

        public EnemyAI ai;

        private bool hasStartedwalking;
        public bool isDefeated;

        [SerializeField] private Transform player;
        [SerializeField] private Transform playerSpawnPos;

        private void Start()
        {
            agent = gameObject.GetComponent<NavMeshAgent>();
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward), Color.white, 100);
        }

        public void Defeated()
        {
            Debug.Log("I have been defeated");
        }

        private void Update()
        {
            RaycastHit hit;
            if(Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
            {
                if (!isDefeated)
                {
                    hit.transform.gameObject.GetComponent<PlayerMovement>().canMove = false;
                    agent.destination = hit.transform.position;

                    if (hit.transform.gameObject.GetComponent<Player>() != null)
                    {
                        hit.transform.gameObject.GetComponent<Player>().LookAtTrainer(transform.position);
                    }

                    Invoke("HasStartedWalkingSetter", .1f);

                    if (agent.velocity.magnitude < 0.15f && hasStartedwalking)
                    {
                        LoadBattle();
                    }
                }
                else
                {
                    hit.transform.gameObject.GetComponent<PlayerMovement>().canMove = true;
                }
            }
        }

        private void HasStartedWalkingSetter()
        {
            hasStartedwalking = true;
        }

        public void LoadBattle()
        {
            playerParty.party[0] = Battler.Init(
                playerPartyTemplate[0],
                5,
                allStatusEffects.effects["Healthy"],
                playerPartyTemplate[0].name,
                allMoves.moves["Ember"],
                allMoves.moves["Tackle"],
                allMoves.moves["Toxic"],
                null,
                true);

            playerParty.party[1] = Battler.Init(
                playerPartyTemplate[1],
                5,
                allStatusEffects.effects["Healthy"],
                playerPartyTemplate[1].name,
                allMoves.moves["Tackle"],
                allMoves.moves["Toxic"],
                null,
                null,
                true);

            apponentParty.party[0] = Battler.Init(
                npcStarterPokemon[0],
                5,
                allStatusEffects.effects["Healthy"],
                npcStarterPokemon[0].name,
                allMoves.moves["Tackle"],
                allMoves.moves["RazorLeaf"],
                null,
                null,
                true);

            GameWorldData.playerTransform = playerSpawnPos.position;
            BattleManager.LoadScene(playerParty, apponentParty, ai, 1);
        }
    }
}
