using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public ContactFilter2D movementFilter;

    public float collisionOffset = 0.01f;

    public Sprite[] bodySprites;

    public Sprite[] bottomSprites;

    private GameObject playerBodyObject;

    private SpriteRenderer playerBodySpriteRenderer;

    private GameObject playerBottomObject;

    private SpriteRenderer playerBottomSpriteRenderer;

    Vector2 movementInput;

    float moveSpeed;

    Rigidbody2D rb;

    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();

    private SpriteRenderer spriteRenderer;

    private int bodySpriteCounter = 0;

    private int bodySpriteInt = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerBodyObject = GameObject.Find("PlayerBody");

        playerBodySpriteRenderer =
            playerBodyObject.GetComponent<SpriteRenderer>();
        playerBodySpriteRenderer.sprite = bodySprites[bodySpriteInt];

        playerBottomObject = GameObject.Find("PlayerBottom");

        playerBottomSpriteRenderer =
            playerBottomObject.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (movementInput != Vector2.zero)
        {
            bool success = TryMove(movementInput);

            if (!success)
            {
                success = TryMove(new Vector2(movementInput.x, 0));

                if (!success)
                    success = TryMove(new Vector2(0, movementInput.y));
            }
        }

        if (movementInput == Vector2.zero)
        {
            playerBodySpriteRenderer.sprite = bodySprites[0];
        }

        // Debug.Log(movementInput);
    }

    void FixedUpdate()
    {
        moveSpeed = GetComponent<PlayerStats>().moveSpeed;

        // Every 10 frames cycle through the body sprite
        if (bodySpriteCounter == 5)
        {
            if (bodySpriteInt == 2)
            {
                bodySpriteInt = 0;
            }
            else
            {
                bodySpriteInt++;
            }
            bodySpriteCounter = 0;
            playerBottomSpriteRenderer.sprite = bottomSprites[bodySpriteInt];
        }
        else
        {
            bodySpriteCounter++;
        }
    }

    private bool TryMove(Vector2 direction)
    {
        int count =
            rb
                .Cast(direction,
                movementFilter,
                castCollisions,
                moveSpeed * Time.fixedDeltaTime + collisionOffset);

        if (count == 0)
        {
            rb
                .MovePosition(rb.position +
                direction * moveSpeed * Time.fixedDeltaTime);
            // Change body sprite depending on movement direction.

            if (direction == Vector2.up){
                playerBodySpriteRenderer.sprite = bodySprites[2];
            }
            else if (direction == Vector2.down){
                playerBodySpriteRenderer.sprite = bodySprites[0];

            }
            else if (direction == Vector2.left){
                playerBodySpriteRenderer.sprite = bodySprites[1];
                playerBodySpriteRenderer.flipX = false;
            }
            else if (direction == Vector2.right){
                playerBodySpriteRenderer.sprite = bodySprites[1];
                playerBodySpriteRenderer.flipX = true;
            }
            return true;
        }
        else
        {
            return false;
        }
    }

    void OnMove(InputValue movementVal)
    {
        movementInput = movementVal.Get<Vector2>();
    }
}
