using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void PlayGame()
    {
        Time.timeScale = 1f; // Ensure time is normal before loading
        SceneManager.LoadScene("GameMainScene"); // Load Game Scene
    }
}
