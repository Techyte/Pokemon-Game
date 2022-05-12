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
        public Party playerParty;
        public Party apponentParty;

        public Move Ember;
        public Move Tackle;
        public Move Toxic;
        public Move RazorLeaf;

        public NavMeshAgent agent;

        public EnemyAI ai;

        private bool hasStartedwalking;
        private bool isDefeated;

        private void Start()
        {
            agent = gameObject.GetComponent<NavMeshAgent>();
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward), Color.white, 100);
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
            playerParty.party[0] = new Battler(
                playerPartyTemplate[0],
                5,
                allStatusEffects.effects["Healthy"],
                playerPartyTemplate[0].name,
                Ember,
                Tackle,
                Toxic,
                null);

            playerParty.party[1] = new Battler(
                playerPartyTemplate[1],
                5,
                allStatusEffects.effects["Healthy"],
                playerPartyTemplate[1].name,
                Tackle,
                Toxic,
                null,
                null);

            apponentParty.party[0] = new Battler(
                npcStarterPokemon[0],
                5,
                allStatusEffects.effects["Healthy"],
                npcStarterPokemon[0].name,
                Tackle,
                RazorLeaf,
                null,
                null);

            BattleManager.LoadBattleScene(playerParty, apponentParty, ai);
        }
    }
}
