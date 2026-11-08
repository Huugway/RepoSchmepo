using UnityEngine;
using UnityEngine.UI;

// Zeigt die aktuelle Welle und wie viele Gegner noch übrig sind.
// Kommt auf ein UI-Text-Objekt, der EnemySpawner wird im Inspector gesetzt.
[RequireComponent(typeof(Text))]
public class WaveUI : MonoBehaviour
{
    [SerializeField] private EnemySpawner spawner;

    private Text label;

    private void Awake()
    {
        label = GetComponent<Text>();
    }

    private void OnEnable()
    {
        spawner.Changed += UpdateLabel;
    }

    private void OnDisable()
    {
        spawner.Changed -= UpdateLabel;
    }

    private void Start()
    {
        UpdateLabel();
    }

    private void UpdateLabel()
    {
        int remaining = spawner.EnemiesRemaining;
        string enemyWord = remaining == 1 ? "Enemy" : "Enemies";

        label.text = $"Wave {spawner.CurrentWave}\n{remaining} {enemyWord} remaining";
    }
}
