using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("UI Элементы")]
    public GameObject mainMenuPanel;
    public GameObject gameOverPanel;
    public GameObject victoryPanel;
    public TextMeshProUGUI gameOverReasonText;
    public TextMeshProUGUI musicButtonText;

    [Header("Аудио")]
    public AudioSource backgroundMusic;

    private bool isMusicOn = true;

    private static bool skipMainMenu = false;

    void Start()
    {
        if (skipMainMenu)
        {
            skipMainMenu = false;
            if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
            if (gameOverPanel != null) gameOverPanel.SetActive(false);
            if (victoryPanel != null) victoryPanel.SetActive(false);
            Time.timeScale = 1f;
        }
        else
        {
            if (mainMenuPanel != null) mainMenuPanel.SetActive(true);
            if (gameOverPanel != null) gameOverPanel.SetActive(false);
            if (victoryPanel != null) victoryPanel.SetActive(false);
            Time.timeScale = 0f;
        }
    }

    public void StartGame()
    {
        if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void ToggleMusic()
    {
        isMusicOn = !isMusicOn;
        if (backgroundMusic != null) backgroundMusic.mute = !isMusicOn;
        if (musicButtonText != null) musicButtonText.text = isMusicOn ? "МУЗЫКА: ВКЛ" : "МУЗЫКА: ВЫКЛ";
    }

    public void ShowGameOver(string reason)
    {
        if (gameOverReasonText != null) gameOverReasonText.text = reason;
        if (gameOverPanel != null) gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ShowVictory()
    {
        if (victoryPanel != null) victoryPanel.SetActive(true);
        Time.timeScale = 0f;
    }
    public void RestartGame()
    {
        skipMainMenu = true;
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ReturnToMainMenu()
    {
        skipMainMenu = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
