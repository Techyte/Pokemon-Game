using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BattleUIManager : MonoBehaviour
{
    public GameObject moveButtons;
    public GameObject healthDisplays;
    public GameObject changeBattlerDisplay;
    public GameObject miscButtons;
    public TextMeshProUGUI[] battlerDisplays;

    public Battle battle;

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
        battle.UpdateBattlerSprites();
        battle.UpdateBattlerMoveDisplays();
    }

    public void Back()
    {
        healthDisplays.SetActive(true);
        moveButtons.SetActive(true);
        miscButtons.SetActive(true);
        changeBattlerDisplay.SetActive(false);
        UpdateBattlerButtons();
    }
}
