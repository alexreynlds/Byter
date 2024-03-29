using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class UIScript : MonoBehaviour
{
    private GameObject player;

    [SerializeField]
    private GameObject inGameUI;

    [SerializeField]
    private GameObject pauseMenu;

    private bool isPaused = true;

    // private bool isPaused = false;

    [SerializeField]
    private AudioSource clickSound;

    [Header("Options Menu")]
    [SerializeField]
    private AudioMixer audioMixer;

    [SerializeField]
    private Slider musicVolumeSlider;

    [SerializeField]
    private Slider sfxVolumeSlider;

    [SerializeField]
    private Dropdown resolutionDropdown;

    [SerializeField]
    private Dropdown graphicsDropdown;

    [SerializeField]
    private Toggle fullscreenToggle;

    private Resolution[] resolutions;
    private int currentResolutionIndex = 0;

    public GameObject deathScreen;

    public GameObject winScreen;

    public GameObject introCard;

    public AudioSource musicSource;

    public AudioClip exploringMusic;

    public AudioClip finalMusic;

    public Toggle extraLivesToggle;

    void Awake()
    {
        player = GameObject.Find("Player");
        Application.targetFrameRate = -1;
        musicSource.clip = exploringMusic;
        musicSource.Play();
        Time.timeScale = 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option =
                resolutions[i].width
                + " x "
                + resolutions[i].height
                + " @ "
                + resolutions[i].refreshRate
                + "hz";
            options.Add(option);

            if (
                resolutions[i].width == Screen.currentResolution.width
                && resolutions[i].height == Screen.currentResolution.height
            )
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);

        if (!PlayerPrefs.HasKey("Resolution"))
        {
            PlayerPrefs.SetInt("Resolution", currentResolutionIndex);
            resolutionDropdown.value = currentResolutionIndex;
            resolutionDropdown.RefreshShownValue();
        }
        else
        {
            currentResolutionIndex = PlayerPrefs.GetInt("Resolution");
            resolutionDropdown.value = currentResolutionIndex;
            resolutionDropdown.RefreshShownValue();
        }

        if (!PlayerPrefs.HasKey("Fullscreen"))
        {
            PlayerPrefs.SetInt("Fullscreen", 1);
            SetFullscreen(true);
            fullscreenToggle.isOn = true;
        }
        else
        {
            SetFullscreen(PlayerPrefs.GetInt("Fullscreen") == 1 ? true : false);
            fullscreenToggle.isOn = PlayerPrefs.GetInt("Fullscreen") == 1 ? true : false;
        }

        if (!PlayerPrefs.HasKey("Quality"))
        {
            PlayerPrefs.SetInt("Quality", 3);
            graphicsDropdown.value = 3;
            graphicsDropdown.RefreshShownValue();
            SetQuality(3);
        }
        else
        {
            graphicsDropdown.value = PlayerPrefs.GetInt("Quality");
            graphicsDropdown.RefreshShownValue();
            SetQuality(PlayerPrefs.GetInt("Quality"));
        }

        if (PlayerPrefs.HasKey("SFXVol"))
        {
            SetSFXVolume(PlayerPrefs.GetFloat("SFXVol"));
            sfxVolumeSlider.value = PlayerPrefs.GetFloat("SFXVol");
        }
        else
        {
            SetSFXVolume(-30);
            sfxVolumeSlider.value = -30;
        }
        if (PlayerPrefs.HasKey("musicVol"))
        {
            SetMusicVolume(PlayerPrefs.GetFloat("musicVol"));
            musicVolumeSlider.value = PlayerPrefs.GetFloat("musicVol");
        }
        else
        {
            SetMusicVolume(-30);
            musicVolumeSlider.value = -30;
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateElements();
    }

    public void Win()
    {
        winScreen.SetActive(true);
        inGameUI.SetActive(false);

        musicSource.clip = finalMusic;
        musicSource.Play();
        Time.timeScale = 0;
    }

    public void GameOver()
    {
        deathScreen.SetActive(true);
        inGameUI.SetActive(false);

        musicSource.clip = finalMusic;
        musicSource.Play();
        Time.timeScale = 0;
    }

    private void UpdateElements()
    {
        // Inventory Screen
        string inventoryString = "Inventory: ";
        foreach (string item in player.GetComponent<PlayerStats>().inventory)
        {
            inventoryString += item + ", ";
        }
        inventoryString = inventoryString.Substring(0, inventoryString.Length - 2);
        inGameUI.transform.Find("Inventory").GetComponent<Text>().text = inventoryString;

        // FPS Counter
        inGameUI.transform.Find("FPSCounter").GetComponent<Text>().text =
            "FPS: " + (1f / Time.unscaledDeltaTime).ToString("F0");

        // Coins
        // inGameUI.transform.Find("CoinsText").GetComponent<Text>().text = player.GetComponent<PlayerStats>().coins.ToString();
        inGameUI.transform
            .Find("CoinHolder")
            .transform.Find("CoinsText")
            .GetComponent<Text>()
            .text = player.GetComponent<PlayerStats>().coins.ToString();
        inGameUI.transform
            .Find("CoinHolder")
            .transform.Find("KeycardText")
            .GetComponent<Text>()
            .text = player.GetComponent<PlayerStats>().keycards.ToString();
    }

    public void OnPause()
    {
        if (!isPaused)
        {
            Time.timeScale = 0;
            pauseMenu.SetActive(true);
            // inGameUI.SetActive(false);
            isPaused = true;
        }
        else
        {
            pauseMenu.transform.Find("SoundTitle").gameObject.SetActive(false);
            pauseMenu.transform.Find("SoundMenuContainer").gameObject.SetActive(false);
            pauseMenu.transform.Find("OptionsTitle").gameObject.SetActive(false);
            pauseMenu.transform.Find("OptionsMenuContainer").gameObject.SetActive(false);
            pauseMenu.transform.Find("PauseMenuContainer").gameObject.SetActive(true);
            pauseMenu.SetActive(false);
            // inGameUI.SetActive(true);
            Time.timeScale = 1;
            isPaused = false;
        }
    }

    public void OnRestart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(1);
    }

    public void OnQuit()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    public void OnClick()
    {
        clickSound.Play();
    }

    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat("SFXVol", volume);
        PlayerPrefs.SetFloat("SFXVol", volume);
        // PlayerPrefs.Save();
    }

    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("musicVol", volume);
        PlayerPrefs.SetFloat("musicVol", volume);
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
        PlayerPrefs.SetInt("Quality", qualityIndex);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        PlayerPrefs.SetInt("Fullscreen", isFullscreen ? 1 : 0);
    }

    public void SetResolution(int resIndex)
    {
        Resolution resolution = resolutions[resIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        PlayerPrefs.SetInt("Resolution", resIndex);
    }

    public void ResetAudio()
    {
        Debug.Log("Resetting Audio");
        musicVolumeSlider.value = -30;
        sfxVolumeSlider.value = -30;
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

    public void chosenFixedDiff()
    {
        GameObject.Find("RoomController").GetComponent<DDASystem>().enabled = false;
        if (extraLivesToggle.isOn)
        {
            GameObject.Find("Player").GetComponent<PlayerStats>().maxHealth = 10;
            GameObject.Find("Player").GetComponent<PlayerStats>().currentHealth = 10;
            GameObject.Find("Player").GetComponent<PlayerStats>().attackDamage = 1.5f;
        }
        OnPause();
    }

    public void chosenDynamicDiff()
    {
        if (extraLivesToggle.isOn)
        {
            GameObject.Find("Player").GetComponent<PlayerStats>().maxHealth = 10;
            GameObject.Find("Player").GetComponent<PlayerStats>().currentHealth = 10;
            GameObject.Find("Player").GetComponent<PlayerStats>().attackDamage = 1.5f;
        }
        OnPause();
    }
}
