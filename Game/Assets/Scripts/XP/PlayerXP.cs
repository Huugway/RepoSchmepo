using System;
using UnityEngine;

// Kommt auf den Spieler und zählt seine XP.
// XP können auch ausgegeben werden (z.B. für Dash oder Heilen).
public class PlayerXP : MonoBehaviour
{
    public int XP { get; private set; }

    // Wird jedes Mal ausgelöst, wenn sich XP ändert (z.B. für die UI).
    public event Action<int> XPChanged;

    public void AddXP(int amount)
    {
        XP += amount;
        XPChanged?.Invoke(XP);
    }

    // Gibt XP aus, wenn genug da sind. Gibt zurück, ob es geklappt hat.
    public bool TrySpend(int amount)
    {
        if (XP < amount)
            return false;

        XP -= amount;
        XPChanged?.Invoke(XP);
        return true;
    }
}
