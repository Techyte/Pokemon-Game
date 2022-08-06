using PokemonGame.Dialogue;
using UnityEngine;
using PokemonGame.ScriptableObjects;

namespace PokemonGame.Game
{
    public class PokemonCenterNPC : DialogueTrigger
    {
        [Header("Visual Cue")]
        [SerializeField] private GameObject visualCue;

        [SerializeField] private bool playerInRange;
        
        [SerializeField] private BattlerTemplate charmander;
        [SerializeField] private BattlerTemplate bulbasaur;
        [SerializeField] private BattlerTemplate squirtle;

        [SerializeField] private TextAsset TextAsset;
        
        private void Update()
        {
            if (playerInRange)
            {
                visualCue.SetActive(true);
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    StartDialogue(TextAsset);
                }
            }
            else
            {
                visualCue.SetActive(false);
            }
        }

        private void Awake()
        {
            playerInRange = false;
            visualCue.SetActive(false);
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.CompareTag("Player"))
                playerInRange = true;
        }

        private void OnTriggerExit(Collider other)
        {
            if(other.gameObject.CompareTag("Player"))
                playerInRange = false;
        }
        
        /// <summary>
        /// Call a dialogue tag
        /// </summary>
        /// <param name="tagKey">The tag key</param>
        /// <param name="tagValue">The tag value</param>
        public override void CallTag(string tagKey, string tagValue)
        {
            switch (tagKey)
            {
                case "chosenPokemon":
                    ChosePokemon(tagValue);
                    break;
            }
        }

        private void ChosePokemon(string tagValue)
        {
            switch (tagValue)
            {
                case "Charmander": 
                    PartyManager.singleton.AddBattler(Battler.Init(charmander, 5, null, "Charmander", null, null, null, null, true));
                    break;
                case "Squirtle":
                    PartyManager.singleton.AddBattler(Battler.Init(squirtle, 5, null, "Squirtle", null, null, null, null, true));
                    break;
                case "Bulbasaur":
                    PartyManager.singleton.AddBattler(Battler.Init(bulbasaur, 5, null, "Bulbasaur", null, null, null, null, true));
                    break;
            }
        }
    }   
}
