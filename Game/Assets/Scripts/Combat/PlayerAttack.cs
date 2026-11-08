using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// Kombo-Angriff des Spielers.
// Jeder Klick führt den nächsten Schlag aus der Liste "combo" aus.
// Wer innerhalb von comboWindow nicht weiterschlägt, fängt wieder beim ersten an.
// Nach dem letzten Schlag geht es ebenfalls wieder von vorne los.
public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private AttackStep[] combo;

    // Mindestzeit zwischen zwei Schlägen (gegen Dauerklicken).
    [SerializeField] private float timeBetweenHits = 0.25f;

    // So lange nach einem Schlag hat man Zeit für den nächsten in der Kette.
    [SerializeField] private float comboWindow = 0.8f;

    [SerializeField] private AttackFlash flash;

    private InputAction attackAction;
    private Health ownHealth;
    private int nextStep;
    private float lastHitTime = float.NegativeInfinity;

    private void Awake()
    {
        attackAction = InputSystem.actions.FindAction("Attack");
        ownHealth = GetComponent<Health>();
    }

    private void Update()
    {
        // Zu lange gewartet -> Kette verfällt
        if (Time.time - lastHitTime > comboWindow)
            nextStep = 0;

        if (!attackAction.WasPressedThisFrame())
            return;

        if (Time.time - lastHitTime < timeBetweenHits)
            return;

        Attack(combo[nextStep]);

        lastHitTime = Time.time;
        nextStep = (nextStep + 1) % combo.Length;
    }

    private void Attack(AttackStep step)
    {
        Vector2 origin = transform.position;
        Vector2 aim = GetAimDirection();

        foreach (Collider2D hit in FindTargets(step, origin, aim))
        {
            Health health = hit.GetComponent<Health>();

            // Nur Dinge mit Health treffen, und nicht uns selbst
            if (health != null && health != ownHealth)
                health.TakeDamage(step.damage);
        }

        flash.Show(step, aim);
    }

    // Sucht alle Collider in der Form des Schlags.
    private List<Collider2D> FindTargets(AttackStep step, Vector2 origin, Vector2 aim)
    {
        switch (step.shape)
        {
            case AttackShape.Circle:
                return new List<Collider2D>(Physics2D.OverlapCircleAll(origin, step.range));

            case AttackShape.Cone:
                // Erst alles im Kreis holen, dann nur behalten, was im Winkel liegt
                List<Collider2D> inCone = new List<Collider2D>();
                foreach (Collider2D hit in Physics2D.OverlapCircleAll(origin, step.range))
                {
                    Vector2 toHit = (Vector2)hit.transform.position - origin;
                    if (Vector2.Angle(aim, toHit) <= step.coneAngle / 2f)
                        inCone.Add(hit);
                }
                return inCone;

            case AttackShape.Line:
                // Eine gedrehte Box, die vom Spieler aus Richtung Cursor zeigt
                Vector2 center = origin + aim * (step.range / 2f);
                Vector2 size = new Vector2(step.range, step.lineWidth);
                return new List<Collider2D>(Physics2D.OverlapBoxAll(center, size, AngleOf(aim)));

            default:
                return new List<Collider2D>();
        }
    }

    // Richtung vom Spieler zum Mauszeiger.
    private Vector2 GetAimDirection()
    {
        Vector2 mouseWorld = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Vector2 direction = mouseWorld - (Vector2)transform.position;

        return direction == Vector2.zero ? Vector2.right : direction.normalized;
    }

    public static float AngleOf(Vector2 direction)
    {
        return Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    }
}
