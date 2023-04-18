using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickupScript : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            string itemName = gameObject.name;
            itemName = itemName.Substring(0, itemName.Length - 7);

            if (itemName == "Coin")
            {
                if(other.GetComponent<PlayerStats>().coins < 100){
                    other.GetComponent<PlayerStats>().coins += 1;
                }
                else{
                    return;
                }
            }
            else
            {
                other.GetComponent<PlayerStats>().inventory.Add(itemName);
            }

            Destroy (gameObject);
        }
    }
}
