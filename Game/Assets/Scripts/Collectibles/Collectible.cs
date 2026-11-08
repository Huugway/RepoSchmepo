using UnityEngine;

// Ein Gegenstand, den der Spieler einsammeln kann.
// Braucht einen Collider2D mit "Is Trigger" an.
// Eingesammelt wird nur von Objekten mit einem PlayerCollector.
public class Collectible : MonoBehaviour
{
    [SerializeField] private int value = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerCollector collector = other.GetComponent<PlayerCollector>();

        if (collector == null)
            return;

        collector.Collect(value);
        Destroy(gameObject);
    }
}
