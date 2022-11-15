using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manage enemy projectiles
/// </summary>
public class Projectile : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] float moveSpeed = 4f;
    [SerializeField] float lifespan = 6f;
    
    private Rigidbody2D myRB2D;
    private GameObject playerObject;

    void Start()
    {
        myRB2D = GetComponent<Rigidbody2D>();
        playerObject = GameObject.FindWithTag("Player");

        // Orient towards player
        float distanceDifference = transform.position.x - playerObject.transform.position.x;
        transform.localScale = new Vector3(-(Mathf.Sign(distanceDifference)), 1f, 1f);

        // Set move speed towards player
        myRB2D.velocity = new Vector2(-(Mathf.Sign(distanceDifference)) * moveSpeed, 0f);
        
        Destroy(gameObject, lifespan);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerScript hit = other.GetComponent<PlayerScript>();
        if (hit != null && other.gameObject.tag == "Player")
        {
            hit.TakeDamage(1);
            Destroy(gameObject);
        }
    }
}
