using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Manage the main menu
/// </summary>
public class MenuMain : MonoBehaviour
{
    // References
    private AudioSource audioSource;
    private GameSession gameSession;
    
    // Variables
    private float audioTimer = 0f;

    void Start()
    {
        // Fetch data about button click SFX
        audioSource = this.GetComponent<AudioSource>();
        audioTimer = audioSource.clip.length;

        // Unpause the game
        Time.timeScale = 1f;
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void StartFirstLevel()
    {
        // Reset player data before launching
        gameSession = FindObjectOfType<GameSession>();
        if (gameSession != null)
        {
            gameSession.Reset();
        }

        audioSource.Play();
        StartCoroutine(LoadMyScene("Level1"));
    }

    public void LoadLevels()
    {
        audioSource.Play();
        StartCoroutine(LoadMyScene("Levels"));
    }

    public void LoadSettings()
    {
        audioSource.Play();
        StartCoroutine(LoadMyScene("Settings"));
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    IEnumerator LoadMyScene(string sceneName)
    {
        yield return new WaitForSeconds(audioTimer);
        SceneManager.LoadScene(sceneName);
    }
}
