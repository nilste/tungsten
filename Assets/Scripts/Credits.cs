using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Play credits
/// </summary>
public class Credits : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] float scrollSpeed = 70f;

    void Update()
    {
        gameObject.transform.Translate(Vector3.up * scrollSpeed * Time.deltaTime);
        Debug.Log(transform.position.y);

        if (Input.GetButtonDown("Pause") || transform.position.y > 2000)
        {
            SceneManager.LoadScene(0);
        }
    }
}
