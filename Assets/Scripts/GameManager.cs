using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameObject gameOverPanel;
    public GameObject victoryPanel;
    public TextMeshProUGUI notesText;

    private int totalNotes = 0;
    private int maxNotes = 7;
    private bool isGameActive = true;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void AddNote()
    {
        if (!isGameActive) return;

        totalNotes++;
        notesText.text = $"Notas: {totalNotes} / {maxNotes}";
        if (totalNotes >= maxNotes)
        {
            GanarJuego();
        }
    }

    public int GetNotesCount() => totalNotes;

    public void PerderJuego()
    {
        isGameActive = false;
        gameOverPanel.SetActive(true);
        DesbloquearCursor();
    }

    private void GanarJuego()
    {
        isGameActive = false;
        victoryPanel.SetActive(true);
        DesbloquearCursor();

    }

    private void DesbloquearCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ReiniciarNivel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void VolverAlMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void IniciarJuego()
    {
        SceneManager.LoadScene("Level1");
    }
}