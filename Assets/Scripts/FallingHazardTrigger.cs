using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Trigger a falling hazard's parent object
/// </summary>
public class FallingHazardTrigger : MonoBehaviour
{
    private bool isActivated = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!isActivated && other.CompareTag("Player"))
        {
            isActivated = true;
            GetComponentInParent<FallingHazard>().ActivateHazard();
        }
    }
}
