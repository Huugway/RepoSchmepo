using UnityEngine;

// Zeigt eine Zone eines Schlags kurz als halbtransparente Form an (Platzhalter-Optik).
// Liegt als Prefab vor: Der Angreifer erzeugt pro Zone eine Kopie als Kind-Objekt,
// ruft Show() auf, und die Kopie löscht sich nach "duration" selbst.
[RequireComponent(typeof(SpriteRenderer))]
public class AttackFlash : MonoBehaviour
{
    [SerializeField] private float duration = 0.1f;

    // Größe und Drehung passend zur Form setzen.
    // Die Sprites sind so gebaut: Kreis = 1 Einheit Durchmesser,
    // Kegel = 1 Einheit lang mit Spitze in der Mitte, Quadrat = 1x1.
    public void Show(AttackStep step, Vector2 aim)
    {
        float angle = MeleeHit.AngleOf(aim);

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

        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = step.visual;
        spriteRenderer.color = step.color;

        Destroy(gameObject, duration);
    }

    // Position und Größe sind in Welt-Einheiten gemeint. Ist das Eltern-Objekt
    // skaliert (z.B. ein großer Tank), wird das hier wieder herausgerechnet.
    private void Place(Vector2 offset, float angle, Vector2 size)
    {
        Vector3 parentScale = transform.parent != null ? transform.parent.lossyScale : Vector3.one;

        transform.localPosition = new Vector3(offset.x / parentScale.x, offset.y / parentScale.y, 0f);
        transform.localRotation = Quaternion.Euler(0f, 0f, angle);
        transform.localScale = new Vector3(size.x / parentScale.x, size.y / parentScale.y, 1f);
    }
}
