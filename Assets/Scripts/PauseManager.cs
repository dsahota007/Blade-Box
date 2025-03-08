using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    public GameObject pausePanel; // Assign in the Inspector
    private bool isPaused = false;

    [Header("UI")]
    public Text audioToggleText; // Drag the button's Text component here

    void Start()
    {
        pausePanel.SetActive(false);
        UpdateAudioToggleText(); // Set initial state
    }

    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;
        pausePanel.SetActive(true);
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

    // ✅ Toggle Audio (Just Drag to Button)
    public void ToggleAudio()
    {
        AudioListener.pause = !AudioListener.pause;
        UpdateAudioToggleText();
    }

    // ✅ Update Button Text
    private void UpdateAudioToggleText()
    {
        if (audioToggleText != null)
        {
            audioToggleText.text = AudioListener.pause ? "Audio Off" : "Audio On";
        }
    }
}
