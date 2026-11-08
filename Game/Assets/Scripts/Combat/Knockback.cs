using UnityEngine;

// Stößt das Objekt bei einem Treffer weg.
// Solange der Rückstoß läuft (IsActive), sollen Bewegungs-Scripts die
// Geschwindigkeit nicht setzen, sonst wäre der Stoß sofort wieder weg.
[RequireComponent(typeof(Rigidbody2D))]
public class Knockback : MonoBehaviour
{
    // Wie lange der Rückstoß die Bewegung übernimmt.
    [SerializeField] private float duration = 0.2f;

    // Wie viel Geschwindigkeit pro Physik-Schritt übrig bleibt (0.85 = 15% weniger).
    [SerializeField, Range(0f, 1f)] private float slowdownPerStep = 0.85f;

    // Wie stark Rückstöße wirken: 1 = voll, 0.3 = nur 30% (z.B. für schwere Gegner).
    [SerializeField, Range(0f, 1f)] private float multiplier = 1f;

    private Rigidbody2D body;
    private float endTime;

    public bool IsActive => Time.time < endTime;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
    }

    public void Apply(Vector2 velocity)
    {
        body.linearVelocity = velocity * multiplier;
        endTime = Time.time + duration;
    }

    // Den Stoß weich auslaufen lassen.
    private void FixedUpdate()
    {
        if (IsActive)
            body.linearVelocity *= slowdownPerStep;
    }
}
