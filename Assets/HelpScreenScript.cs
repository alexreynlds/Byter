using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpScreenScript : MonoBehaviour
{
    public GameObject[] screens;
    public int currentScreen = 0;
    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject screen in screens)
        {
            screen.SetActive(false);
        }
        screens[0].SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void NextScreen()
    {
        if (currentScreen < screens.Length - 1)
        {
            screens[currentScreen].SetActive(false);
            currentScreen++;
            screens[currentScreen].SetActive(true);
        }
        else
        {
            screens[currentScreen].SetActive(false);
            currentScreen = 0;
            screens[currentScreen].SetActive(true);
        }
    }

    public void PreviousScreen()
    {
        if (currentScreen > 0)
        {
            screens[currentScreen].SetActive(false);
            currentScreen--;
            screens[currentScreen].SetActive(true);
        }
        else
        {
            screens[currentScreen].SetActive(false);
            currentScreen = screens.Length - 1;
            screens[currentScreen].SetActive(true);
        }
    }
}
