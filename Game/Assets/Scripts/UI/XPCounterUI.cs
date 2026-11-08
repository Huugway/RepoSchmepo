using UnityEngine;
using UnityEngine.UI;

// Zeigt die XP des Spielers an.
// Kommt auf ein UI-Text-Objekt, der Spieler wird im Inspector gesetzt.
[RequireComponent(typeof(Text))]
public class XPCounterUI : MonoBehaviour
{
    [SerializeField] private PlayerXP playerXP;

    private Text label;

    private void Awake()
    {
        label = GetComponent<Text>();
    }

    // Beim Event anmelden ...
    private void OnEnable()
    {
        playerXP.XPChanged += UpdateLabel;
        UpdateLabel(playerXP.XP);
    }

    // ... und wieder abmelden, wenn das Objekt deaktiviert wird.
    private void OnDisable()
    {
        playerXP.XPChanged -= UpdateLabel;
    }

    private void UpdateLabel(int xp)
    {
        label.text = $"XP {xp}";
    }
}
