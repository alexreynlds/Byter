using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    // Adjustable stats
    public int maxHealth = 8;
    public int maxEnergy = 4;
    public float moveSpeed = 1f;

    public float projectileSpeed = 1f;
    public float attackSpeed = 0.5f;
    public float attackDamage = 1f;
    // Range controls the amount of time before the bullet is destroyed
    // Lower float means lower range
    public float attackRange = 3f;
    public float attackKnockback = 1f;
    // Main Stats
    public int currentHealth;
    public int currentEnergy;

    // Inventory
    public List<string> inventory = new List<string>();

    public int coins;

    // Start is called before the first frame update
    void Awake()
    {
        currentHealth = maxHealth;
        currentEnergy = maxEnergy;
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
        if (currentEnergy > maxEnergy)
        {
            currentEnergy = maxEnergy;
        }
        else if (currentEnergy < 0)
        {
            currentEnergy = 0;
        }
    }

    public void wipeInventory()
    {
        inventory.Clear();
    }
}
