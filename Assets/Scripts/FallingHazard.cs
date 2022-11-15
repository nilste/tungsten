using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manage falling hazard
/// </summary>
public class FallingHazard : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] float fallDelay = .3f;
    private bool isActivated = false;
    
    // References
    private AudioSource mySFX;
    private Rigidbody2D myRB2D;
    private Collider2D myAttackArea;

    void Start()
    {
        myRB2D = GetComponent<Rigidbody2D>();
        myAttackArea = GetComponent<CircleCollider2D>();
        mySFX = GetComponent<AudioSource>();
    }

    void Update()
    {
        // Disable it when it touches the ground, to avoid falling through the ground
        if (isActivated && myAttackArea.IsTouchingLayers(LayerMask.GetMask("Ground"))) {
            StartCoroutine(MakeHarmless());
        }
    }

    public void ActivateHazard()
    {
        StartCoroutine(Fall());
    }

    IEnumerator Fall()
    {
        isActivated = true;
        yield return new WaitForSeconds(fallDelay);
        
        // Delete children that isn't light
        foreach (Transform child in transform)
        {   
            if (child.gameObject.GetComponent<Light>() == null)
            {
                Destroy(child.gameObject);
            }
        }
        
        // Make it fall
        myRB2D.isKinematic = false;

        // Play SFX
        mySFX.Play();
    }

    IEnumerator MakeHarmless()
    {
        yield return new WaitForSeconds(1f);

        // Make it stay on the ground
        myRB2D.isKinematic = true;

        // Make it harmless for the player
        myAttackArea.enabled = false;
    }
}
