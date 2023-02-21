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

    void Start()
    {
        playerHeadObject = GameObject.Find("PlayerHead");

        playerHeadSpriteRenderer =
            playerHeadObject.GetComponent<SpriteRenderer>();
    }

    void OnShoot(InputValue shootVal)
    {
        Vector2 shootValue = shootVal.Get<Vector2>();
        Debug.Log (shootValue);
    }
}
