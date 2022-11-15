using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manage interactable object that gives the player more health
/// </summary>
public class HealthPickup : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] int healthPickup = 1;

    [Header("References")]
    [SerializeField] AudioClip healthPickUpSFX = null;
    private PlayerScript player;

    void Start()
    {
        player = FindObjectOfType<PlayerScript>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (player.GetCurrentHealth() < player.GetMaxHealth())
        {
            // Play SFX
            AudioSource.PlayClipAtPoint(healthPickUpSFX, transform.position, .4f);

            // Add player score to the GameSession
            player.AddToHealth(healthPickup);
            
            Destroy(gameObject);
        }
    }
}
