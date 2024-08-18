using PokemonGame.Global;
using PokemonGame.NPC;
using UnityEngine;

namespace PokemonGame.Battle
{
    public class PokeCenterForceHeal : MonoBehaviour
    {
        [SerializeField] private NuseJoyNPC nurseJoy;
        
        private void Start()
        {
            if (SceneLoader.sceneLoadedFrom == "Battle")
            {
                nurseJoy.ForceHealDialogue();
            }
        }
    }
}
