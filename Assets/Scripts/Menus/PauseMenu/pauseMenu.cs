using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class pauseMenu : MonoBehaviour
{
    public GameObject MainMenus;

    private PlayerInput playerInput;

    public GameObject mainMenuContainer;

    // Start is called before the first frame update
    void Start()
    {
        playerInput = GameObject.Find("Player").GetComponent<PlayerInput>();
        this.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void PauseGameAnim()
    {
        Time.timeScale = 0;
        this.gameObject.SetActive(true);
        mainMenuContainer.GetComponent<PlayerInput>().DeactivateInput();

        playerInput.DeactivateInput();
        MainMenus.GetComponent<Animator>().Play("PauseMenuOpen");
    }

    public void PauseGame()
    {
        mainMenuContainer.GetComponent<PlayerInput>().ActivateInput();
    }

    public void ResumeGameAnim()
    {
        MainMenus.GetComponent<Animator>().Play("PauseMenuClose");
    }

    public void ResumeGame()
    {
        playerInput.ActivateInput();
        this.gameObject.SetActive(false);
        Time.timeScale = 1;
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }
}
