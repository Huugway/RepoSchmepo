using UnityEngine;
using UnityEngine.InputSystem;

// Kombo-Angriff des Spielers.
// Jeder Klick führt den nächsten Schlag aus der Liste "combo" aus.
// Wer innerhalb von comboWindow nicht weiterschlägt, fängt wieder beim ersten an.
// Nach dem letzten Schlag geht es ebenfalls wieder von vorne los.
[RequireComponent(typeof(Health))]
public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private ComboHit[] combo;

    // Mindestzeit zwischen zwei Schlägen (gegen Dauerklicken).
    [SerializeField] private float timeBetweenHits = 0.25f;

    // So lange nach einem Schlag hat man Zeit für den nächsten in der Kette.
    [SerializeField] private float comboWindow = 0.8f;

    [SerializeField] private AttackFlash flashPrefab;

    private InputAction attackAction;
    private Health ownHealth;
    private int nextHit;
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
            nextHit = 0;

        if (!attackAction.WasPressedThisFrame())
            return;

        if (Time.time - lastHitTime < timeBetweenHits)
            return;

        Vector2 aim = GetAimDirection();

        // Alle Zonen dieses Schlags gleichzeitig auslösen
        foreach (AttackStep zone in combo[nextHit].zones)
        {
            MeleeHit.Perform(zone, transform.position, aim, ownHealth.Team);
            Instantiate(flashPrefab, transform).Show(zone, aim);
        }

        lastHitTime = Time.time;
        nextHit = (nextHit + 1) % combo.Length;
    }

    // Richtung vom Spieler zum Mauszeiger.
    private Vector2 GetAimDirection()
    {
        Vector2 mouseWorld = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Vector2 direction = mouseWorld - (Vector2)transform.position;

        return direction == Vector2.zero ? Vector2.right : direction.normalized;
    }
}
