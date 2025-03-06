using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public GameObject pausePanel; // Assign PausePanel in the Inspector
    private bool isPaused = false;

    void Start()
    {
        pausePanel.SetActive(false); // Hide pause menu when game starts
    }

    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f; // Pause game
        pausePanel.SetActive(true); // Show pause menu
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;
        pausePanel.SetActive(false); 
    }
    public void QuitToMenu()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene("MainMenu"); 
    }

}
