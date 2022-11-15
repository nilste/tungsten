using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used as a weapon hit box for the player to attack others
/// </summary>
public class AttackTrigger : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] int damageAmount = 10;

    [Header("References")]
    [SerializeField] GameObject hitVFX = null;
    private BoxCollider2D myCollider;

    void Start()
    {
        myCollider = GetComponent<BoxCollider2D>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject != null)
        {
            if (myCollider.IsTouchingLayers(LayerMask.GetMask("Enemy", "Hazards", "Destructible")))
            {
                // Swallow error message complaining about irrelevant child triggers
                other.SendMessage("TakeDamage", damageAmount, SendMessageOptions.DontRequireReceiver);
                TriggerHitVFX();
            }
        }
        
    }

    private void TriggerHitVFX()
    {
        if (!hitVFX) { return; }

        if (IsParentFacingRight())
        {
            Instantiate(hitVFX,
                transform.position + Vector3.right * 0.5f,
                transform.rotation);
        }
        else
        {
            Instantiate(hitVFX,
                transform.position + Vector3.left * 0.5f,
                transform.rotation);
        }
    }

    private bool IsParentFacingRight()
    {
        return transform.parent.localScale.x > 0;
    }
}
