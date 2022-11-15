using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Manage the levels menu
/// </summary>
public class MenuLevels : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Text[] timeList = null;
    private AudioSource audioSource;
    private GameSession gameSession;
    
    private float audioTimer = .3f;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        // Fetch record time from player prefs
        timeList[0].text = PlayerPrefs.GetInt("Level1", 0).ToString() + " seconds";
        timeList[1].text = PlayerPrefs.GetInt("Level2", 0).ToString() + " seconds";
        timeList[2].text = PlayerPrefs.GetInt("Level3", 0).ToString() + " seconds";
    }

    public void ResetScore()
    {
        audioSource.Play();
        
        // Delete all saved highscores
        PlayerPrefs.DeleteKey("Level1");
        PlayerPrefs.DeleteKey("Level2");
        PlayerPrefs.DeleteKey("Level3");

        // Update labels with new values
        foreach (Text item in timeList)
        {
            item.text = "0 seconds";
        }
    }

    public void LoadMainMenu()
    {
        audioSource.Play();
        StartCoroutine(LoadMyScene(0));
    }

    public void LoadLevel1()
    {
        audioSource.Play();
        StartCoroutine(LoadMyScene(1));
    }

    public void LoadLevel2()
    {
        audioSource.Play();
        StartCoroutine(LoadMyScene(2));
    }

    public void LoadLevel3()
    {
        audioSource.Play();
        StartCoroutine(LoadMyScene(3));
    }

    IEnumerator LoadMyScene(int sceneNumber)
    {
        yield return new WaitForSeconds(audioTimer);

        // Reset player data before launching
        gameSession = FindObjectOfType<GameSession>();
        if (gameSession != null)
        {
            gameSession.Reset();
        }
        
        SceneManager.LoadScene(sceneNumber);
    }

}
