namespace PokemonGame.Battle
{
    using UnityEngine;
    using TMPro;
    using UnityEngine.UI;   

    public class BattleUIManager : MonoBehaviour
    {
        [SerializeField] private float healthUpdateSpeed = 0.8f;
        
        [Space]
        [SerializeField] private GameObject playerUIHolder;
        [SerializeField] private GameObject controlUIHolder;
        [SerializeField] private GameObject moveButtons;
        [SerializeField] private GameObject healthDisplays;
        [SerializeField] private GameObject changeBattlerDisplay;
        [SerializeField] private GameObject useItemDisplay;
        [SerializeField] private GameObject miscButtons;
        [SerializeField] private GameObject backButton;
        [SerializeField] private TextMeshProUGUI[] battlerDisplays;
        [SerializeField] private SpriteRenderer currentBattlerRenderer;
        [SerializeField] private SpriteRenderer opponentBattlerRenderer;
        [SerializeField] private Slider currentBattlerHealthDisplay;
        [SerializeField] private Slider opponentHealthDisplay;
        [SerializeField] private TextMeshProUGUI[] moveTexts;
        [SerializeField] private TextMeshProUGUI currentBattlerNameDisplay;
        [SerializeField] private TextMeshProUGUI opponentBattlerNameDisplay;
        [SerializeField] private BattleBagMenu battleBagMenu;

        public Battle battle;

        private void Start()
        {
            UpdateBattlerSprites();
            UpdateBattlerMoveDisplays();
            UpdateBattlerTexts();
            UpdateBattlerButtons();
            
            opponentHealthDisplay.maxValue = battle.opponentParty[battle.opponentBattlerIndex].maxHealth;
            opponentHealthDisplay.value = battle.opponentParty[battle.opponentBattlerIndex].currentHealth;

            currentBattlerHealthDisplay.maxValue = battle.playerParty[battle.currentBattlerIndex].maxHealth;
            currentBattlerHealthDisplay.value = battle.playerParty[battle.currentBattlerIndex].currentHealth;
        }

        private void Update()
        {
            UpdateHealthDisplays();
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
                    battlerDisplays[i].color = Color.black;

                    if (i == battle.currentBattlerIndex)
                    {
                        battlerDisplays[i].text = battle.playerParty[i].name + " is selected";
                        battlerDisplays[i].transform.parent.GetComponent<Button>().interactable = false;
                        battlerDisplays[i].color = Color.blue;
                    }

                    if (battle.playerParty[i].isFainted)
                    {
                        battlerDisplays[i].text = battle.playerParty[i].name + " is fainted";
                        battlerDisplays[i].transform.parent.GetComponent<Button>().interactable = false;
                        battlerDisplays[i].color = Color.red;
                    }
                }
            }
        }

        public void ShowUI(bool show)
        {
            playerUIHolder.SetActive(show);
            ShowControlUI(show);
            healthDisplays.SetActive(show);
        }

        public void ShowControlUI(bool show)
        {
            controlUIHolder.SetActive(show);
        }

        public void SwitchBattler()
        {
            playerUIHolder.SetActive(true);
            healthDisplays.SetActive(false);
            moveButtons.SetActive(false);
            miscButtons.SetActive(false);
            changeBattlerDisplay.SetActive(true);
            useItemDisplay.SetActive(false);
            backButton.SetActive(true);
            UpdateBattlerButtons();
        }

        public void UseItem()
        {
            playerUIHolder.SetActive(true);
            healthDisplays.SetActive(false);
            moveButtons.SetActive(false);
            miscButtons.SetActive(false);
            useItemDisplay.SetActive(true);
            backButton.SetActive(true);
            UpdateBattlerButtons();
            battleBagMenu.UpdateBagUI();
        }

        public void SwitchBattlerBecauseOfDeath()
        {
            playerUIHolder.SetActive(true);
            controlUIHolder.SetActive(true);
            healthDisplays.SetActive(false);
            moveButtons.SetActive(false);
            miscButtons.SetActive(false);
            changeBattlerDisplay.SetActive(true);
            backButton.SetActive(false);
            useItemDisplay.SetActive(false);
            UpdateBattlerButtons();
        }

        public void ChangeBattler(int partyID)
        {
            playerUIHolder.SetActive(false);
            healthDisplays.SetActive(true);
            moveButtons.SetActive(true);
            miscButtons.SetActive(true);
            changeBattlerDisplay.SetActive(false);
            useItemDisplay.SetActive(false);
            battle.ChooseToSwap(partyID);
            battle.AddParticipatedBattler(battle.playerParty[partyID]);
            Back();
        }

        public void Back()
        {
            healthDisplays.SetActive(true);
            moveButtons.SetActive(true);
            miscButtons.SetActive(true);
            changeBattlerDisplay.SetActive(false);
            useItemDisplay.SetActive(false);
            UpdateBattlerButtons();
        }

        private void UpdateBattlerTexts()
        {
            currentBattlerNameDisplay.text = battle.playerParty[battle.currentBattlerIndex].name;
            opponentBattlerNameDisplay.text = battle.opponentParty[battle.opponentBattlerIndex].name;
            
            currentBattlerNameDisplay.color = Color.white;
            if (battle.playerParty[battle.currentBattlerIndex].isFainted)
            {
                currentBattlerNameDisplay.text = battle.playerParty[battle.currentBattlerIndex].name + " (FAINTED)";
                currentBattlerNameDisplay.color = Color.red;
            }
            
            opponentBattlerNameDisplay.color = Color.white;
            if (battle.opponentParty[battle.opponentBattlerIndex].isFainted)
            {
                opponentBattlerNameDisplay.text = battle.opponentParty[battle.opponentBattlerIndex].name + " (FAINTED)";
                opponentBattlerNameDisplay.color = Color.red;
            }
        }

        private void UpdateBattlerSprites()
        {
            currentBattlerRenderer.sprite = battle.playerParty[battle.currentBattlerIndex].texture;
            opponentBattlerRenderer.sprite = battle.opponentParty[battle.opponentBattlerIndex].texture;
            
            if (battle.playerParty[battle.currentBattlerIndex].isFainted)
            {
                currentBattlerRenderer.sprite = null;
            }
            
            if (battle.opponentParty[battle.opponentBattlerIndex].isFainted)
            {
                opponentBattlerRenderer.sprite = null;
            }
        }

        public void UpdatePlayerBattlerDetails()
        {
            UpdateBattlerButtons();
            UpdateBattlerSprites();
            UpdateBattlerMoveDisplays();
            UpdateBattlerTexts();
        }

        public void UpdateOpponentBattlerDetails()
        {
            UpdateBattlerSprites();
            UpdateBattlerTexts();
        }

        public void UpdateBattlerMoveDisplays()
        {
            foreach (var text in moveTexts)
            {
                text.transform.parent.gameObject.SetActive(false);
            }

            for (var i = 0; i < battle.playerParty[battle.currentBattlerIndex].moves.Count; i++)
            {
                if (battle.playerParty[battle.currentBattlerIndex].moves[i])
                {
                    int currentPP = battle.playerParty[battle.currentBattlerIndex].movePpInfos[i].CurrentPP;
                    int maxPP = battle.playerParty[battle.currentBattlerIndex].movePpInfos[i].MaxPP;
                    
                    moveTexts[i].transform.parent.gameObject.SetActive(true);
                    moveTexts[i].text = $"{battle.playerParty[battle.currentBattlerIndex].moves[i].name} {currentPP}/{maxPP}";
                    if (currentPP <= 0)
                    {
                        moveTexts[i].transform.parent.GetComponent<Button>().interactable = false;
                    }
                }
            }
        }

        private void UpdateHealthDisplays()
        {
            float t = healthUpdateSpeed;
            
            opponentHealthDisplay.maxValue = battle.opponentParty[battle.opponentBattlerIndex].maxHealth;
            opponentHealthDisplay.value = Mathf.Lerp(opponentHealthDisplay.value, battle.opponentParty[battle.opponentBattlerIndex].currentHealth, t);

            currentBattlerHealthDisplay.maxValue = battle.playerParty[battle.currentBattlerIndex].maxHealth;
            currentBattlerHealthDisplay.value = Mathf.Lerp(currentBattlerHealthDisplay.value, battle.playerParty[battle.currentBattlerIndex].currentHealth, t);
        }
    }
}