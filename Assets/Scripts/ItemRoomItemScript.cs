using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemRoomItemScript : MonoBehaviour
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
    private List<Spawnable> itemPool = new List<Spawnable>();

    private Spawnable chosenItem;
    private GameObject storedItem;
    private GameObject player;
    private bool itemTaken = false;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        totalWeight = 0;
        foreach (Spawnable item in itemPool)
        {
            totalWeight += item.weight;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        float pick = Random.Range(0, totalWeight);
        int chosenIndex = 0;
        float cumulativeWeight = itemPool[0].weight;

        while (pick > cumulativeWeight && chosenIndex < itemPool.Count - 1)
        {
            chosenIndex++;
            cumulativeWeight += itemPool[chosenIndex].weight;
        }

        chosenItem = itemPool[chosenIndex];
        storedItem = Instantiate(
            itemPool[chosenIndex].gameObject,
            new Vector2(transform.position.x, transform.position.y + 0.5f),
            Quaternion.identity
        );
        storedItem.GetComponent<BoxCollider2D>().enabled = false;
        storedItem.transform.parent = transform;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!itemTaken && other.CompareTag("Player"))
        {
            Debug.Log("Taken");
            storedItem.GetComponent<ItemPickupScript>().ItemPickup(player);
            itemTaken = true;
        }
        else
        {
            return;
        }
    }
}
