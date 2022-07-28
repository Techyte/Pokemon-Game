using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace PokemonGame.Battle
{
    public class BattleUIManager : MonoBehaviour
    {
        public GameObject playerUIHolder;
        public GameObject moveButtons;
        public GameObject healthDisplays;
        public GameObject changeBattlerDisplay;
        public GameObject miscButtons;
        public GameObject backButton;
        public TextMeshProUGUI[] battlerDisplays;
        public SpriteRenderer currentBattlerRenderer;
        public SpriteRenderer opponentBattlerRenderer;
        public Slider currentBattlerHealthDisplay;
        public Slider opponentHealthDisplay;
        public TextMeshProUGUI[] moveTexts;
        public TextMeshProUGUI currentBattlerNameDisplay;
        public TextMeshProUGUI opponentBattlerNameDisplay;

        public Battle battle;

        private void Start()
        {
            UpdateBattlerSprites();
            UpdateBattlerMoveDisplays();
            UpdateHealthDisplays();
            UpdateBattlerTexts();
            UpdateBattlerButtons();
        }

        private void UpdateBattlerButtons()
        {
            foreach (var text in battlerDisplays)
            {
                text.transform.parent.gameObject.SetActive(false);
            }

            for (int i = 0; i < battle.playerParty.party.Count; i++)
            {
                if (!battle.playerParty.party[i])
                {
                    battlerDisplays[i].transform.parent.gameObject.SetActive(false);
                }
                else
                {
                    battlerDisplays[i].transform.parent.gameObject.SetActive(true);
                    battlerDisplays[i].text = battle.playerParty.party[i].name;
                    battlerDisplays[i].transform.parent.GetComponent<Button>().interactable = !battle.playerParty.party[i].isFainted;

                    if (i == battle.currentBattlerIndex)
                    {
                        battlerDisplays[i].text = battle.playerParty.party[i].name + " is selected";
                        battlerDisplays[i].transform.parent.GetComponent<Button>().interactable = false;
                    }

                    if (battle.playerParty.party[i].isFainted)
                    {
                        battlerDisplays[i].text = battle.playerParty.party[i].name + " is fainted";
                        battlerDisplays[i].transform.parent.GetComponent<Button>().interactable = false;
                    }
                }
            }
        }

        public void ShowUI(bool showUIBool)
        {
            playerUIHolder.SetActive(showUIBool);
        }

        public void SwitchBattler()
        {
            playerUIHolder.SetActive(true);
            healthDisplays.SetActive(false);
            moveButtons.SetActive(false);
            miscButtons.SetActive(false);
            changeBattlerDisplay.SetActive(true);
            backButton.SetActive(true);
            UpdateBattlerButtons();
        }

        public void SwitchBattlerBecauseOfDeath()
        {
            playerUIHolder.SetActive(true);
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
            battle.currentBattlerIndex = partyID;
            battle.playerParty.party[battle.currentBattlerIndex] = battle.playerParty.party[partyID];
            Back();
            battle.currentTurn = TurnStatus.Showing;
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

        private void UpdateBattlerTexts()
        {
            currentBattlerNameDisplay.text = battle.playerParty.party[battle.currentBattlerIndex].name;
            opponentBattlerNameDisplay.text = battle.opponentParty.party[battle.opponentBattlerIndex].name;
        }

        private void UpdateBattlerSprites()
        {
            currentBattlerRenderer.sprite = battle.playerParty.party[battle.currentBattlerIndex].texture;
            opponentBattlerRenderer.sprite = battle.opponentParty.party[battle.opponentBattlerIndex].texture;
        }

        private void UpdateBattlerMoveDisplays()
        {
            foreach (var text in moveTexts)
            {
                text.transform.parent.gameObject.SetActive(false);
            }

            for (var i = 0; i < battle.playerParty.party[battle.currentBattlerIndex].moves.Count; i++)
            {
                if (battle.playerParty.party[battle.currentBattlerIndex].moves[i])
                {
                    moveTexts[i].transform.parent.gameObject.SetActive(true);
                    moveTexts[i].text = battle.playerParty.party[battle.currentBattlerIndex].moves[i].name;   
                }
            }
        }

        public void UpdateHealthDisplays()
        {
            opponentHealthDisplay.maxValue = battle.opponentParty.party[battle.opponentBattlerIndex].maxHealth;
            opponentHealthDisplay.value = battle.opponentParty.party[battle.opponentBattlerIndex].currentHealth;

            currentBattlerHealthDisplay.maxValue = battle.playerParty.party[battle.currentBattlerIndex].maxHealth;
            currentBattlerHealthDisplay.value = battle.playerParty.party[battle.currentBattlerIndex].currentHealth;
        }
    }

}