using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooting : MonoBehaviour
{
    [Header("Player Head")]
    private GameObject playerHeadObject;

    private SpriteRenderer playerHeadSpriteRenderer;

    public Sprite[] headSprites;

    private Vector2 shootInput;

    private string shootDir;

    void Start()
    {
        playerHeadObject = GameObject.Find("PlayerHead");

        playerHeadSpriteRenderer =
            playerHeadObject.GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        // Updating Shooting direction
        if (shootInput != Vector2.zero)
        {
            if (shootInput.x > 0)
            {
                shootDir = "right";
            }
            else if (shootInput.x < 0)
            {
                shootDir = "left";
            }
            else if (shootInput.y > 0)
            {
                shootDir = "up";
            }
            else if (shootInput.y < 0)
            {
                shootDir = "down";
            }
        }
        if (shootInput == null)
        {
            shootDir = "none";
        }

        // Updating Player Head Sprite based on shooting direction
        if (shootDir == "right")
        {
            playerHeadSpriteRenderer.sprite = headSprites[3];
        }
        else if (shootDir == "left")
        {
            playerHeadSpriteRenderer.sprite = headSprites[2];
        }
        else if (shootDir == "up")
        {
            playerHeadSpriteRenderer.sprite = headSprites[1];
        }
        else if (shootDir == "down")
        {
            playerHeadSpriteRenderer.sprite = headSprites[0];
        }
        else if (shootDir == "none")
        {
            playerHeadSpriteRenderer.sprite = headSprites[0];
        }

        Debug.Log (shootDir);
    }

    void OnShoot(InputValue shootVal)
    {
        shootInput = shootVal.Get<Vector2>();
    }
}
