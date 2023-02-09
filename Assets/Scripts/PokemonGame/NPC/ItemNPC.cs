namespace PokemonGame.NPC
{
    using General;
    using ScriptableObjects;
    using UnityEngine;
    using Global;

    public class ItemNPC : NPC
    {
        [SerializeField] private TextAsset textAsset;

        protected override void OnPlayerInteracted()
        {
            StartDialogue(textAsset);
            base.OnPlayerInteracted();
        }

        public override void CallTag(string tagKey, string tagValue)
        {
            switch (tagKey)
            {
                case "giveItem":
                    string[] secondaryValues = tagValue.Split('.');
                    if (Registry.GetItem(secondaryValues[0], out Item item))
                    {
                        Bag.Instance.Add(item, int.Parse(secondaryValues[1]));
                    }
                    break;
            }
        }
    }
}