using UnityEngine;

// Ein Gegner, der auf sein Ziel zuläuft und mit einem Kegel-Schlag angreift,
// sobald das Ziel in Reichweite ist. Das Ziel setzt der EnemySpawner über SetTarget().
[RequireComponent(typeof(Rigidbody2D), typeof(Health))]
public class Enemy : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2.5f;

    // So nah läuft der Gegner an das Ziel heran, dann bleibt er stehen.
    [SerializeField] private float stopDistance = 1.1f;

    // Der Schlag des Gegners (Form, Reichweite, Schaden, Rückstoß).
    [SerializeField] private AttackStep attack;
    [SerializeField] private AttackFlash flashPrefab;

    // Wartezeit, nachdem das Ziel in Reichweite kommt, bis zum ersten Schlag.
    // Wird jedes Mal zufällig zwischen min und max gewürfelt.
    [SerializeField] private float minReactionTime = 0.2f;
    [SerializeField] private float maxReactionTime = 0.7f;

    // Pause zwischen zwei Schlägen, ebenfalls jedes Mal zufällig.
    [SerializeField] private float minAttackCooldown = 0.8f;
    [SerializeField] private float maxAttackCooldown = 1.5f;

    private Rigidbody2D body;
    private Health health;
    private Knockback knockback;
    private Transform target;
    private bool targetWasInRange;
    private float nextAttackTime;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        health = GetComponent<Health>();
        knockback = GetComponent<Knockback>();
        health.Died += OnDied;
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    private void Update()
    {
        if (target == null)
            return;

        bool targetInRange = DistanceToTarget() <= attack.range;

        // Ziel ist gerade erst in Reichweite gekommen -> kurz "reagieren"
        if (targetInRange && !targetWasInRange)
        {
            float reactionTime = Random.Range(minReactionTime, maxReactionTime);
            nextAttackTime = Mathf.Max(nextAttackTime, Time.time + reactionTime);
        }

        targetWasInRange = targetInRange;

        if (targetInRange && Time.time >= nextAttackTime)
            Attack();
    }

    private void Attack()
    {
        Vector2 aim = DirectionToTarget();
        MeleeHit.Perform(attack, body.position, aim, health.Team);
        Instantiate(flashPrefab, transform).Show(attack, aim);

        nextAttackTime = Time.time + Random.Range(minAttackCooldown, maxAttackCooldown);
    }

    // Auf das Ziel zulaufen, ab stopDistance stehen bleiben.
    private void FixedUpdate()
    {
        // Während eines Rückstoßes steuert der Knockback die Bewegung
        if (knockback != null && knockback.IsActive)
            return;

        if (target == null || DistanceToTarget() <= stopDistance)
        {
            body.linearVelocity = Vector2.zero;
            return;
        }

        body.linearVelocity = DirectionToTarget() * moveSpeed;
    }

    private float DistanceToTarget()
    {
        return Vector2.Distance(body.position, target.position);
    }

    private Vector2 DirectionToTarget()
    {
        return ((Vector2)target.position - body.position).normalized;
    }

    private void OnDied()
    {
        Destroy(gameObject);
    }
}
