using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuScript : MonoBehaviour
{
    [SerializeField] private GameObject statsScreen;
    [SerializeField] private GameObject showIcon;
    [SerializeField] private GameObject hideIcon;

    public void ToggleStatsScreen()
    {
        statsScreen.SetActive(!statsScreen.activeSelf);
        showIcon.SetActive(!showIcon.activeSelf);
        hideIcon.SetActive(!hideIcon.activeSelf);
    }
}
