using UnityEngine;

// Fernkampf-Angriff eines Gegners: schießt in Abständen eine Kugel Richtung Ziel,
// solange das Ziel in Reichweite ist.
// Tipp: Am Enemy eine große stopDistance einstellen, damit er auf Abstand bleibt.
[RequireComponent(typeof(Enemy), typeof(Health))]
public class EnemyShooter : MonoBehaviour
{
    [SerializeField] private Projectile projectilePrefab;
    [SerializeField] private float range = 9f;

    // Pause zwischen zwei Schüssen, jedes Mal zufällig zwischen min und max.
    [SerializeField] private float minCooldown = 2.5f;
    [SerializeField] private float maxCooldown = 4f;

    private Enemy enemy;
    private Health health;
    private float nextShotTime;

    private void Awake()
    {
        enemy = GetComponent<Enemy>();
        health = GetComponent<Health>();
    }

    // Nicht sofort nach dem Spawnen schießen
    private void Start()
    {
        nextShotTime = Time.time + Random.Range(minCooldown, maxCooldown);
    }

    private void Update()
    {
        if (enemy.Target == null || enemy.DistanceToTarget() > range)
            return;

        if (Time.time >= nextShotTime)
        {
            Shoot();
            nextShotTime = Time.time + Random.Range(minCooldown, maxCooldown);
        }
    }

    private void Shoot()
    {
        Vector2 direction = enemy.DirectionToTarget();
        Projectile projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        projectile.Launch(direction, health.Team);
    }
}
