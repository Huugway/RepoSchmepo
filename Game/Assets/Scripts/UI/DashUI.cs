using UnityEngine;
using UnityEngine.UI;

// Zeigt an, ob der Dash bereit ist, wie lange der Cooldown noch läuft,
// oder dass zu wenig XP da sind.
// Kommt auf ein UI-Text-Objekt, der PlayerDash wird im Inspector gesetzt.
[RequireComponent(typeof(Text))]
public class DashUI : MonoBehaviour
{
    [SerializeField] private PlayerDash dash;
    [SerializeField] private Color readyColor = Color.green;
    [SerializeField] private Color notReadyColor = Color.gray;

    private Text label;

    private void Awake()
    {
        label = GetComponent<Text>();
    }

    // Der Cooldown läuft jeden Frame weiter, darum hier jeden Frame aktualisieren.
    private void Update()
    {
        if (!dash.IsReady)
        {
            label.text = $"Dash {dash.CooldownRemaining:0.0}s";
            label.color = notReadyColor;
        }
        else if (!dash.HasEnoughXP)
        {
            label.text = $"Dash {dash.XPCost} XP";
            label.color = notReadyColor;
        }
        else
        {
            label.text = $"Dash ready ({dash.XPCost} XP)";
            label.color = readyColor;
        }
    }
}
