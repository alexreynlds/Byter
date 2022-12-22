using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuAnimationScript : MonoBehaviour
{
    GameObject pauseMenu;

    // Start is called before the first frame update
    void Start()
    {
        pauseMenu = GameObject.Find("PauseMenu");
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void ResumeGame()
    {
        pauseMenu.GetComponent<pauseMenu>().ResumeGame();
    }

    public void PauseGame()
    {
        pauseMenu.GetComponent<pauseMenu>().PauseGame();
    }
}
