using UnityEngine;

// Lässt das Sprite kurz aufblinken, wenn das Objekt Schaden nimmt.
[RequireComponent(typeof(Health), typeof(SpriteRenderer))]
public class DamageFlash : MonoBehaviour
{
    [SerializeField] private Color flashColor = Color.white;
    [SerializeField] private float duration = 0.1f;

    private SpriteRenderer spriteRenderer;
    private Color normalColor;
    private float flashEndTime;
    private bool isFlashing;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        normalColor = spriteRenderer.color;
        GetComponent<Health>().Changed += OnHealthChanged;
    }

    private void OnHealthChanged(int current, int max)
    {
        spriteRenderer.color = flashColor;
        flashEndTime = Time.time + duration;
        isFlashing = true;
    }

    private void Update()
    {
        if (isFlashing && Time.time >= flashEndTime)
        {
            spriteRenderer.color = normalColor;
            isFlashing = false;
        }
    }
}
