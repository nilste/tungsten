using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Transform game object into a destructible object
/// </summary>
public class DestructibleObject : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] int maxHealth = 20;
    private bool isAlive = true;
    private int currentHealth;

    [Header("References")]
    [SerializeField] AudioSource destroyedSFX = null;
    [SerializeField] AudioSource hitSFX = null;
    [SerializeField] GameObject destroyedVFX = null;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;

        if (isAlive)
        {
            if (currentHealth > 0)
            {
                hitSFX.Play();
            }
            else
            {
                ProcessDestruction();            
            }
        }
    }

    private void ProcessDestruction()
    {
        isAlive = false;
        GetComponent<Collider2D>().enabled = false;

        // VFX
        Instantiate(destroyedVFX, transform.position, transform.rotation);

        // SFX   
        destroyedSFX.Play();

        // Destroy object
        StartCoroutine(ScheduleDestruction());
    }

    IEnumerator ScheduleDestruction()
    {
        yield return new WaitForSeconds(.4f);
        Destroy(gameObject);
    }
}
