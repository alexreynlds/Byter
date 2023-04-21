using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    // Main Stats
    public int maxHealth = 8;
    public int currentHealth;

    public float moveSpeed = 1f;

    // Inventory

    public List<string> inventory = new List<string>();

    public int coins;

    // Start is called before the first frame update
    void Awake()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        else if (currentHealth <= 0)
        {
            currentHealth = 0;
        }
    }

    public void wipeInventory()
    {
        inventory.Clear();
    }
}
