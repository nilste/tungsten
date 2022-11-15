using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handle effect of picking up coin
/// </summary>
public class CoinPickup : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] int scorePickup = 1;

    [Header("References")]
    [SerializeField] AudioClip coinPickUpSFX = null;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Play SFX
        AudioSource.PlayClipAtPoint(coinPickUpSFX, transform.position);

        // Add player score to the GameSession
        FindObjectOfType<GameSession>().AddToScore(scorePickup);
        
        Destroy(gameObject);
    }
}
