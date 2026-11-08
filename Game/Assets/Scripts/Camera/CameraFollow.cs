using UnityEngine;

// Lässt die Kamera einem Ziel (z.B. dem Spieler) weich folgen.
// Kommt auf die Main Camera, das Ziel wird im Inspector gesetzt.
public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float followSpeed = 5f;

    // LateUpdate läuft nach allen Bewegungen im Frame -> kein Ruckeln.
    private void LateUpdate()
    {
        if (target == null)
            return;

        // Nur X/Y übernehmen, Z der Kamera bleibt (sonst sieht man nichts).
        Vector3 targetPosition = new Vector3(target.position.x, target.position.y, transform.position.z);

        transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
    }
}
