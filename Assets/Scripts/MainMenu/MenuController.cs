using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [Header("Levels to load")]
    public string _newGameScene;

    private string levelToLoad;

    [SerializeField]
    private GameObject noSavedGameDialog = null;

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

    public void Test()
    {
        Debug.Log("test");
    }
}
