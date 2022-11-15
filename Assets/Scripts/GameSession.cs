using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Game session manager
/// </summary>
public class GameSession : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] int maxPlayerLives = 3;

    private int currentPlayerLives;
    private int currentScore = 0;

    private void Awake()
    {
        // Find number of game sessions in play
        int numGameSessions = FindObjectsOfType<GameSession>().Length;

        // Singleton pattern
        if (numGameSessions > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }

        currentPlayerLives = maxPlayerLives;
    }

    public void ProcessPlayerDeath()
    {
        if (currentPlayerLives > 1)
        {
            TakeLife();
        }
        else
        {
            // Load main menu
            SceneManager.LoadScene("GameOver");
            Destroy(gameObject);
        }
    }

    public void AddToScore(int pointsToAdd)
    {
        currentScore += pointsToAdd;
    }

    public void TakeLife()
    {
        --currentPlayerLives;

        // Reload the current level
        var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    public int GetPlayerLives()
    {
        return currentPlayerLives;
    }

    public int GetScore()
    {
        return currentScore;
    }

    public void Reset()
    {
        currentPlayerLives = maxPlayerLives;
        currentScore = 0;
    }
}
