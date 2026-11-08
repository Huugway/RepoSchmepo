using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Zeigt den Game-Over-Bildschirm, wenn der Spieler stirbt.
// Das Spiel wird angehalten, mit R startet man neu.
// Kommt auf ein immer aktives Objekt (z.B. den Canvas), das Panel selbst ist anfangs aus.
public class GameOverScreen : MonoBehaviour
{
    [SerializeField] private Health playerHealth;
    [SerializeField] private EnemySpawner spawner;
    [SerializeField] private GameObject panel;
    [SerializeField] private Text detailsText;

    private bool isShown;

    private void Awake()
    {
        panel.SetActive(false);
        playerHealth.Died += Show;
    }

    private void OnDestroy()
    {
        if (playerHealth != null)
            playerHealth.Died -= Show;
    }

    private void Show()
    {
        isShown = true;
        panel.SetActive(true);
        detailsText.text = $"Wave {spawner.CurrentWave}\n\nR = Restart";

        // Spieler ausblenden und alles anhalten
        playerHealth.gameObject.SetActive(false);
        Time.timeScale = 0f;
    }

    // Update läuft auch bei timeScale 0 weiter, darum kann man hier auf R warten.
    private void Update()
    {
        if (isShown && Keyboard.current != null && Keyboard.current.rKey.wasPressedThisFrame)
            Restart();
    }

    private void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
