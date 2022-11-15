using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Manage the level completion process
/// </summary>
public class LevelExit : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] float levelLoadDelay = 3f;

    [Header("References")]
    [SerializeField] GameObject completedVFX = null;

    void OnTriggerEnter2D(Collider2D other)
    {
        StartCoroutine(LoadNextLevel());
    }

    IEnumerator LoadNextLevel()
    {
        // If this was a new record, store the time
        string sceneName = SceneManager.GetActiveScene().name;
        int oldTime = PlayerPrefs.GetInt(sceneName, 0);
        int newTime = (int)Time.timeSinceLevelLoad;

        if (oldTime == 0 || newTime < oldTime)
        {
            PlayerPrefs.SetInt(sceneName, newTime);
        }

        // Play VFX
        Instantiate(completedVFX, transform.position, transform.rotation);

        // Set it to wait before loading next level
        yield return new WaitForSecondsRealtime(levelLoadDelay);

        // Load next level
        var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }
}
