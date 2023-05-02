namespace PokemonGame.Game.World
{
    using Party;
    using Trainers;
    using Global;
    using UnityEngine;

    public class GameLoader : MonoBehaviour
    {
        [SerializeField] private Party party;
    
        private void Awake()
        {
            if(SceneLoader.sceneLoadedFrom == "Battle")
            {
                LoadGameFromBattle();
            }
            else if (SceneLoader.sceneLoadedFrom == "Boot")
            {
                PartyManager.SetPlayerParty(party.Copy());
            }
        }

        private void LoadGameFromBattle()
        {
            PartyManager.SetPlayerParty(SceneLoader.GetVariable<Party>("playerParty"));
            string trainerName = SceneLoader.GetVariable<string>("trainerName");
            Vector3 playerPos = SceneLoader.GetVariable<Vector3>("playerPos");
            Quaternion playerRotation = SceneLoader.GetVariable<Quaternion>("playerRotation");
            bool isDefeated = SceneLoader.GetVariable<bool>("isDefeated");
            
            if (isDefeated)
            {
                GameObject.Find(trainerName).GetComponent<Trainer>().Defeated();
            }
                
            Player.Instance.SetPosRot(playerPos, playerRotation);
        }
    }    
}