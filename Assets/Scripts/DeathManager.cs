using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject deathScreenPanel; // Assign DeathScreenPanel in Inspector

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ShowDeathScreen()
    {
        StartCoroutine(DeathScreenCoroutine());
    }

    private IEnumerator DeathScreenCoroutine()
    {
        yield return new WaitForSeconds(1.5f); // Wait 1 second

        if (deathScreenPanel != null)
        {
            deathScreenPanel.SetActive(true);
            Debug.Log("Death Screen Activated!");
        }
        else
        {
            Debug.LogError("Death Screen Panel is not assigned in GameManager!");
        }
    }
    public void QuitToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
