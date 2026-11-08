using UnityEngine;
using UnityEngine.UI;

// Zeigt an, wie viel der Spieler eingesammelt hat.
// Kommt auf ein UI-Text-Objekt, der Spieler wird im Inspector gesetzt.
[RequireComponent(typeof(Text))]
public class CollectibleCounterUI : MonoBehaviour
{
    [SerializeField] private PlayerCollector collector;

    private Text label;

    private void Awake()
    {
        label = GetComponent<Text>();
    }

    // Beim Event anmelden ...
    private void OnEnable()
    {
        collector.CountChanged += UpdateLabel;
        UpdateLabel(collector.Count);
    }

    // ... und wieder abmelden, wenn das Objekt deaktiviert wird.
    private void OnDisable()
    {
        collector.CountChanged -= UpdateLabel;
    }

    private void UpdateLabel(int count)
    {
        label.text = $"Münzen: {count}";
    }
}
