using PokemonGame.General;
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
                        Bag.Instance.Add(item, int.Parse(tagValues[1]));
                    }
                    break;
            }
        }
    }
}