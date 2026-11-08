using System;
using UnityEngine;

// Kommt auf den Spieler und zählt seine XP.
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
}
