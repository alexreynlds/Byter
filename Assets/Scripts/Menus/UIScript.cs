using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIScript : MonoBehaviour
{
    private GameObject player;

    void Awake()
    {
        player = GameObject.Find("Player");
        Application.targetFrameRate = -1;
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        UpdateElements();
    }

    private void UpdateElements()
    {
        // Inventory Screen
        string inventoryString = "Inventory: ";
        foreach (string item in player.GetComponent<PlayerStats>().inventory)
        {
            inventoryString += item + ", ";
        }
        inventoryString =
            inventoryString.Substring(0, inventoryString.Length - 2);
        transform.Find("Inventory").GetComponent<TextMeshProUGUI>().text =
            inventoryString;

        // FPS Counter
        transform.Find("FPSCounter").GetComponent<TextMeshProUGUI>().text =
            "FPS: " + (1f / Time.unscaledDeltaTime).ToString("F0");

        // Coins
        transform.Find("Coins").GetComponent<TextMeshProUGUI>().text =
            "Coins: " + player.GetComponent<PlayerStats>().coins.ToString();
    }
}
