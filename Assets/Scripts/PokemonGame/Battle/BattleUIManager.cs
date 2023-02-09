namespace PokemonGame.Battle
{
    using UnityEngine;
    using TMPro;
    using UnityEngine.UI;   

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

            for (int i = 0; i < battle.playerParty.Count; i++)
            {
                if (!battle.playerParty[i])
                {
                    battlerDisplays[i].transform.parent.gameObject.SetActive(false);
                }
                else
                {
                    battlerDisplays[i].transform.parent.gameObject.SetActive(true);
                    battlerDisplays[i].text = battle.playerParty[i].name;
                    battlerDisplays[i].transform.parent.GetComponent<Button>().interactable = !battle.playerParty[i].isFainted;

                    if (i == battle.currentBattlerIndex)
                    {
                        battlerDisplays[i].text = battle.playerParty[i].name + " is selected";
                        battlerDisplays[i].transform.parent.GetComponent<Button>().interactable = false;
                    }

                    if (battle.playerParty[i].isFainted)
                    {
                        battlerDisplays[i].text = battle.playerParty[i].name + " is fainted";
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
            battle.playerParty[battle.currentBattlerIndex] = battle.playerParty[partyID];
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
            currentBattlerNameDisplay.text = battle.playerParty[battle.currentBattlerIndex].name;
            opponentBattlerNameDisplay.text = battle.opponentParty[battle.opponentBattlerIndex].name;
        }

        private void UpdateBattlerSprites()
        {
            Debug.Log("Second");
            currentBattlerRenderer.sprite = battle.playerParty[battle.currentBattlerIndex].texture;
            opponentBattlerRenderer.sprite = battle.opponentParty[battle.opponentBattlerIndex].texture;
        }

        private void UpdateBattlerMoveDisplays()
        {
            foreach (var text in moveTexts)
            {
                text.transform.parent.gameObject.SetActive(false);
            }

            for (var i = 0; i < battle.playerParty[battle.currentBattlerIndex].moves.Count; i++)
            {
                if (battle.playerParty[battle.currentBattlerIndex].moves[i])
                {
                    moveTexts[i].transform.parent.gameObject.SetActive(true);
                    moveTexts[i].text = battle.playerParty[battle.currentBattlerIndex].moves[i].name;   
                }
            }
        }

        public void UpdateHealthDisplays()
        {
            opponentHealthDisplay.maxValue = battle.opponentParty[battle.opponentBattlerIndex].maxHealth;
            opponentHealthDisplay.value = battle.opponentParty[battle.opponentBattlerIndex].currentHealth;

            currentBattlerHealthDisplay.maxValue = battle.playerParty[battle.currentBattlerIndex].maxHealth;
            currentBattlerHealthDisplay.value = battle.playerParty[battle.currentBattlerIndex].currentHealth;
        }
    }
}