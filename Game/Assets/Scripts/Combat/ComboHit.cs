// Ein Schlag in der Kombo des Spielers.
// Er kann aus mehreren Zonen bestehen, die alle gleichzeitig ausgelöst werden
// (z.B. ein Kegel plus ein Rundum-Stoß).
[System.Serializable]
public class ComboHit
{
    public string name = "Schlag";
    public AttackStep[] zones;
}
