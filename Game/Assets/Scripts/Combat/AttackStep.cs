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
    public float range = 1.5f;

    // Nur für Cone: Öffnungswinkel in Grad.
    public float coneAngle = 120f;

    // Nur für Line: Breite in Einheiten.
    public float lineWidth = 0.5f;

    public int damage = 1;

    // Platzhalter-Optik, die beim Schlag kurz aufblitzt.
    public Sprite visual;
}
