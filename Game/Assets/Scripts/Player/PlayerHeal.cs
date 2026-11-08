using UnityEngine;
using UnityEngine.InputSystem;

// Heilen durch Kanalisieren: Heal-Taste (Rechtsklick) gedrückt halten.
// Nach channelTime Sekunden werden XP ausgegeben und HP geheilt.
// Loslassen oder ein Treffer bricht ab, der Fortschritt beginnt dann von vorne.
// Wer weiter gedrückt hält, kanalisiert direkt die nächste Heilung.
[RequireComponent(typeof(Health), typeof(PlayerXP))]
public class PlayerHeal : MonoBehaviour
{
    [SerializeField, Range(0.5f, 5f)] private float channelTime = 2f;
    [SerializeField, Range(0, 100)] private int xpCost = 20;
    [SerializeField] private int healAmount = 1;

    private Health health;
    private PlayerXP playerXP;
    private InputAction healAction;
    private float channelTimer;

    public bool IsChanneling { get; private set; }
    public float Progress => channelTimer / channelTime;
    public int XPCost => xpCost;

    // Heilen geht nur mit genug XP und wenn nicht schon volle HP
    public bool CanHeal => playerXP.XP >= xpCost && health.Current < health.Max;

    private void Awake()
    {
        health = GetComponent<Health>();
        playerXP = GetComponent<PlayerXP>();
        healAction = InputSystem.actions.FindAction("Heal");
        health.Damaged += OnDamaged;
    }

    private void Update()
    {
        if (!healAction.IsPressed() || !CanHeal)
        {
            StopChanneling();
            return;
        }

        IsChanneling = true;
        channelTimer += Time.deltaTime;

        if (channelTimer >= channelTime)
        {
            playerXP.TrySpend(xpCost);
            health.Heal(healAmount);
            channelTimer = 0f;
        }
    }

    // Ein Treffer unterbricht das Kanalisieren
    private void OnDamaged(int amount)
    {
        StopChanneling();
    }

    private void StopChanneling()
    {
        IsChanneling = false;
        channelTimer = 0f;
    }
}
