using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    [Header("Levels To Load")]
    public int newGameLevel;

    private string levelToLoad;

    [Header("Screens")]
    public GameObject mainMenuScreen;

    public GameObject optionsScreen;

    public GameObject newGameDialogueScreen;

    public GameObject loadGameDialogueScreen;

    public GameObject noSaveDataDialogueScreen;

    private GameObject currentMainScreen;

    public void ReturnToMain()
    {
        currentMainScreen.SetActive(false);
        mainMenuScreen.SetActive(true);
        currentMainScreen = null;
    }

    public void NewGameDialogue()
    {
        newGameDialogueScreen.SetActive(true);
        currentMainScreen = newGameDialogueScreen;
    }

    public void NewGameDialogueYes()
    {
        SceneManager.LoadScene (newGameLevel);
    }

    public void LoadGameDialogue()
    {
        if (PlayerPrefs.HasKey("SavedLevel"))
        {
            // Load Saved Game
        }
        else
        {
            noSaveDataDialogueScreen.SetActive(true);
            currentMainScreen = noSaveDataDialogueScreen;
        }
    }

    public void OpenOptions()
    {
        optionsScreen.SetActive(true);
        currentMainScreen = optionsScreen;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
