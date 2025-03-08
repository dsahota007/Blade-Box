using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    public Text scoreText;
    public Text deathScoreText;
    public Text highScoreText;

    private int score = 0;
    private int highScore = 0;

    private int displayedScore = 0;
    private Coroutine scoreAnimation;

    void Awake()
    {
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
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        UpdateScoreText();
    }

    // ✅ Add Score with Smooth Animation and Scale Effect
    public void AddScore(int amount)
    {
        score += amount;

        if (scoreAnimation != null)
            StopCoroutine(scoreAnimation);

        scoreAnimation = StartCoroutine(AnimateScore());

        // Scale effect when score increases
        StartCoroutine(ScaleTextEffect());
    }

    // ✅ Coroutine to Increment Score Smoothly
    IEnumerator AnimateScore()
    {
        float duration = 0.5f; // Duration of the increment animation
        float elapsed = 0f;

        int startingScore = displayedScore;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            displayedScore = (int)Mathf.Lerp(startingScore, score, elapsed / duration);
            UpdateScoreText();
            yield return null;
        }

        displayedScore = score;
        UpdateScoreText();
    }

    // ✅ Scale Animation (Bounce Effect)
    IEnumerator ScaleTextEffect()
    {
        Vector3 originalScale = scoreText.transform.localScale;
        Vector3 targetScale = originalScale * 1.1f; // Scale up by 20%

        // Scale up
        float elapsed = 0f;
        float duration = 0.1f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            scoreText.transform.localScale = Vector3.Lerp(originalScale, targetScale, elapsed / duration);
            yield return null;
        }

        // Scale down
        elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            scoreText.transform.localScale = Vector3.Lerp(targetScale, originalScale, elapsed / duration);
            yield return null;
        }
    }

    // ✅ Update the Score Text
    void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "" + displayedScore;
        }
    }

    // ✅ Show Final Score on Death Screen
    public void ShowFinalScore()
    {
        if (deathScoreText != null)
        {
            deathScoreText.text = "Score: " + score;
        }

        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("HighScore", highScore);
            PlayerPrefs.Save();
        }

        if (highScoreText != null)
        {
            highScoreText.text = "High Score: " + highScore;
        }
    }

    // ✅ Reset the Score
    public void ResetScore()
    {
        score = 0;
        displayedScore = 0;
        UpdateScoreText();
    }
}
