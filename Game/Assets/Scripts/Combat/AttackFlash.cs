using UnityEngine;

// Zeigt einen Schlag kurz als halbtransparente Form an (Platzhalter-Optik).
// Kommt auf ein Kind-Objekt des Spielers mit einem SpriteRenderer.
[RequireComponent(typeof(SpriteRenderer))]
public class AttackFlash : MonoBehaviour
{
    [SerializeField] private float duration = 0.1f;

    private SpriteRenderer spriteRenderer;
    private float hideTime;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = false;
    }

    private void Update()
    {
        if (spriteRenderer.enabled && Time.time >= hideTime)
            spriteRenderer.enabled = false;
    }

    // Größe und Drehung passend zur Form setzen.
    // Die Sprites sind so gebaut: Kreis = 1 Einheit Durchmesser,
    // Kegel = 1 Einheit lang mit Spitze in der Mitte, Quadrat = 1x1.
    public void Show(AttackStep step, Vector2 aim)
    {
        float angle = PlayerAttack.AngleOf(aim);

        switch (step.shape)
        {
            case AttackShape.Circle:
                Place(Vector2.zero, 0f, new Vector2(step.range * 2f, step.range * 2f));
                break;

            case AttackShape.Cone:
                Place(Vector2.zero, angle, new Vector2(step.range, step.range));
                break;

            case AttackShape.Line:
                Place(aim * (step.range / 2f), angle, new Vector2(step.range, step.lineWidth));
                break;
        }

        spriteRenderer.sprite = step.visual;
        spriteRenderer.enabled = true;
        hideTime = Time.time + duration;
    }

    private void Place(Vector2 localPosition, float angle, Vector2 scale)
    {
        transform.localPosition = localPosition;
        transform.localRotation = Quaternion.Euler(0f, 0f, angle);
        transform.localScale = new Vector3(scale.x, scale.y, 1f);
    }
}
