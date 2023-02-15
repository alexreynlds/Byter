using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void LoadGame()
    {
    }

    public void OpenOptions()
    {
    }

    public void CloseOptions()
    {
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
