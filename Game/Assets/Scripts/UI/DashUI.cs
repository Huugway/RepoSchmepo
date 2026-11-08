using UnityEngine;
using UnityEngine.UI;

// Zeigt an, ob der Dash bereit ist, oder wie lange der Cooldown noch läuft.
// Kommt auf ein UI-Text-Objekt, der PlayerDash wird im Inspector gesetzt.
[RequireComponent(typeof(Text))]
public class DashUI : MonoBehaviour
{
    [SerializeField] private PlayerDash dash;
    [SerializeField] private Color readyColor = Color.green;
    [SerializeField] private Color cooldownColor = Color.gray;

    private Text label;

    private void Awake()
    {
        label = GetComponent<Text>();
    }

    // Der Cooldown läuft jeden Frame weiter, darum hier jeden Frame aktualisieren.
    private void Update()
    {
        if (dash.IsReady)
        {
            label.text = "Dash ready";
            label.color = readyColor;
        }
        else
        {
            label.text = $"Dash {dash.CooldownRemaining:0.0}s";
            label.color = cooldownColor;
        }
    }
}
