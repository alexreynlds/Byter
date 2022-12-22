using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [Header("Volume Setting")]
    [SerializeField]
    private TMP_Text volumeTextValue = null;

    [SerializeField]
    private Slider volumeSlider = null;

    [SerializeField]
    private float defaultVolume = 0.5f;

    [SerializeField]
    private Button noSaveFoundOk = null;

    [Header("Gameplay Settings")]
    [SerializeField]
    private TMP_Text controllerSensTextValue = null;

    [SerializeField]
    private Slider controllerSensSlider = null;

    [SerializeField]
    private int defaultSens = 4;

    public int mainControllerSens = 4;

    [Header("Toggle Settings")]
    [SerializeField]
    private Toggle invertYToggle = null;

    [Header("Graphics Settings")]
    [SerializeField]
    private Slider brightnessSlider = null;

    [SerializeField]
    private TMP_Text brightnessTextValue = null;

    [SerializeField]
    private float defaultBrightness = 1f;

    [Space(10)]
    [SerializeField]
    private TMP_Dropdown qualityDropdown;

    [SerializeField]
    private Toggle fullscreenToggle;

    private int _qualityIndex;

    private bool _isFullscreen;

    private float _brightnessLevel;

    [Header("Confirmation Box")]
    [SerializeField]
    private GameObject confirmationPrompt = null;

    [Header("Resolution Dropdowns")]
    public TMP_Dropdown resolutionDropdown;

    private Resolution[] resolutions;

    [Header("Levels to load")]
    public string _newGameScene;

    private string levelToLoad;

    [SerializeField]
    private GameObject noSavedGameDialog = null;

    public GameObject LoadMarker;

    [Header("Menu Music")]
    [SerializeField]
    private AudioClip menuMusic;

    void Start()
    {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();
        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option =
                resolutions[i].width + " x " + resolutions[i].height;
            options.Add (option);
            if (
                resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height
            )
            {
                currentResolutionIndex = i;
            }
        }
        resolutionDropdown.AddOptions (options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        brightnessSlider.value = PlayerPrefs.GetFloat("brightnessLevel");
        controllerSensSlider.value = PlayerPrefs.GetInt("controllerSens");
        invertYToggle.isOn = PlayerPrefs.GetInt("invertY") == 1;
        qualityDropdown.value = PlayerPrefs.GetInt("qualityIndex");
        fullscreenToggle.isOn = PlayerPrefs.GetInt("isFullscreen") == 1;
        volumeSlider.value = PlayerPrefs.GetFloat("masterVolume");

        if (menuMusic != null)
        {
            AudioManager.instance.PlayMusic (menuMusic);
        }
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen
            .SetResolution(resolution.width,
            resolution.height,
            Screen.fullScreen);
    }

    public void NewGameYes()
    {
        if (
            System
                .IO
                .File
                .Exists(Application.persistentDataPath + "/savedGame.gd")
        )
        {
            File.Delete(Application.persistentDataPath + "/savedGame.gd");
        }
        GameObject.Find("GameManager").GetComponent<GameManager>().gameLevel =
            1;
        AudioManager.instance.StopMusic();
        SceneManager.LoadScene (_newGameScene);
    }

    public void LoadGameYes()
    {
        if (
            System
                .IO
                .File
                .Exists(Application.persistentDataPath + "/savedGame.gd")
        )
        {
            GameObject.Find("GameManager").GetComponent<GameManager>().Loaded =
                true;
            AudioManager.instance.StopMusic();
            levelToLoad = PlayerPrefs.GetString("SavedLevel");
            SceneManager.LoadScene(1);
        }
        else
        {
            noSavedGameDialog.SetActive(true);
            noSaveFoundOk.Select();
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
        volumeTextValue.text = volume.ToString("0.0");
    }

    public void VolumeApply()
    {
        PlayerPrefs.SetFloat("masterVolume", AudioListener.volume);
        StartCoroutine(ConfirmationBox());
    }

    public void SetControllerSens(float sens)
    {
        mainControllerSens = (int) sens;
        controllerSensTextValue.text = mainControllerSens.ToString("0");
    }

    public void GameplayApply()
    {
        if (invertYToggle.isOn)
        {
            PlayerPrefs.SetInt("invertY", 1);
        }
        else
        {
            PlayerPrefs.SetInt("invertY", 0);
        }
        PlayerPrefs.SetFloat("controllerSens", mainControllerSens);
        StartCoroutine(ConfirmationBox());
    }

    public void SetBrightness(float brightness)
    {
        _brightnessLevel = brightness;
        brightnessTextValue.text = brightness.ToString("0.0");
        RenderSettings.ambientIntensity = brightness;
    }

    public void SetQuality(int qualityIndex)
    {
        _qualityIndex = qualityIndex;
    }

    public void SetFullscreen(bool isFullscreen)
    {
        _isFullscreen = isFullscreen;
    }

    public void GraphicsApply()
    {
        PlayerPrefs.SetFloat("brightnessLevel", _brightnessLevel);

        PlayerPrefs.SetInt("qualityIndex", _qualityIndex);
        QualitySettings.SetQualityLevel (_qualityIndex);

        PlayerPrefs.SetInt("isFullscreen", _isFullscreen ? 1 : 0);
        Screen.fullScreen = _isFullscreen;

        StartCoroutine(ConfirmationBox());
    }

    public void ResetButton(string MenuType)
    {
        if (MenuType == "Audio")
        {
            AudioListener.volume = defaultVolume;
            volumeSlider.value = defaultVolume;
            volumeTextValue.text = defaultVolume.ToString("0.0");
            PlayerPrefs.SetFloat("masterVolume", defaultVolume);
            StartCoroutine(ConfirmationBox());
        }

        if (MenuType == "Gameplay")
        {
            mainControllerSens = defaultSens;
            controllerSensSlider.value = defaultSens;
            controllerSensTextValue.text = defaultSens.ToString("0");
            invertYToggle.isOn = false;
            PlayerPrefs.SetInt("invertY", 0);
            PlayerPrefs.SetFloat("controllerSens", defaultSens);
            StartCoroutine(ConfirmationBox());
        }
        if (MenuType == "Graphics")
        {
            _brightnessLevel = defaultBrightness;
            brightnessSlider.value = defaultBrightness;
            brightnessTextValue.text = defaultBrightness.ToString("0.0");
            PlayerPrefs.SetFloat("brightnessLevel", defaultBrightness);

            qualityDropdown.value = 1;
            QualitySettings.SetQualityLevel(1);

            fullscreenToggle.isOn = false;
            Screen.fullScreen = false;

            Resolution currentResolution = Screen.currentResolution;
            Screen
                .SetResolution(currentResolution.width,
                currentResolution.height,
                false);
            resolutionDropdown.value = resolutions.Length;
            GraphicsApply();

            StartCoroutine(ConfirmationBox());
        }
    }

    public IEnumerator ConfirmationBox()
    {
        confirmationPrompt.SetActive(true);
        yield return new WaitForSeconds(2);
        confirmationPrompt.SetActive(false);
    }
}
