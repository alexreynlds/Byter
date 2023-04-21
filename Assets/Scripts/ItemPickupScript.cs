using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickupScript : MonoBehaviour
{
    [SerializeField] private enum ItemType { Health, MaxHealth, Energy, Coin };
    [SerializeField] private ItemType itemType;
    [SerializeField] private int changeAmount = 0;
    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Player"))
        {
            string itemName = gameObject.name;
            itemName = itemName.Substring(0, itemName.Length - 7);

            if (itemType == ItemType.Coin)
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
            else if (itemType == ItemType.Health)
            {
                other.GetComponent<PlayerStats>().currentHealth += changeAmount;
            }
            else if (itemType == ItemType.MaxHealth)
            {
                if (changeAmount < 0)
                {
                    if (other.GetComponent<PlayerStats>().maxHealth > 4)
                    {
                        other.GetComponent<PlayerStats>().maxHealth += changeAmount;
                    }
                    else return;
                }
                else if (changeAmount > 0)
                {
                    other.GetComponent<PlayerStats>().maxHealth += changeAmount;
                }
                else return;
            }
            else if (itemType == ItemType.Energy)
            {
                other.GetComponent<PlayerStats>().currentEnergy += changeAmount;
            }
            else
            {
                other.GetComponent<PlayerStats>().inventory.Add(itemName);
            }

            Destroy(gameObject);
        }
    }
}
