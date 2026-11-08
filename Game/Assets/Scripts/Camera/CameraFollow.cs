using UnityEngine;

// Lässt die Kamera einem Ziel (z.B. dem Spieler) weich folgen.
// Kommt auf die Main Camera, das Ziel wird im Inspector gesetzt.
public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;

    // Ungefähre Zeit in Sekunden, bis die Kamera das Ziel eingeholt hat.
    // Klein = klebt am Spieler, groß = zieht weich hinterher.
    [SerializeField] private float smoothTime = 0.15f;

    // Aktuelle Geschwindigkeit der Kamera, wird von SmoothDamp verwaltet.
    private Vector3 velocity;

    // LateUpdate läuft nach allen Bewegungen im Frame -> kein Ruckeln.
    private void LateUpdate()
    {
        if (target == null)
            return;

        // Nur X/Y übernehmen, Z der Kamera bleibt (sonst sieht man nichts).
        Vector3 targetPosition = new Vector3(target.position.x, target.position.y, transform.position.z);

        // SmoothDamp beschleunigt und bremst die Kamera weich ab,
        // auch wenn der Spieler plötzlich die Richtung wechselt.
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }
}
