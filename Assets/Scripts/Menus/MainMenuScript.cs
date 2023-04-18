using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;

public class MainMenuScript : MonoBehaviour
{
    [SerializeField]
    private AudioSource clickSound;
        [SerializeField]
    private AudioSource menuMusic;
    
    [Header("Options Menu")]
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;
    [SerializeField] private Dropdown resolutionDropdown;
    [SerializeField] private Dropdown graphicsDropdown;
    [SerializeField] private Toggle fullscreenToggle;

    private Resolution[] resolutions;
    private int currentResolutionIndex = 0;
    

    private void Start()
    {
        if (menuMusic != null)
        {
            menuMusic.Play();
        }

        // Setup the resolution dropdown
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height + " @ " + resolutions[i].refreshRate + "hz";
            options.Add(option);

            if(resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        graphicsDropdown.value = QualitySettings.GetQualityLevel();
        graphicsDropdown.RefreshShownValue();

        

        SetSFXVolume(-40);
        SetMusicVolume(-40);
    }

    public void SetSFXVolume(float volume){
        audioMixer.SetFloat("SFXVol", volume);
    }

    public void SetMusicVolume(float volume){
        audioMixer.SetFloat("musicVol", volume);
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void PlayClickSound()
    {
        if (clickSound != null)
        {
            clickSound.Play();
        }
    }
    

    public void SetQuality (int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFullscreen (bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void SetResolution (int resIndex)
    {
        Resolution resolution = resolutions[resIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void ResetAudio()
    {
        musicVolumeSlider.value = -40;
        sfxVolumeSlider.value = -40;
    }

    public void ResetGraphics()
    {
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        SetFullscreen(true);
        fullscreenToggle.isOn = true;


        graphicsDropdown.value = 3;
        graphicsDropdown.RefreshShownValue();

    }

}
