using UnityEngine;

// Die Formen, die ein Schlag haben kann.
public enum AttackShape
{
    Circle, // rundum um den Spieler
    Cone,   // Kegel Richtung Cursor
    Line    // dünner, langer Stich Richtung Cursor
}

// Ein einzelner Schlag in der Kombo. Wird im Inspector vom PlayerAttack eingestellt.
[System.Serializable]
public class AttackStep
{
    public string name = "Schlag";
    public AttackShape shape = AttackShape.Circle;

    // Kreis: Radius. Kegel: Länge. Linie: Länge.
    [Range(0.5f, 6f)] public float range = 1.5f;

    // Nur für Cone: Öffnungswinkel in Grad.
    public float coneAngle = 120f;

    // Nur für Line: Breite in Einheiten.
    public float lineWidth = 0.5f;

    // 0 = macht keinen Schaden (z.B. ein reiner Rückstoß).
    public int damage = 1;

    // Mit welcher Geschwindigkeit Getroffene weggestoßen werden (0 = kein Rückstoß).
    public float knockback = 6f;

    // Platzhalter-Optik, die beim Schlag kurz aufblitzt.
    public Sprite visual;
    public Color color = new Color(1f, 1f, 1f, 0.4f);
}
