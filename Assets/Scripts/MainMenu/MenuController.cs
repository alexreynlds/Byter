using System.Collections;
using System.Collections.Generic;
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

    [Header("Confirmation Box")]
    [SerializeField]
    private GameObject confirmationPrompt = null;

    [Header("Levels to load")]
    public string _newGameScene;

    private string levelToLoad;

    [SerializeField]
    private GameObject noSavedGameDialog = null;

    void Start()
    {
        if (PlayerPrefs.HasKey("masterVolume"))
        {
            AudioListener.volume = PlayerPrefs.GetFloat("masterVolume");
            volumeSlider.value = AudioListener.volume;
            volumeTextValue.text = AudioListener.volume.ToString("0.0");
        }
        else
        {
            AudioListener.volume = defaultVolume;
            volumeSlider.value = defaultVolume;
            volumeTextValue.text = defaultVolume.ToString("0.0");
        }

        if (PlayerPrefs.HasKey("controllerSens"))
        {
            mainControllerSens = (int) PlayerPrefs.GetFloat("controllerSens");
            controllerSensSlider.value = mainControllerSens;
            controllerSensTextValue.text = mainControllerSens.ToString("0");
        }
        else
        {
            mainControllerSens = defaultSens;
            controllerSensSlider.value = defaultSens;
            controllerSensTextValue.text = defaultSens.ToString("0");
        }

        if (PlayerPrefs.HasKey("invertY"))
        {
            if (PlayerPrefs.GetInt("invertY") == 1)
            {
                invertYToggle.isOn = true;
            }
            else
            {
                invertYToggle.isOn = false;
            }
        }
        else
        {
            invertYToggle.isOn = false;
        }
    }

    public void NewGameYes()
    {
        SceneManager.LoadScene (_newGameScene);
    }

    public void LoadGameYes()
    {
        if (PlayerPrefs.HasKey("SavedLevel"))
        {
            levelToLoad = PlayerPrefs.GetString("SavedLevel");
            SceneManager.LoadScene (levelToLoad);
        }
        else
        {
            noSavedGameDialog.SetActive(true);
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
    }

    public IEnumerator ConfirmationBox()
    {
        confirmationPrompt.SetActive(true);
        yield return new WaitForSeconds(2);
        confirmationPrompt.SetActive(false);
    }
}
