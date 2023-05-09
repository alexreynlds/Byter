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
    public int keycards;
    public bool bossKeycard = false;

    private bool canTakeDamage = true;

    public bool isSuper = false;
    private float normalDamage;
    private float normalAttackRange;

    private bool hasEnded = false;

    // Start is called before the first frame update
    void Awake()
    {
        currentHealth = maxHealth;
        currentEnergy = maxEnergy;
        coins = 0;
        keycards = 0;
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

        if (currentHealth == 0)
        {
            if (!hasEnded)
            {
                hasEnded = true;
                GameObject.Find("UICanvas").GetComponent<UIScript>().GameOver();
            }

        }
    }

    public void StartSuper()
    {
        normalAttackRange = attackRange;
        normalDamage = attackDamage;
        attackDamage = attackDamage * 1.5f;
        attackRange = attackRange * 1.5f;
        isSuper = true;
        currentEnergy = 0;
        foreach (Transform child in transform)
        {
            child.gameObject.GetComponent<SpriteRenderer>().color = Color.green;
        }
        GameObject.Find("RoomController").GetComponent<DDASystem>().timesSupered++;
        Invoke("StopSuper", 5f);
    }

    public void StopSuper()
    {
        attackRange = normalAttackRange;
        attackDamage = normalDamage;
        isSuper = false;
        foreach (Transform child in transform)
        {
            child.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        }
    }

    public void wipeInventory()
    {
        inventory.Clear();
        coins = 0;
        keycards = 0;
    }

    public void TakeDamage(float damage)
    {
        if (canTakeDamage)
        {
            currentHealth -= (int)damage;

            foreach (Transform child in transform)
            {
                child.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
            }

            Invoke("ResetColor", 0.1f);
            GetComponent<PlayerAudioManager>().TakeDamageSound();
            StartCoroutine(InvincibilityFrames());
            GameObject.Find("RoomController").GetComponent<DDASystem>().damageTaken += damage;
        }
    }

    IEnumerator InvincibilityFrames()
    {
        canTakeDamage = false;
        yield return new WaitForSeconds(0.5f);
        canTakeDamage = true;
    }

    public void ResetColor()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        }
    }
}
