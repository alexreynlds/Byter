using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BreakableBlockScript : MonoBehaviour
{
    [SerializeField]
    private enum BreakableType
    {
        Normal,
        Item
    };

    [SerializeField] private int health = 4;
    [SerializeField] private Sprite[] purpleSprites;
    [SerializeField] private Sprite[] goldSprites;
    [SerializeField] private BreakableType blockType;

    [SerializeField] private SpriteRenderer spriteRenderer;
    float totalWeight;
    public ItemPoolData itemPool;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (blockType == BreakableType.Normal)
        {
            spriteRenderer.sprite = purpleSprites[health - 1];
        }
        else
        {
            spriteRenderer.sprite = goldSprites[health - 1];
            foreach (Spawnable spawnable in itemPool.itemPool)
            {
                totalWeight += spawnable.weight;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {

        if (other.gameObject.CompareTag("Bullet"))
        {
            health--;
            if (health <= 0)
            {
                if (blockType == BreakableType.Item)
                {
                    DropItem();
                }
                Destroy(gameObject);
            }
            else
            {
                if (blockType == BreakableType.Normal)
                {
                    GetComponent<SpriteRenderer>().sprite = purpleSprites[health - 1];
                }
                else
                {
                    GetComponent<SpriteRenderer>().sprite = goldSprites[health - 1];
                }
            }
        }
    }

    private void DropItem()
    {
        float pick = Random.Range(0, totalWeight);
        int chosenIndex = 0;
        float cumulativeWeight = itemPool.itemPool[0].weight;

        while (pick > cumulativeWeight && chosenIndex < itemPool.itemPool.Count - 1)
        {
            chosenIndex++;
            cumulativeWeight += itemPool.itemPool[chosenIndex].weight;
        }

        if (itemPool.itemPool[chosenIndex].gameObject != null)
        {
            RoomController.instance.spawnItem(itemPool.itemPool[chosenIndex].gameObject, transform.position);
        }
        else
        {
            return;
        }
    }
}