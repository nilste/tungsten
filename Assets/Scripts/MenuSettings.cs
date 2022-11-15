using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

/// <summary>
/// Manage the settings menu
/// </summary>
public class MenuSettings : MonoBehaviour
{
    [Header("References")]
    [SerializeField] AudioMixer audioMixer = null;

    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("musicVolume", volume);
    }

    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat("sfxVolume", volume);
    }

    public void SetMasterVolume(float volume)
    {
        audioMixer.SetFloat("masterVolume", volume);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }

}
