using UnityEngine;
using UnityEngine.UI;

// Zeigt den Heil-Fortschritt beim Kanalisieren, oder ob Heilen gerade möglich ist.
// Kommt auf ein UI-Text-Objekt, der PlayerHeal wird im Inspector gesetzt.
[RequireComponent(typeof(Text))]
public class HealUI : MonoBehaviour
{
    [SerializeField] private PlayerHeal heal;
    [SerializeField] private Color readyColor = Color.green;
    [SerializeField] private Color notReadyColor = Color.gray;

    private Text label;

    private void Awake()
    {
        label = GetComponent<Text>();
    }

    private void Update()
    {
        if (heal.IsChanneling)
        {
            label.text = $"Healing {heal.Progress * 100f:0}%";
            label.color = readyColor;
        }
        else if (heal.CanHeal)
        {
            label.text = $"Heal ready ({heal.XPCost} XP)";
            label.color = readyColor;
        }
        else
        {
            label.text = $"Heal {heal.XPCost} XP";
            label.color = notReadyColor;
        }
    }
}
