using UnityEngine;

// Grundlage für jeden Gegner: kennt sein Ziel, läuft darauf zu und stirbt.
// Wie der Gegner angreift, entscheiden eigene Komponenten
// (z.B. EnemyMeleeAttack oder EnemyShooter). Das Ziel setzt der EnemySpawner.
[RequireComponent(typeof(Rigidbody2D), typeof(Health))]
public class Enemy : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2.5f;

    // So nah läuft der Gegner an das Ziel heran, dann bleibt er stehen.
    [SerializeField] private float stopDistance = 1.1f;

    private Rigidbody2D body;
    private Knockback knockback;

    public Transform Target { get; private set; }

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        knockback = GetComponent<Knockback>();
        GetComponent<Health>().Died += OnDied;
    }

    public void SetTarget(Transform newTarget)
    {
        Target = newTarget;
    }

    // Auf das Ziel zulaufen, ab stopDistance stehen bleiben.
    private void FixedUpdate()
    {
        // Während eines Rückstoßes steuert der Knockback die Bewegung
        if (knockback != null && knockback.IsActive)
            return;

        if (Target == null || DistanceToTarget() <= stopDistance)
        {
            body.linearVelocity = Vector2.zero;
            return;
        }

        body.linearVelocity = DirectionToTarget() * moveSpeed;
    }

    public float DistanceToTarget()
    {
        return Vector2.Distance(body.position, Target.position);
    }

    public Vector2 DirectionToTarget()
    {
        return ((Vector2)Target.position - body.position).normalized;
    }

    private void OnDied()
    {
        Destroy(gameObject);
    }
}
