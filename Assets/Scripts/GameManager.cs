using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("UI Ёлементы")]
    public GameObject gameOverPanel;
    public GameObject victoryPanel;
    public TextMeshProUGUI gameOverReasonText;

    public void ShowGameOver(string reason)
    {
        if (gameOverReasonText != null)
        {
            gameOverReasonText.text = reason;
        }

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
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}