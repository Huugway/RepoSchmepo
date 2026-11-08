using UnityEngine;

// Lässt beim Tod ein Objekt fallen (z.B. einen XP-Orb).
[RequireComponent(typeof(Health))]
public class DropOnDeath : MonoBehaviour
{
    [SerializeField] private GameObject dropPrefab;

    private void Awake()
    {
        GetComponent<Health>().Died += Drop;
    }

    private void Drop()
    {
        Instantiate(dropPrefab, transform.position, Quaternion.identity);
    }
}
