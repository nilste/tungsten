using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Manage the pause menu
/// </summary>
public class MenuPause : MonoBehaviour
{
    [Header("References")]
    [SerializeField] GameObject pauseUI = null;

    private bool paused = false;

    void Start()
    {
        pauseUI.SetActive(false);
    }

    void Update()
    {
        // If the player paused and isn't in the main menu
        if (Input.GetButtonDown("Pause") &&
            SceneManager.GetActiveScene().buildIndex > 0)
        {
            // Toggle pause status
            paused = !paused;

            if (paused)
            {
                PauseGame();
            }
            else
            {
                ResumeGame();
            }
        }
    }

    public void PauseGame()
    {
        paused = true;

        // Show the UI
        pauseUI.SetActive(true);

        // Pause the game
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        paused = false;

        // Hide the UI
        pauseUI.SetActive(false);

        // Resume the game
        Time.timeScale = 1;
    }

    public void MainMenu()
    {
        pauseUI.SetActive(false);
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
