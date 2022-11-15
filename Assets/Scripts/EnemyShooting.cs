using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Initiate shooting from the correct position
/// </summary>
public class EnemyShooting : MonoBehaviour
{
    [SerializeField] GameObject projectile = null;
    
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void PerformAttack()
    {
        Instantiate(projectile, transform.position, transform.rotation);
    }
}
