using UnityEngine;

namespace PokemonGame.Battle
{
    public class PartyManager : MonoBehaviour
    {
        private static PartyManager _singleton;

        public static PartyManager singleton
        {
            get => _singleton;
            private set
            {
                if (_singleton == null)
                    _singleton = value;
                else if (_singleton != value)
                {
                    Debug.Log($"{nameof(PartyManager)} instance already exists, destroying duplicate!");
                    Destroy(value);
                }
            }
        }
        
        [SerializeField] private Party playerParty;

        private void Awake()
        {
            singleton = this;
        }

        public void AddPokemon(Battler battlerToAdd)
        {
            playerParty.party.Add(battlerToAdd);
        }
        
        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }
    }   
}
