using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadPrefs : MonoBehaviour
{
    [Header("General Setting")]
    [SerializeField]
    private bool canUse = false;

    [SerializeField]
    private MenuController menuController;

    [Header("Volume Setting")]
    [SerializeField]
    private TMP_Text volumeTextValue = null;

    [SerializeField]
    private Slider volumeSlider = null;

    [Header("Brightness Setting")]
    [SerializeField]
    private Slider brightnessSlider = null;

    [SerializeField]
    private TMP_Text brightnessTextValue = null;

    [Header("Quality Setting")]
    [SerializeField]
    private TMP_Dropdown qualityDropdown;

    [Header("Fullscreen Setting")]
    [SerializeField]
    private Toggle fullscreenToggle;

    [Header("Sens Setting")]
    [SerializeField]
    private TMP_Text controllerSensTextValue = null;

    [SerializeField]
    private Slider controllerSensSlider = null;

    [Header("Invert Setting")]
    [SerializeField]
    private Toggle invertYToggle = null;

    private void Awake()
    {
        if (canUse)
        {
            if (PlayerPrefs.HasKey("Volume"))
            {
                volumeSlider.value = PlayerPrefs.GetFloat("Volume");
                volumeTextValue.text = volumeSlider.value.ToString("0.00");
                AudioListener.volume = volumeSlider.value;
            }
            else
            {
                menuController.ResetButton("Audio");
            }

            if (PlayerPrefs.HasKey("Brightness"))
            {
                brightnessSlider.value = PlayerPrefs.GetFloat("Brightness");
                brightnessTextValue.text =
                    brightnessSlider.value.ToString("0.00");
                RenderSettings.ambientIntensity = brightnessSlider.value;
            }
            else
            {
                menuController.ResetButton("Graphics");
            }

            if (PlayerPrefs.HasKey("Quality"))
            {
                qualityDropdown.value = PlayerPrefs.GetInt("Quality");
                QualitySettings.SetQualityLevel(qualityDropdown.value);
            }
            else
            {
                menuController.ResetButton("Graphics");
            }

            if (PlayerPrefs.HasKey("Fullscreen"))
            {
                fullscreenToggle.isOn = PlayerPrefs.GetInt("Fullscreen") == 1;
                Screen.fullScreen = fullscreenToggle.isOn;
            }
            else
            {
                menuController.ResetButton("Graphics");
            }

            if (PlayerPrefs.HasKey("Sens"))
            {
                controllerSensSlider.value = PlayerPrefs.GetInt("Sens");
                controllerSensTextValue.text =
                    controllerSensSlider.value.ToString();
                menuController.mainControllerSens =
                    (int) controllerSensSlider.value;
            }
            else
            {
                menuController.ResetButton("Gameplay");
            }
            if (PlayerPrefs.HasKey("Invert"))
            {
                invertYToggle.isOn = PlayerPrefs.GetInt("Invert") == 1;
            }
            else
            {
                menuController.ResetButton("Gameplay");
            }
        }
    }
}
