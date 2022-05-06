using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace PokemonGame.Battle
{
    public class BattleUIManager : MonoBehaviour
    {
        public GameObject moveButtons;
        public GameObject healthDisplays;
        public GameObject changeBattlerDisplay;
        public GameObject miscButtons;
        public GameObject backButton;
        public TextMeshProUGUI[] battlerDisplays;
        public SpriteRenderer currentBattlerRenderer;
        public SpriteRenderer apponentBattlerRenderer;
        public Slider currentBattlerHealthDisplay;
        public Slider apponentHealthDisplay;
        public TextMeshProUGUI[] moveTexts;
        public TextMeshProUGUI currentBattlerNameDisplay;
        public TextMeshProUGUI apponentBattlerNameDisplay;

        public Battle battle;

        private void Start()
        {
            UpdateBattlerSprites();
            UpdateBattlerMoveDisplays();
            UpdateHealthDisplays();
            UpdateBattlerTexts();
        }

        public void UpdateBattlerButtons()
        {
            for (int i = 0; i < battlerDisplays.Length; i++)
            {
                battlerDisplays[i].transform.parent.gameObject.SetActive(false);
            }

            for (int i = 0; i < battle.playerParty.party.Length; i++)
            {
                if (battle.playerParty.party[i].name == "")
                {
                    battlerDisplays[i].transform.parent.gameObject.SetActive(false);
                }
                else
                {
                    battlerDisplays[i].transform.parent.gameObject.SetActive(true);
                    battlerDisplays[i].text = battle.playerParty.party[i].name;
                    battlerDisplays[i].transform.parent.GetComponent<Button>().interactable = !battle.playerParty.party[i].isFainted;
                }
            }
        }

        public void SwitchBattler()
        {
            healthDisplays.SetActive(false);
            moveButtons.SetActive(false);
            miscButtons.SetActive(false);
            changeBattlerDisplay.SetActive(true);
            backButton.SetActive(true);
            UpdateBattlerButtons();
        }

        public void SwitchBattlerBecauseOfDeath()
        {
            healthDisplays.SetActive(false);
            moveButtons.SetActive(false);
            miscButtons.SetActive(false);
            changeBattlerDisplay.SetActive(true);
            backButton.SetActive(false);
            UpdateBattlerButtons();
        }

        public void ChangeBattler(int partyID)
        {
            healthDisplays.SetActive(true);
            moveButtons.SetActive(true);
            miscButtons.SetActive(true);
            changeBattlerDisplay.SetActive(false);
            battle.currentBattler = battle.playerParty.party[partyID];
            battle.ChangeTurn();
            UpdateBattlerButtons();
            UpdateBattlerSprites();
            UpdateBattlerMoveDisplays();
            UpdateHealthDisplays();
            UpdateBattlerTexts();
        }

        public void Back()
        {
            healthDisplays.SetActive(true);
            moveButtons.SetActive(true);
            miscButtons.SetActive(true);
            changeBattlerDisplay.SetActive(false);
            UpdateBattlerButtons();
        }

        public void UpdateBattlerTexts()
        {
            currentBattlerNameDisplay.text = battle.currentBattler.name;
            apponentBattlerNameDisplay.text = battle.apponentBattler.name;
        }

        public void UpdateBattlerSprites()
        {
            currentBattlerRenderer.sprite = battle.currentBattler.texture;
            apponentBattlerRenderer.sprite = battle.apponentBattler.texture;
        }

        public void UpdateBattlerMoveDisplays()
        {
            for (int i = 0; i < moveTexts.Length; i++)
            {
                moveTexts[i].transform.parent.gameObject.SetActive(false);
            }

            for (int i = 0; i < battle.currentBattler.moves.Length; i++)
            {
                if (battle.currentBattler.moves[i])
                {
                    moveTexts[i].transform.parent.gameObject.SetActive(true);
                    moveTexts[i].text = battle.currentBattler.moves[i].name;
                }
            }
        }

        public void UpdateHealthDisplays()
        {
            apponentHealthDisplay.maxValue = battle.apponentBattler.maxHealth;
            apponentHealthDisplay.value = battle.apponentBattler.currentHealth;

            currentBattlerHealthDisplay.maxValue = battle.currentBattler.maxHealth;
            currentBattlerHealthDisplay.value = battle.currentBattler.currentHealth;
        }
    }

}