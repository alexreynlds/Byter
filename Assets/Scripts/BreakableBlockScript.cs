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

    private SpriteRenderer spriteRenderer;
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
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {

        if (other.gameObject.CompareTag("Bullet"))
        {
            health--;
            if (health <= 0)
            {
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
}