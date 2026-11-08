using UnityEngine;
using UnityEngine.InputSystem;

// Bewegt den Spieler in 8 Richtungen (Top-Down).
// Input kommt aus der "Move"-Action der projektweiten Input Actions
// (Assets/Settings/InputSystem_Actions): WASD, Pfeiltasten, Gamepad-Stick.
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;

    // Wie schnell der Spieler auf moveSpeed kommt (Einheiten pro Sekunde²).
    // Hoch = direkt, niedrig = rutschig.
    [SerializeField] private float acceleration = 40f;

    private Rigidbody2D body;
    private Knockback knockback;
    private PlayerDash dash;
    private InputAction moveAction;
    private Vector2 moveInput;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        knockback = GetComponent<Knockback>();
        dash = GetComponent<PlayerDash>();
        moveAction = InputSystem.actions.FindAction("Move");
    }

    // Input jeden Frame lesen ...
    private void Update()
    {
        moveInput = moveAction.ReadValue<Vector2>();

        // Diagonal nicht schneller als geradeaus
        moveInput = Vector2.ClampMagnitude(moveInput, 1f);
    }

    // ... und in der Physik-Schleife anwenden.
    private void FixedUpdate()
    {
        // Während eines Rückstoßes steuert der Knockback die Bewegung
        if (knockback != null && knockback.IsActive)
            return;

        // Während eines Dashs steuert der Dash die Bewegung
        if (dash != null && dash.IsDashing)
            return;

        Vector2 targetVelocity = moveInput * moveSpeed;

        // Nicht sofort auf die Zielgeschwindigkeit springen, sondern hinbeschleunigen.
        body.linearVelocity = Vector2.MoveTowards(body.linearVelocity, targetVelocity, acceleration * Time.fixedDeltaTime);
    }
}
