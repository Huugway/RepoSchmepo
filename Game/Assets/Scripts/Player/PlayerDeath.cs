using UnityEngine;
using UnityEngine.SceneManagement;

// Was passiert, wenn der Spieler stirbt: Fürs Erste wird das Level neu gestartet.
[RequireComponent(typeof(Health))]
public class PlayerDeath : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<Health>().Died += OnDied;
    }

    private void OnDied()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
