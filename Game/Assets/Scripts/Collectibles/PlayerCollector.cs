using System;
using UnityEngine;

// Kommt auf den Spieler und zählt, wie viel er eingesammelt hat.
public class PlayerCollector : MonoBehaviour
{
    public int Count { get; private set; }

    // Wird jedes Mal ausgelöst, wenn sich Count ändert (z.B. für die UI).
    public event Action<int> CountChanged;

    public void Collect(int amount)
    {
        Count += amount;
        CountChanged?.Invoke(Count);
    }
}
