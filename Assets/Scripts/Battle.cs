using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Battle : MonoBehaviour
{
    [Header("Assignments")]
    public BattlerTemplate currentBattler;
    public SpriteRenderer currentBattlerRenderer;
    public Slider currentBattlerHealthSlider;

    public BattlerTemplate aponentBattler;
    public SpriteRenderer aponentBattlerRenderer;

    public TextMeshProUGUI[] moveTexts;
    [Space]
    public float currentBattlercurrentHealth;

    private void Start()
    {
        currentBattlerRenderer.sprite = currentBattler.texture;
        aponentBattlerRenderer.sprite = aponentBattler.texture;

        for (int i = 0; i < currentBattler.moves.Count; i++)
        {
            moveTexts[i].transform.parent.gameObject.SetActive(true);
            moveTexts[i].text = currentBattler.moves[0].name;
        }

        currentBattlerHealthSlider.maxValue = currentBattler.maxHealth;
        currentBattlercurrentHealth = currentBattler.maxHealth;
    }

    private void Update()
    {
        currentBattlerHealthSlider.value = currentBattlercurrentHealth;
    }

    public void DoMove(int moveID)
    {
        if (currentBattler.moves[moveID])
            DoMoveOnAposingBattler(currentBattler.moves[moveID]);
    }

    void DoMoveOnAposingBattler(Move move)
    {
        //You can add any animation calls for attacking here
        Debug.Log("We are attacking to aposing Battler with: " + move);
        currentBattlercurrentHealth -= move.damage;
    }
}
