using PokemonGame.ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuBattlerDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI battlerNameText;
    [SerializeField] private TextMeshProUGUI battlerHealthText;
    [SerializeField] private TextMeshProUGUI statusDisplay;
    [SerializeField] private Image battlerSpriteImage;

    public void Init(string name, int health, int maxHealth, StatusEffect effect, Sprite sprite)
    {
        battlerNameText.text = name;
        battlerHealthText.text = $"{health}/{maxHealth}";
        battlerSpriteImage.sprite = sprite;
        statusDisplay.text = effect.name;
        statusDisplay.color = effect.colour;
    }
}
