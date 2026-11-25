using UnityEngine;
using TMPro;

public class PlayerCollision : MonoBehaviour
{
    public int score = 0;
    public TextMeshProUGUI scoreText;
    public TunnelMovement movementScript;
    public GameManager gameManager;
    public int requiredScore = 2048;

    private void Start()
    {
        UpdateScoreUI();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            if (movementScript != null && movementScript.isDashing)
            {
                Destroy(other.gameObject);
                score += 5;
                UpdateScoreUI();
            }
            else
            {
                if (gameManager != null) gameManager.ShowGameOver("ÀÍÒÈÂÈÐÓÑ ÎÁÍÀÐÓÆÈË ÂÀÑ!");
            }
        }
        else if (other.CompareTag("Packet"))
        {
            score += 8;
            UpdateScoreUI();
            Destroy(other.gameObject);
        }
    }

    public bool HasEnoughData()
    {
        return score >= requiredScore;
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = score.ToString() + " / " + requiredScore.ToString() + " MB";
        }
    }
    public bool TrySpendData(int cost)
    {
        if (score >= cost)
        {
            score -= cost;
            UpdateScoreUI();
            return true;
        }
        return false;
    }
}