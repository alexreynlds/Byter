using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public bool controllerConnected;

    public Button initialButton;

    public Button selectedButton;

    void Start()
    {
        if (controllerCheck())
        {
            controllerConnected = true;
            initialButton.Select();
        }
    }

    void Update()
    {
        if (controllerCheck())
        {
            if (EventSystem.current.currentSelectedGameObject != null)
            {
                selectedButton =
                    EventSystem
                        .current
                        .currentSelectedGameObject
                        .GetComponent<Button>();
            }
        }

        if (!controllerCheck() && !controllerConnected)
        {
            EventSystem.current.SetSelectedGameObject(null);
        }

        if (controllerCheck() && !controllerConnected)
        {
            if (selectedButton != null)
            {
                selectedButton.Select();
            }
            else
            {
                initialButton.Select();
            }
            controllerConnected = true;
        }

        if (!controllerCheck() && controllerConnected)
        {
            EventSystem.current.SetSelectedGameObject(null);
            controllerConnected = false;
        }
    }

    bool controllerCheck()
    {
        if (Input.GetJoystickNames().Length > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
