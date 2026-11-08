using UnityEngine;
using UnityEngine.InputSystem;

// Dash des Spielers: ein schneller Sprung in Laufrichtung
// (oder Richtung Maus, wenn man gerade steht).
// Ab dem Dash ist man kurz unverwundbar und wird dabei grün angezeigt.
[RequireComponent(typeof(Rigidbody2D), typeof(Health), typeof(SpriteRenderer))]
public class PlayerDash : MonoBehaviour
{
    [SerializeField, Range(1f, 10f)] private float distance = 5f;
    [SerializeField, Range(0.05f, 0.5f)] private float duration = 0.15f;
    [SerializeField, Range(0f, 1f)] private float invincibleTime = 0.2f;
    [SerializeField, Range(0f, 10f)] private float cooldown = 5f;
    [SerializeField] private Color invincibleColor = Color.green;

    private Rigidbody2D body;
    private Health health;
    private SpriteRenderer spriteRenderer;
    private Color normalColor;
    private InputAction dashAction;
    private InputAction moveAction;

    private Vector2 dashVelocity;
    private float dashEndTime;
    private float nextDashTime;
    private bool wasDashing;
    private bool showingInvincible;

    public bool IsDashing => Time.time < dashEndTime;

    // Wie lange es noch dauert, bis man wieder dashen kann (0 = bereit).
    public float CooldownRemaining => Mathf.Max(0f, nextDashTime - Time.time);
    public bool IsReady => CooldownRemaining <= 0f;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        health = GetComponent<Health>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        normalColor = spriteRenderer.color;
        dashAction = InputSystem.actions.FindAction("Dash");
        moveAction = InputSystem.actions.FindAction("Move");
    }

    private void Update()
    {
        if (dashAction.WasPressedThisFrame() && Time.time >= nextDashTime)
            StartDash();

        // Grün, solange man unverwundbar ist. Die Farbe nur beim Wechsel setzen,
        // damit andere Effekte (z.B. DamageFlash) nicht überschrieben werden.
        if (health.IsInvincible != showingInvincible)
        {
            showingInvincible = health.IsInvincible;
            spriteRenderer.color = showingInvincible ? invincibleColor : normalColor;
        }
    }

    private void StartDash()
    {
        // Geschwindigkeit so wählen, dass in "duration" genau "distance" geschafft wird
        dashVelocity = GetDashDirection() * (distance / duration);
        dashEndTime = Time.time + duration;
        nextDashTime = Time.time + cooldown;

        health.MakeInvincible(invincibleTime);
    }

    private void FixedUpdate()
    {
        if (IsDashing)
        {
            body.linearVelocity = dashVelocity;
            wasDashing = true;
        }
        else if (wasDashing)
        {
            // Dash vorbei: anhalten, damit man nicht weiterrutscht
            body.linearVelocity = Vector2.zero;
            wasDashing = false;
        }
    }

    // In Laufrichtung, oder Richtung Maus, wenn man gerade nicht läuft.
    private Vector2 GetDashDirection()
    {
        Vector2 moveInput = moveAction.ReadValue<Vector2>();
        if (moveInput != Vector2.zero)
            return moveInput.normalized;

        Vector2 mouseWorld = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Vector2 toMouse = mouseWorld - body.position;

        return toMouse == Vector2.zero ? Vector2.right : toMouse.normalized;
    }
}
