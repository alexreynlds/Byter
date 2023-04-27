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

    [SerializeField] private List<ShopItem> shopItems = new List<ShopItem>();

    [SerializeField] private GameObject priceText;

    void Awake()
    {
        totalWeight = 0;
        foreach (ShopItem shopItem in shopItems)
        {
            totalWeight += shopItem.weight;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        float pick = Random.Range(0, totalWeight);
        int chosenIndex = 0;
        float cumulativeWeight = shopItems[0].weight;

        while (pick > cumulativeWeight && chosenIndex < shopItems.Count - 1)
        {
            chosenIndex++;
            cumulativeWeight += shopItems[chosenIndex].weight;
        }

        var item = Instantiate(shopItems[chosenIndex].gameObject, new Vector2(transform.position.x, transform.position.y + 0.5f), Quaternion.identity);
        item.transform.parent = transform;
        priceText.GetComponent<TextMeshPro>().text = shopItems[chosenIndex].price.ToString();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Triggered");
    }
}
