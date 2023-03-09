using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UserInterfaceScript : MonoBehaviour
{
    [Header("Menus")]
    [SerializeField]
    private GameObject pauseMenu;

    void OnPause()
    {
        // pauseMenu.SetActive(true);
        if (pauseMenu.active)
        {
            pauseMenu.SetActive(false);
        }
        else
        {
            pauseMenu.SetActive(true);
        }
    }
}
