using System;
using UnityEngine;

// Lebenspunkte für alles, was Schaden nehmen kann (Spieler, Gegner, ...).
// Was beim Tod passiert, entscheiden andere Scripts über das Died-Event.
public class Health : MonoBehaviour
{
    [SerializeField] private int maxHealth = 5;

    public int Current { get; private set; }
    public int Max => maxHealth;
    public bool IsDead => Current <= 0;

    // (aktuelle HP, maximale HP)
    public event Action<int, int> Changed;
    public event Action Died;

    private void Awake()
    {
        Current = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        if (IsDead)
            return;

        Current = Mathf.Max(Current - amount, 0);
        Changed?.Invoke(Current, maxHealth);

        if (IsDead)
            Died?.Invoke();
    }

    // Zum Testen: Rechtsklick auf die Komponente im Inspector -> "Kill".
    [ContextMenu("Kill")]
    private void Kill()
    {
        TakeDamage(Current);
    }
}
