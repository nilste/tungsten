using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Singleton used to manage the background music
/// </summary>
public class MusicPlayer : MonoBehaviour
{
    void Start()
    {
        InitiateMusicPlayer();
    }

    private void InitiateMusicPlayer()
    {
        if (FindObjectsOfType(GetType()).Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
