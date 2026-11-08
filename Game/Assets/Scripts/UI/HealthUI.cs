using UnityEngine;
using UnityEngine.UI;

// Zeigt die Lebenspunkte an (z.B. vom Spieler).
// Kommt auf ein UI-Text-Objekt, die Health-Komponente wird im Inspector gesetzt.
[RequireComponent(typeof(Text))]
public class HealthUI : MonoBehaviour
{
    [SerializeField] private Health health;

    private Text label;

    private void Awake()
    {
        label = GetComponent<Text>();
    }

    private void OnEnable()
    {
        health.Changed += UpdateLabel;
    }

    private void OnDisable()
    {
        health.Changed -= UpdateLabel;
    }

    // Erst in Start anzeigen: Dann hat Health in Awake seine HP sicher schon gesetzt.
    private void Start()
    {
        UpdateLabel(health.Current, health.Max);
    }

    private void UpdateLabel(int current, int max)
    {
        label.text = $"HP {current}/{max}";
    }
}
