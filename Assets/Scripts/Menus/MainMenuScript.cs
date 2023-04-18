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

        if(!PlayerPrefs.HasKey("Resolution"))
        {
            PlayerPrefs.SetInt("Resolution", currentResolutionIndex);
            resolutionDropdown.value = currentResolutionIndex;
            resolutionDropdown.RefreshShownValue();
        } else {
            currentResolutionIndex = PlayerPrefs.GetInt("Resolution");
            resolutionDropdown.value = currentResolutionIndex;
            resolutionDropdown.RefreshShownValue();
        }

        if(!PlayerPrefs.HasKey("Fullscreen"))
        {
            PlayerPrefs.SetInt("Fullscreen", 1);
            SetFullscreen(true);
            fullscreenToggle.isOn = true;
        } else {
            SetFullscreen(PlayerPrefs.GetInt("Fullscreen") == 1 ? true : false);
            fullscreenToggle.isOn = PlayerPrefs.GetInt("Fullscreen") == 1 ? true : false;
        }

        if(!PlayerPrefs.HasKey("Quality"))
        {
            PlayerPrefs.SetInt("Quality", 3);
            graphicsDropdown.value = 3;
            graphicsDropdown.RefreshShownValue();
            SetQuality(3);
        } else {
            graphicsDropdown.value = PlayerPrefs.GetInt("Quality");
            graphicsDropdown.RefreshShownValue();
            SetQuality(PlayerPrefs.GetInt("Quality"));
        }

        if(PlayerPrefs.HasKey("SFXVol"))
        {
            SetSFXVolume(PlayerPrefs.GetFloat("SFXVol"));
            sfxVolumeSlider.value = PlayerPrefs.GetFloat("SFXVol");
        } else {
            SetSFXVolume(-40);
            sfxVolumeSlider.value = -40;
        }
        if(PlayerPrefs.HasKey("musicVol"))
        {
            SetMusicVolume(PlayerPrefs.GetFloat("musicVol"));
            musicVolumeSlider.value = PlayerPrefs.GetFloat("musicVol");
        } else {
            SetMusicVolume(-40);
            musicVolumeSlider.value = -40;
        }
    }

    public void SetSFXVolume(float volume){
        audioMixer.SetFloat("SFXVol", volume);
        PlayerPrefs.SetFloat("SFXVol", volume);
    }

    public void SetMusicVolume(float volume){
        audioMixer.SetFloat("musicVol", volume);
        PlayerPrefs.SetFloat("musicVol", volume);
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
        PlayerPrefs.SetInt("Quality", qualityIndex);
    }

    public void SetFullscreen (bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        PlayerPrefs.SetInt("Fullscreen", isFullscreen ? 1 : 0);        
    }

    public void SetResolution (int resIndex)
    {
        Resolution resolution = resolutions[resIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        PlayerPrefs.SetInt("Resolution", resIndex);
    }

    public void ResetAudio()
    {
        musicVolumeSlider.value = -40;
        sfxVolumeSlider.value = -40;
    }

    public void ResetGraphics()
    {
        resolutionDropdown.value = resolutionDropdown.options.Count - 1;
        resolutionDropdown.RefreshShownValue();
        SetResolution(resolutionDropdown.options.Count - 1);

        SetFullscreen(true);
        fullscreenToggle.isOn = true;

        graphicsDropdown.value = 3;
        graphicsDropdown.RefreshShownValue();
        SetQuality(3);

    }

}
