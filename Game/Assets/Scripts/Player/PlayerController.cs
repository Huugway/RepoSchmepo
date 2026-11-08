using UnityEngine;
using UnityEngine.InputSystem;

// Bewegt den Spieler in 8 Richtungen (Top-Down).
// Input kommt aus der "Move"-Action der projektweiten Input Actions
// (Assets/Settings/InputSystem_Actions): WASD, Pfeiltasten, Gamepad-Stick.
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;

    private Rigidbody2D body;
    private InputAction moveAction;
    private Vector2 moveInput;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
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
        body.linearVelocity = moveInput * moveSpeed;
    }
}
