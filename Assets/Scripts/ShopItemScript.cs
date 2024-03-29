using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShopItemScript : MonoBehaviour
{
    [System.Serializable]
    struct ShopItem
    {
        public GameObject gameObject;

        public float weight;

        public int price;
    }

    float totalWeight;

    [SerializeField]
    private List<ShopItem> shopItems = new List<ShopItem>();

    [SerializeField]
    private GameObject priceText;

    private ShopItem chosenItem;
    private GameObject storedItem;
    private GameObject player;
    private bool bought = false;
    private int chosenIndex = 0;
    private float previousDifficulty;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        totalWeight = 0;
        previousDifficulty = DDASystem.instance.currentDifficulty;
        foreach (ShopItem shopItem in shopItems)
        {
            totalWeight += shopItem.weight;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        float pick = Random.Range(0, totalWeight);
        float cumulativeWeight = shopItems[0].weight;

        while (pick > cumulativeWeight && chosenIndex < shopItems.Count - 1)
        {
            chosenIndex++;
            cumulativeWeight += shopItems[chosenIndex].weight;
        }

        chosenItem = shopItems[chosenIndex];
        storedItem = Instantiate(
            shopItems[chosenIndex].gameObject,
            new Vector2(transform.position.x, transform.position.y + 0.5f),
            Quaternion.identity
        );
        storedItem.GetComponent<BoxCollider2D>().enabled = false;
        storedItem.transform.parent = transform;

        priceText.GetComponent<TextMeshPro>().text = shopItems[chosenIndex].price.ToString();
    }

    void Update()
    {
        if (DDASystem.instance)
        {
            if (previousDifficulty != DDASystem.instance.currentDifficulty)
            {
                previousDifficulty = DDASystem.instance.currentDifficulty;
                priceText.GetComponent<TextMeshPro>().text = Mathf.Ceil(shopItems[chosenIndex].price * DDASystem.instance.currentDifficulty).ToString();
            }
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Triggered");
        if (!bought && other.CompareTag("Player"))
        {
            if (other.GetComponent<PlayerStats>().coins >= chosenItem.price)
            {
                Debug.Log("Bought");
                other.GetComponent<PlayerStats>().coins -= (int)Mathf.Ceil(chosenItem.price * DDASystem.instance.currentDifficulty);
                storedItem.GetComponent<ItemPickupScript>().ItemPickup(player);
                priceText.SetActive(false);
            }
            else
            {
                Debug.Log("Not enough coins");
            }
        }
        else
        {
            return;
        }
    }
}
