using System;
using UnityEngine;

// Zu welcher Seite etwas gehört. Angriffe treffen nur die andere Seite.
public enum Team
{
    Player,
    Enemy
}

// Lebenspunkte für alles, was Schaden nehmen kann (Spieler, Gegner, ...).
// Was beim Tod passiert, entscheiden andere Scripts über das Died-Event.
public class Health : MonoBehaviour
{
    [SerializeField] private int maxHealth = 5;
    [SerializeField] private Team team = Team.Player;

    public Team Team => team;
    public int Current { get; private set; }
    public int Max => maxHealth;
    public bool IsDead => Current <= 0;

    // Unverwundbar: kein Schaden und kein Rückstoß (z.B. während eines Dashs).
    public bool IsInvincible => Time.time < invincibleUntil;
    private float invincibleUntil;

    // (aktuelle HP, maximale HP) - bei Schaden und bei Heilung
    public event Action<int, int> Changed;

    // (Höhe des Schadens) - nur bei Schaden, z.B. für Treffer-Effekte
    public event Action<int> Damaged;

    public event Action Died;

    private void Awake()
    {
        Current = maxHealth;
    }

    public void MakeInvincible(float seconds)
    {
        invincibleUntil = Time.time + seconds;
    }

    public void TakeDamage(int amount)
    {
        if (IsDead || IsInvincible)
            return;

        Current = Mathf.Max(Current - amount, 0);
        Changed?.Invoke(Current, maxHealth);
        Damaged?.Invoke(amount);

        if (IsDead)
            Died?.Invoke();
    }

    public void Heal(int amount)
    {
        if (IsDead)
            return;

        Current = Mathf.Min(Current + amount, maxHealth);
        Changed?.Invoke(Current, maxHealth);
    }

    // Zum Testen: Rechtsklick auf die Komponente im Inspector -> "Kill".
    [ContextMenu("Kill")]
    private void Kill()
    {
        TakeDamage(Current);
    }
}
