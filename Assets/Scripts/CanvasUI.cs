using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manage the GUI with score, health, and lives
/// </summary>
public class CanvasUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Sprite[] healthBars = null;
    [SerializeField] Image healthUI = null;
    [SerializeField] Text livesText = null;
    [SerializeField] Text scoreText = null;
    private GameObject player;
    private GameSession gameSession;

    void Update()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            int currentHealth = player.GetComponent<PlayerScript>().GetCurrentHealth();
            if (currentHealth >= 0 && currentHealth <= healthBars.Length)
            {
                healthUI.sprite = healthBars[currentHealth];
            }
        }

        gameSession = FindObjectOfType<GameSession>();
        if (gameSession != null)
        {
            int playerLives = gameSession.GetComponent<GameSession>().GetPlayerLives();
            int score = gameSession.GetComponent<GameSession>().GetScore();
        
            // Update GUI
            livesText.text = playerLives.ToString();
            scoreText.text = score.ToString();
        }
    }
}
