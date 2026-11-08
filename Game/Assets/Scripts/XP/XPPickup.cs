using UnityEngine;

// Ein XP-Orb, den der Spieler einsammeln kann (wird z.B. von Gegnern gedroppt).
// Braucht einen Collider2D mit "Is Trigger" an.
// Eingesammelt wird nur von Objekten mit PlayerXP.
public class XPPickup : MonoBehaviour
{
    [SerializeField] private int amount = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerXP playerXP = other.GetComponent<PlayerXP>();

        if (playerXP == null)
            return;

        playerXP.AddXP(amount);
        Destroy(gameObject);
    }
}
