using System.Collections.Generic;
using UnityEngine;

// Führt einen Schlag aus: findet alle Ziele in der Form des AttackStep,
// macht Schaden und stößt sie weg. Wird von Spieler und Gegnern benutzt.
public static class MeleeHit
{
    public static void Perform(AttackStep step, Vector2 origin, Vector2 aim, Team attackerTeam)
    {
        foreach (Collider2D hit in FindTargets(step, origin, aim))
        {
            Health health = hit.GetComponent<Health>();

            // Nur Dinge mit Health treffen, nur die andere Seite, und nichts Unverwundbares
            if (health == null || health.Team == attackerTeam || health.IsInvincible)
                continue;

            if (step.damage > 0)
                health.TakeDamage(step.damage);

            Knockback knockback = hit.GetComponent<Knockback>();
            if (knockback != null)
                knockback.Apply(AwayFrom(origin, hit.transform.position, aim) * step.knockback);
        }
    }

    // Sucht alle Collider in der Form des Schlags.
    private static List<Collider2D> FindTargets(AttackStep step, Vector2 origin, Vector2 aim)
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
                // Eine gedrehte Box, die vom Angreifer aus in Zielrichtung zeigt
                Vector2 center = origin + aim * (step.range / 2f);
                Vector2 size = new Vector2(step.range, step.lineWidth);
                return new List<Collider2D>(Physics2D.OverlapBoxAll(center, size, AngleOf(aim)));

            default:
                return new List<Collider2D>();
        }
    }

    // Richtung vom Angreifer weg. Steht das Ziel genau auf dem Angreifer, in Zielrichtung.
    private static Vector2 AwayFrom(Vector2 origin, Vector2 targetPosition, Vector2 aim)
    {
        Vector2 away = targetPosition - origin;
        return away == Vector2.zero ? aim : away.normalized;
    }

    // Winkel einer Richtung in Grad (0 = rechts, 90 = oben).
    public static float AngleOf(Vector2 direction)
    {
        return Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    }
}
