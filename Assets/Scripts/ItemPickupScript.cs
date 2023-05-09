using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickupScript : MonoBehaviour
{
    [SerializeField]
    private enum BasicItemType
    {
        None,
        Health,
        Energy,
        Coin,
        Keycard,
        bossKeycard
    };

    [SerializeField]
    private BasicItemType itemType;

    [SerializeField]
    private int changeAmount = 0;

    [SerializeField]
    private string itemName;

    [SerializeField]
    private string itemDescription;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            ItemPickup(other.gameObject);
        }
    }

    public void ItemPickup(GameObject other)
    {
        if (itemType != BasicItemType.None)
        {
            if (itemType == BasicItemType.Coin)
            {
                if (other.GetComponent<PlayerStats>().coins < 100)
                {
                    other.GetComponent<PlayerStats>().coins += 1;
                }
                else
                {
                    return;
                }
            }
            else if (itemType == BasicItemType.Keycard)
            {
                if (other.GetComponent<PlayerStats>().keycards < 10)
                {
                    other.GetComponent<PlayerStats>().keycards += 1;
                }
                else
                {
                    return;
                }
            }
            else if (itemType == BasicItemType.bossKeycard)
            {
                other.GetComponent<PlayerStats>().bossKeycard = true;
                FindObjectOfType<PopupWindowScript>()
                            .AddToQueue("Boss Keycard", "You can now enter the boss room!");
            }
            else if (itemType == BasicItemType.Health)
            {
                other.GetComponent<PlayerStats>().currentHealth += changeAmount;
            }
            else if (itemType == BasicItemType.Energy)
            {
                other.GetComponent<PlayerStats>().currentEnergy += changeAmount;
            }
        }
        else
        {
            string tempName = gameObject.name;

            if (tempName.Contains("MHealth"))
            {
                if (changeAmount < 0)
                {
                    // If max health is above 4, decrease it, 4 is the least it can be
                    if (other.GetComponent<PlayerStats>().maxHealth > 4)
                    {
                        FindObjectOfType<PopupWindowScript>()
                            .AddToQueue(itemName, "Decreased Max Health!");
                        other.GetComponent<PlayerStats>().maxHealth += changeAmount;
                    }
                    else
                        Destroy(gameObject);

                }
                // Increase the player's max health
                else if (changeAmount > 0)
                {
                    FindObjectOfType<PopupWindowScript>()
                        .AddToQueue(itemName, "Increased Max Health!");
                    other.GetComponent<PlayerStats>().maxHealth += changeAmount;
                }
                else
                    return;
            }
            else if (tempName.Contains("RangeUp"))
            {
                other.GetComponent<PlayerStats>().attackRange += 0.3f;
                FindObjectOfType<PopupWindowScript>()
                        .AddToQueue(itemName, itemDescription);
            }
            else if (tempName.Contains("DamageUp"))
            {
                other.GetComponent<PlayerStats>().attackDamage += 0.25f;
                FindObjectOfType<PopupWindowScript>()
                        .AddToQueue(itemName, itemDescription);
            }
            else if (tempName.Contains("AtkSpeedUp"))
            {
                other.GetComponent<PlayerStats>().attackSpeed -= 0.2f;
                FindObjectOfType<PopupWindowScript>()
                        .AddToQueue(itemName, itemDescription);
            }
            else if (tempName.Contains("ProjSpeedUp"))
            {
                other.GetComponent<PlayerStats>().projectileSpeed += 1f;
                FindObjectOfType<PopupWindowScript>()
                        .AddToQueue(itemName, itemDescription);
            }
            else if (tempName.Contains("MoveSpeedUp"))
            {
                other.GetComponent<PlayerStats>().moveSpeed += 0.5f;
                FindObjectOfType<PopupWindowScript>()
                        .AddToQueue(itemName, itemDescription);
            }
        }
        other.GetComponent<PlayerAudioManager>().ItemPickupSound();
        Destroy(gameObject);
    }
}
