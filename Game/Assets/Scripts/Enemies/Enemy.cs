using UnityEngine;

// Ein Gegner, der auf sein Ziel zuläuft und zuschlägt, sobald er nah genug ist.
// Das Ziel setzt der EnemySpawner über SetTarget().
[RequireComponent(typeof(Rigidbody2D), typeof(Health))]
public class Enemy : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2.5f;
    [SerializeField] private float attackRange = 1.2f;
    [SerializeField] private int damage = 1;
    [SerializeField] private float attackCooldown = 1f;

    private Rigidbody2D body;
    private Transform target;
    private Health targetHealth;
    private float nextAttackTime;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        GetComponent<Health>().Died += OnDied;
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
        targetHealth = newTarget.GetComponent<Health>();
    }

    // Angreifen, wenn das Ziel in Reichweite ist und der Cooldown vorbei ist.
    private void Update()
    {
        if (target == null || !IsTargetInRange())
            return;

        if (Time.time >= nextAttackTime)
        {
            targetHealth.TakeDamage(damage);
            nextAttackTime = Time.time + attackCooldown;
        }
    }

    // Auf das Ziel zulaufen, in Reichweite stehen bleiben.
    private void FixedUpdate()
    {
        if (target == null || IsTargetInRange())
        {
            body.linearVelocity = Vector2.zero;
            return;
        }

        Vector2 direction = ((Vector2)target.position - body.position).normalized;
        body.linearVelocity = direction * moveSpeed;
    }

    private bool IsTargetInRange()
    {
        return Vector2.Distance(transform.position, target.position) <= attackRange;
    }

    private void OnDied()
    {
        Destroy(gameObject);
    }
}
