using UnityEngine;

// Eine Kugel, die geradeaus fliegt.
// Trifft sie die andere Seite, macht sie Schaden und verschwindet.
// Trifft sie eine Wand (fester Collider ohne Health), verschwindet sie auch.
// Braucht einen Rigidbody2D (Gravity 0) und einen Collider2D mit "Is Trigger".
[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed = 4f;
    [SerializeField] private int damage = 1;

    // Sicherheitsnetz: Falls sie nie etwas trifft, nach dieser Zeit löschen.
    [SerializeField] private float maxLifetime = 10f;

    private Team ownerTeam;

    public void Launch(Vector2 direction, Team team)
    {
        ownerTeam = team;
        GetComponent<Rigidbody2D>().linearVelocity = direction * speed;
        Destroy(gameObject, maxLifetime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Health health = other.GetComponent<Health>();

        if (health != null)
        {
            // Eigene Seite und Unverwundbare (z.B. im Dash) werden durchflogen
            if (health.Team == ownerTeam || health.IsInvincible)
                return;

            health.TakeDamage(damage);
            Destroy(gameObject);
            return;
        }

        // Fester Collider ohne Health = Wand
        if (!other.isTrigger)
            Destroy(gameObject);
    }
}
