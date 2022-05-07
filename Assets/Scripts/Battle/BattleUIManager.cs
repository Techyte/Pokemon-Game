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
            UpdateBattlerButtons();
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

                if (i == battle.currentBattlerIndex)
                {
                    Debug.Log("Dissabling " + battle.playerParty.party[i].name + " option");
                    battlerDisplays[i].transform.parent.GetComponent<Button>().interactable = false;
                }
            }

            /*
            if (i == battle.currentBattlerIndex)
            {
                Debug.Log("Dissabling " + battle.playerParty.party[i].name + " option");
                battlerDisplays[i].transform.parent.GetComponent<Button>().interactable = false;
            }
            */
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
            battle.currentBattlerIndex = partyID;
            battle.playerParty.party[battle.currentBattlerIndex] = battle.playerParty.party[partyID];
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
            currentBattlerNameDisplay.text = battle.playerParty.party[battle.currentBattlerIndex].name;
            apponentBattlerNameDisplay.text = battle.apponentParty.party[battle.apponentBattlerIndex].name;
        }

        public void UpdateBattlerSprites()
        {
            currentBattlerRenderer.sprite = battle.playerParty.party[battle.currentBattlerIndex].texture;
            apponentBattlerRenderer.sprite = battle.apponentParty.party[battle.apponentBattlerIndex].texture;
        }

        public void UpdateBattlerMoveDisplays()
        {
            for (int i = 0; i < moveTexts.Length; i++)
            {
                moveTexts[i].transform.parent.gameObject.SetActive(false);
            }

            for (int i = 0; i < battle.playerParty.party[battle.currentBattlerIndex].moves.Length; i++)
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
            apponentHealthDisplay.maxValue = battle.apponentParty.party[battle.apponentBattlerIndex].maxHealth;
            apponentHealthDisplay.value = battle.apponentParty.party[battle.apponentBattlerIndex].currentHealth;

            currentBattlerHealthDisplay.maxValue = battle.playerParty.party[battle.currentBattlerIndex].maxHealth;
            currentBattlerHealthDisplay.value = battle.playerParty.party[battle.currentBattlerIndex].currentHealth;
        }
    }

}