using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.UIElements;
using TMPro;
using Cursor = UnityEngine.Cursor;

public class menu : MonoBehaviour
{
    public AudioMixer audioMixer;
    private Dropdown resolutionDropdown;
    public TMP_Dropdown qualityDropdown;
    private Dropdown textureDropdown;
    private Dropdown aaDropdown;
    public UnityEngine.UI.Slider volumeSlider;
    float currentVolume;
    public GameObject[] cameras;
    public GameObject pauseMenu;
    public GameManager gm;
    public AudioMixer audiomixer;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        MenuCanvas();
        Time.timeScale = 1;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(scene.name == "Deep_Tunnel_set" || scene.name == "FinalScene" || scene.name == "BossTestScene" || scene.name == "BossFight")
        {
            gm = GameObject.Find("Game Manager").GetComponent<GameManager>();
            Debug.Log("Game Manager found!");
        }
        Debug.Log("OnSceneLoaded: " + scene.name);
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void Instructions()
    {
        for (int i = 0; i < cameras.Length; i++)
        {
            cameras[i].SetActive(false);
        }

        if (cameras.Length > 0)
        {
            cameras[0].SetActive(true);
        }
    }

    public void Options()
    {
        for (int i = 0; i < cameras.Length; i++)
        {
            cameras[i].SetActive(false);
        }

        if (cameras.Length > 0)
        {
            cameras[1].SetActive(true);
        }
    }

    public void Volume()
    {
        for (int i = 0; i < cameras.Length; i++)
        {
            cameras[i].SetActive(false);
        }

        if (cameras.Length > 0)
        {
            cameras[2].SetActive(true);
        }
    }

    public void MenuCanvas()
    {
        for (int i = 0; i < cameras.Length; i++)
        {
            cameras[i].SetActive(false);
        }

        if (cameras.Length > 0)
        {
            cameras[3].SetActive(true);
        }
    }

    public void setFullscreen(bool fullscreen)
    {
        Screen.fullScreen = fullscreen;
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("Volume", volume);
    }

    public void SetTextureQuality(int textureIndex)
    {
        QualitySettings.masterTextureLimit = textureIndex;
        qualityDropdown.value = 4;
    }
    public void SetAntiAliasing(int aaIndex)
    {
        QualitySettings.antiAliasing = aaIndex;
        qualityDropdown.value = 4;
    }

    public void SetQuality(int qualityIndex)
    {
            QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void IsMuted(bool mute)
    {
        if (mute == true)
        {
            audioMixer.SetFloat("Volume", -80);
            currentVolume = -80;
        }
        else
        {
            audioMixer.SetFloat("Volume", 0);
        }
    }

    public void Resume()
    {
        gm.isPaused = false;
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        gm.audioList[3].volume = 0.193f;
    }

    public void Quit()
    {
        Application.Quit();
    }
}
