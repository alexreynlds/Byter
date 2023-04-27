using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickupScript : MonoBehaviour
{
    [SerializeField] private enum BasicItemType { None, Health, Energy, Coin };
    [SerializeField] private BasicItemType itemType;

    [SerializeField] private int changeAmount = 0;
    [SerializeField] private string itemName;
    [SerializeField] private string itemDescription;

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Player"))
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

                if (tempName == "MHealthUp" || tempName == "MHealthDown")
                {
                    if (changeAmount < 0)
                    {
                        // If max health is above 4, decrease it, 4 is the least it can be
                        if (other.GetComponent<PlayerStats>().maxHealth > 4)
                        {
                            FindObjectOfType<PopupWindowScript>().AddToQueue(itemName, "Decreased Max Health!");
                            other.GetComponent<PlayerStats>().maxHealth += changeAmount;
                        }
                        else return;
                    }
                    // Increase the player's max health
                    else if (changeAmount > 0)
                    {
                        FindObjectOfType<PopupWindowScript>().AddToQueue(itemName, "Increased Max Health!");
                        other.GetComponent<PlayerStats>().maxHealth += changeAmount;
                    }
                    else return;
                }
            }


            // string itemName = gameObject.name;
            // // itemName = itemName.Substring(0, itemName.Length - 7);
            // if (itemType == ItemType.Coin)
            // {
            //     if (other.GetComponent<PlayerStats>().coins < 100)
            //     {
            //         other.GetComponent<PlayerStats>().coins += 1;
            //     }
            //     else
            //     {
            //         return;
            //     }
            // }
            // else if (itemType == ItemType.Health)
            // {
            //     other.GetComponent<PlayerStats>().currentHealth += changeAmount;
            // }
            // else if (itemType == ItemType.MaxHealth)
            // {
            //     if (changeAmount < 0)
            //     {
            //         // If max health is above 4, decrease it, 4 is the least it can be
            //         if (other.GetComponent<PlayerStats>().maxHealth > 4)
            //         {
            //             FindObjectOfType<PopupWindowScript>().AddToQueue(itemName, "Decreased Max Health!");
            //             other.GetComponent<PlayerStats>().maxHealth += changeAmount;
            //         }
            //         else return;
            //     }
            //     // Increase the player's max health
            //     else if (changeAmount > 0)
            //     {
            //         FindObjectOfType<PopupWindowScript>().AddToQueue(itemName, "Increased Max Health!");
            //         other.GetComponent<PlayerStats>().maxHealth += changeAmount;
            //     }
            //     else return;
            // }
            // else if (itemType == ItemType.Energy)
            // {
            //     other.GetComponent<PlayerStats>().currentEnergy += changeAmount;
            // }
            // else
            // {
            //     other.GetComponent<PlayerStats>().inventory.Add(itemName);
            // }

            Destroy(gameObject);
        }
    }
}
