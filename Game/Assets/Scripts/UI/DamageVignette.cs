using UnityEngine;
using UnityEngine.UI;

// Roter Bildschirmrand (Vignette) für den Spieler:
// - Bei einem Treffer blitzt er kurz kräftig auf und blendet dann aus.
// - Bei wenig HP bleibt er dauerhaft sichtbar, bei 1 HP stärker als bei 2 HP.
// Kommt auf ein UI-Image über den ganzen Bildschirm mit dem Vignette-Sprite.
[RequireComponent(typeof(Image))]
public class DamageVignette : MonoBehaviour
{
    [SerializeField] private Health playerHealth;

    // Wie deckend die Vignette ist (0 = unsichtbar, 1 = voll)
    [SerializeField, Range(0f, 1f)] private float hitAlpha = 0.7f;
    [SerializeField, Range(0f, 1f)] private float twoHealthAlpha = 0.3f;
    [SerializeField, Range(0f, 1f)] private float oneHealthAlpha = 0.55f;

    // Wie schnell der Treffer-Blitz wieder ausblendet (Alpha pro Sekunde)
    [SerializeField] private float fadeSpeed = 1.5f;

    private Image image;
    private float alpha;

    private void Awake()
    {
        image = GetComponent<Image>();
        SetAlpha(0f);
    }

    private void OnEnable()
    {
        playerHealth.Damaged += OnDamaged;
    }

    private void OnDisable()
    {
        playerHealth.Damaged -= OnDamaged;
    }

    private void OnDamaged(int amount)
    {
        alpha = hitAlpha;
    }

    // Langsam zurück auf den Wert, der zu den aktuellen HP passt.
    // unscaledDeltaTime, damit es auch im pausierten Game Over noch ausblendet.
    private void Update()
    {
        alpha = Mathf.MoveTowards(alpha, GetLowHealthAlpha(), fadeSpeed * Time.unscaledDeltaTime);
        SetAlpha(alpha);
    }

    private float GetLowHealthAlpha()
    {
        if (playerHealth.Current <= 1)
            return oneHealthAlpha;

        if (playerHealth.Current == 2)
            return twoHealthAlpha;

        return 0f;
    }

    private void SetAlpha(float value)
    {
        Color color = image.color;
        color.a = value;
        image.color = color;
    }
}
