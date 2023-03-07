using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public ContactFilter2D movementFilter;

    public float collisionOffset = 0.01f;

    public Sprite[] bodySprites;

    private GameObject playerBodyObject;

    private SpriteRenderer playerBodySpriteRenderer;

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
    }

    void FixedUpdate()
    {
        moveSpeed = GetComponent<PlayerStats>().moveSpeed;

        if (bodySpriteCounter == 10)
        {
            if (bodySpriteInt == 3)
            {
                bodySpriteInt = 0;
            }
            else
            {
                bodySpriteInt++;
            }
            bodySpriteCounter = 0;
            playerBodySpriteRenderer.sprite = bodySprites[bodySpriteInt];
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
