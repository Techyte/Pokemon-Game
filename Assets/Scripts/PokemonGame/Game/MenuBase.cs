namespace PokemonGame.Game
{
    using UnityEngine;
    
    public class MenuBase : MonoBehaviour
    {
        [SerializeField] private GameObject menuUiObject;

        public void Open()
        {
            menuUiObject.SetActive(true);
        }
        
        public void Close()
        {
            menuUiObject.SetActive(false);
        }
    }
}