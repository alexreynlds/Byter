using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooting : MonoBehaviour
{
    Vector2 shootInput;
    [SerializeField] private Sprite[] headSprites;
    [SerializeField] private GameObject head;

    private void Update()
    {
        if (shootInput != Vector2.zero)
        {
            ChangeHeadSprite();
        }
        else
        {
            head.GetComponent<SpriteRenderer>().sprite = headSprites[0];
        }
    }
    private void OnFire(InputValue shootDir)
    {
        shootInput = shootDir.Get<Vector2>();
    }

    private void ChangeHeadSprite()
    {
        if (shootInput.x > 0)
        {
            head.GetComponent<SpriteRenderer>().sprite = headSprites[1];
            head.GetComponent<SpriteRenderer>().flipX = true;
        }
        else if (shootInput.x < 0)
        {
            head.GetComponent<SpriteRenderer>().sprite = headSprites[1];
            head.GetComponent<SpriteRenderer>().flipX = false;
        }
        else if (shootInput.y > 0)
        {
            head.GetComponent<SpriteRenderer>().sprite = headSprites[2];
            head.GetComponent<SpriteRenderer>().flipX = false;
        }
        else if (shootInput.y < 0)
        {
            head.GetComponent<SpriteRenderer>().sprite = headSprites[0];
            head.GetComponent<SpriteRenderer>().flipX = false;
        }
    }
}
