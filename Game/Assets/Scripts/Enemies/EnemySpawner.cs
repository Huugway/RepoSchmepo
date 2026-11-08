using System;
using UnityEngine;

// Schickt Gegner in Wellen.
// Jede Welle hat ein paar Gegner mehr als die vorherige. Die Gegner einer Welle
// erscheinen nacheinander. Sind alle tot, startet nach einer Pause die nächste Welle.
public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Enemy enemyPrefab;
    [SerializeField] private Transform player;

    [Header("Wellen")]
    [SerializeField] private int enemiesInFirstWave = 5;
    [SerializeField] private int extraEnemiesPerWave = 3;
    [SerializeField] private float timeBetweenSpawns = 0.4f;
    [SerializeField] private float timeBetweenWaves = 3f;

    [Header("Spawn-Bereich")]
    // Bereich, in dem gespawnt wird (Mitte = Position dieses Objekts).
    [SerializeField] private Vector2 areaSize = new Vector2(33f, 33f);
    [SerializeField] private float minDistanceToPlayer = 8f;

    public int CurrentWave { get; private set; }

    // Gegner, die noch kommen, plus Gegner, die noch leben.
    public int EnemiesRemaining => enemiesLeftToSpawn + enemiesAlive;

    // Wird ausgelöst, wenn sich Welle oder Anzahl ändert (z.B. für die UI).
    public event Action Changed;

    private int enemiesLeftToSpawn;
    private int enemiesAlive;
    private float nextSpawnTime;

    private void Start()
    {
        StartNextWave();
    }

    private void Update()
    {
        if (enemiesLeftToSpawn > 0 && Time.time >= nextSpawnTime)
        {
            SpawnEnemy();
            nextSpawnTime = Time.time + timeBetweenSpawns;
        }
    }

    private void StartNextWave()
    {
        CurrentWave++;
        enemiesLeftToSpawn = enemiesInFirstWave + (CurrentWave - 1) * extraEnemiesPerWave;
        nextSpawnTime = Time.time;
        Changed?.Invoke();
    }

    private void SpawnEnemy()
    {
        Enemy enemy = Instantiate(enemyPrefab, GetSpawnPosition(), Quaternion.identity, transform);
        enemy.SetTarget(player);
        enemy.GetComponent<Health>().Died += OnEnemyDied;

        enemiesLeftToSpawn--;
        enemiesAlive++;
        Changed?.Invoke();
    }

    private void OnEnemyDied()
    {
        enemiesAlive--;
        Changed?.Invoke();

        // Welle geschafft -> nach einer Pause die nächste
        if (EnemiesRemaining == 0)
            Invoke(nameof(StartNextWave), timeBetweenWaves);
    }

    // Zufälliger Punkt im Bereich. Liegt er zu nah am Spieler, wird neu gewürfelt.
    private Vector2 GetSpawnPosition()
    {
        Vector2 position = Vector2.zero;

        for (int attempt = 0; attempt < 10; attempt++)
        {
            position = (Vector2)transform.position + new Vector2(
                UnityEngine.Random.Range(-areaSize.x / 2f, areaSize.x / 2f),
                UnityEngine.Random.Range(-areaSize.y / 2f, areaSize.y / 2f));

            if (Vector2.Distance(position, player.position) >= minDistanceToPlayer)
                break;
        }

        return position;
    }

    // Zeigt den Spawn-Bereich in der Scene-Ansicht, wenn das Objekt ausgewählt ist.
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, areaSize);
    }
}
