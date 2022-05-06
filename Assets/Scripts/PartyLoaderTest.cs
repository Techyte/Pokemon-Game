using UnityEngine;
using PokemonGame.Battle;

namespace PokemonGame
{
    public class PartyLoaderTest : MonoBehaviour
    {
        public BattlerTemplate[] playerPartyTemplate;
        public BattlerTemplate[] apponentPartyTemplate;

        public AllMoves allMoves;
        public AllStatusEffects allStatusEffects;
        public AllAis allAis;

        public Party playerParty;
        public Party apponentParty;

        private void Awake()
        {
            LoadBattle();
        }

        private void LoadBattle()
        {
            playerParty.party[0] = new Battler(
                playerPartyTemplate[0],
                5,
                allStatusEffects.effects["Healthy"],
                playerPartyTemplate[0].name,
                allMoves.moves["Ember"],
                allMoves.moves["Tackle"],
                allMoves.moves["Toxic"],
                null);

            playerParty.party[1] = new Battler(
                playerPartyTemplate[1],
                5,
                allStatusEffects.effects["Healthy"],
                playerPartyTemplate[1].name,
                allMoves.moves["Tackle"],
                allMoves.moves["Toxic"],
                null,
                null);

            apponentParty.party[0] = new Battler(
                apponentPartyTemplate[0],
                5,
                allStatusEffects.effects["Healthy"],
                apponentPartyTemplate[0].name,
                allMoves.moves["Tackle"],
                allMoves.moves["Razor Leaf"],
                allMoves.moves["Toxic"],
                null);

            string playerPath = Application.persistentDataPath + "/party.json";
            string aponentPath = Application.persistentDataPath + "/apponentTestParty.json";

            SaveAndLoad<Party>.SaveJson(playerParty, playerPath);
            SaveAndLoad<Party>.SaveJson(apponentParty, aponentPath);

            BattleManager.LoadBattleScene(SaveAndLoad<Party>.LoadJson(playerPath), SaveAndLoad<Party>.LoadJson(aponentPath), allAis.ais["DefaultAI"]);
        }
    }

}