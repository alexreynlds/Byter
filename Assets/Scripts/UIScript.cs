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
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        UpdateInventory();
    }

    void UpdateInventory()
    {
        string inventoryString = "Inventory: ";
        foreach (string item in player.GetComponent<PlayerStats>().inventory)
        {
            inventoryString += item + ", ";
        }
        inventoryString =
            inventoryString.Substring(0, inventoryString.Length - 2);
        transform.Find("Inventory").GetComponent<TextMeshProUGUI>().text =
            inventoryString;
    }
}
