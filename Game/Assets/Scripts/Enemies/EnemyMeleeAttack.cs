using UnityEngine;

// Nahkampf-Angriff eines Gegners: ein Schlag (z.B. Kegel) Richtung Ziel,
// sobald das Ziel in Reichweite ist.
[RequireComponent(typeof(Enemy), typeof(Health))]
public class EnemyMeleeAttack : MonoBehaviour
{
    // Der Schlag (Form, Reichweite, Schaden, Rückstoß).
    [SerializeField] private AttackStep attack;
    [SerializeField] private AttackFlash flashPrefab;

    // Wartezeit, nachdem das Ziel in Reichweite kommt, bis zum ersten Schlag.
    // Wird jedes Mal zufällig zwischen min und max gewürfelt.
    [SerializeField] private float minReactionTime = 0.2f;
    [SerializeField] private float maxReactionTime = 0.7f;

    // Pause zwischen zwei Schlägen, ebenfalls jedes Mal zufällig.
    [SerializeField] private float minAttackCooldown = 0.8f;
    [SerializeField] private float maxAttackCooldown = 1.5f;

    private Enemy enemy;
    private Health health;
    private bool targetWasInRange;
    private float nextAttackTime;

    private void Awake()
    {
        enemy = GetComponent<Enemy>();
        health = GetComponent<Health>();
    }

    private void Update()
    {
        if (enemy.Target == null)
            return;

        bool targetInRange = enemy.DistanceToTarget() <= attack.range;

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
        Vector2 aim = enemy.DirectionToTarget();
        MeleeHit.Perform(attack, transform.position, aim, health.Team);
        Instantiate(flashPrefab, transform).Show(attack, aim);

        nextAttackTime = Time.time + Random.Range(minAttackCooldown, maxAttackCooldown);
    }
}
