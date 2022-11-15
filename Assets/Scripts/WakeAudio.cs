using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Play the object's audio when the player comes near
/// </summary>
public class WakeAudio : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] float wakeDistance = 0f;

    private AudioSource mySFX;
    private GameObject player;

    void Start()
    {
        mySFX = GetComponent<AudioSource>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        float distance = Vector3.Distance(player.transform.position, transform.position);
        bool withinDistance = distance < wakeDistance;

        if (withinDistance)
        {
            if (!mySFX.isPlaying)
            {
                mySFX.Play();
            }
        }
        else
        {
            if (mySFX.isPlaying)
            {
                mySFX.Stop();
            }
        }
    }
}
