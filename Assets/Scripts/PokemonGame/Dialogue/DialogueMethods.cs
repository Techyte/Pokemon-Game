using PokemonGame.Game;
using PokemonGame.Global;
using PokemonGame.ScriptableObjects;

namespace PokemonGame.Dialogue
{
    public class DialogueMethods
    {
        public void HandleGlobalTag(string tagKey, string[] tagValues)
        {
            switch (tagKey)
            {
                case "giveItem":
                    Bag.Add(Registry.GetItem(tagValues[0]), int.Parse(tagValues[1]));
                    break;
            }
        }
    }
}