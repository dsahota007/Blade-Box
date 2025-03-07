using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance; // Singleton instance

    public Text scoreText; // UI Text element for score during the game
    public Text deathScoreText; // UI Text element for score on death screen
    public Text highScoreText; // UI Text element for high score on death screen

    private int score = 0; // Current score
    private int highScore = 0; // High score

    void Awake()
    {
        // Ensure only one instance exists
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        // Load high score from PlayerPrefs
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        UpdateScoreText();
    }

    public void AddScore(int amount)
    {
        score += amount; // Increase score
        UpdateScoreText(); // Update UI
    }

    void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "" + score;
        }
    }

    public void ShowFinalScore()
    {
        // Update death screen score
        if (deathScoreText != null)
        {
            deathScoreText.text = "Score: " + score;
        }

        // Update high score if beaten
        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("HighScore", highScore);
            PlayerPrefs.Save(); // Save high score
        }

        // Show high score on death screen
        if (highScoreText != null)
        {
            highScoreText.text = "High Score: " + highScore;
        }
    }

    // Optionally reset the score when starting a new game
    public void ResetScore()
    {
        score = 0;
        UpdateScoreText();
    }
}
