using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Make the button clickable so we can return to main menu
/// </summary>
public class GameOver : MonoBehaviour
{
    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
