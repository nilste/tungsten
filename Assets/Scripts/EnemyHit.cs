using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A generic hit box for enemies
/// </summary>
public class EnemyHit : MonoBehaviour
{
    private BoxCollider2D myBoxCollider;
    private GameObject playerObject;

    void Start()
    {
        playerObject = GameObject.FindWithTag("Player");
        myBoxCollider = GetComponent<BoxCollider2D>();
    }

    public void PerformAttack()
    {
        if (myBoxCollider.IsTouchingLayers(LayerMask.GetMask("Player")))
        {
            playerObject.GetComponent<PlayerScript>().TakeDamage(1);
        }
    }
}
