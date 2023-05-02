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
                    if (Registry.GetItem(tagValues[0], out Item item))
                    {
                        Bag.Add(item, int.Parse(tagValues[1]));
                    }
                    break;
            }
        }
    }
}