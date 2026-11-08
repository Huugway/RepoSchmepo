using UnityEngine;

// Erzeugt in regelmäßigen Abständen Gegner an zufälligen Stellen im Spielfeld,
// aber nicht direkt neben dem Spieler.
// Die erzeugten Gegner werden Kinder dieses Objekts, so lassen sie sich leicht zählen.
public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Enemy enemyPrefab;
    [SerializeField] private Transform player;

    [SerializeField] private float spawnInterval = 2f;
    [SerializeField] private int maxEnemies = 15;

    // Bereich, in dem gespawnt wird (Mitte = Position dieses Objekts).
    [SerializeField] private Vector2 areaSize = new Vector2(33f, 33f);
    [SerializeField] private float minDistanceToPlayer = 8f;

    private float nextSpawnTime;

    private void Update()
    {
        if (Time.time < nextSpawnTime)
            return;

        nextSpawnTime = Time.time + spawnInterval;

        if (transform.childCount < maxEnemies)
            SpawnEnemy();
    }

    private void SpawnEnemy()
    {
        Enemy enemy = Instantiate(enemyPrefab, GetSpawnPosition(), Quaternion.identity, transform);
        enemy.SetTarget(player);
    }

    // Zufälliger Punkt im Bereich. Liegt er zu nah am Spieler, wird neu gewürfelt.
    private Vector2 GetSpawnPosition()
    {
        Vector2 position = Vector2.zero;

        for (int attempt = 0; attempt < 10; attempt++)
        {
            position = (Vector2)transform.position + new Vector2(
                Random.Range(-areaSize.x / 2f, areaSize.x / 2f),
                Random.Range(-areaSize.y / 2f, areaSize.y / 2f));

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
