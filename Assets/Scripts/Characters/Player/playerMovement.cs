using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class playerMovement : MonoBehaviour
{
    SpriteRenderer spriteRenderer;

    Rigidbody2D rb;

    [Header("Player Movement")]
    public float moveSpeed = 1f;

    public float collisionOffset = 0.05f;

    public ContactFilter2D movementFilter;

    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();

    private Vector2 movementInput;

    public Sprite[] playerSprites;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (
            movementInput != Vector2.zero &&
            gameObject.GetComponent<PlayerController>().canMove
        )
        {
            bool success = TryMove(movementInput);
            if (!success)
            {
                success = TryMove(new Vector2(movementInput.x, 0));

                if (!success)
                {
                    success = TryMove(new Vector2(0, movementInput.y));
                }
            }
        }

        if (movementInput.x < 0)
        {
            spriteRenderer.flipX = true;
            spriteRenderer.sprite = playerSprites[1];
        }
        else if (movementInput.x > 0)
        {
            spriteRenderer.flipX = false;
            spriteRenderer.sprite = playerSprites[1];
        }
        else if (movementInput.y > 0)
        {
            spriteRenderer.sprite = playerSprites[2];
        }
        else if (movementInput == Vector2.zero)
        {
            spriteRenderer.sprite = playerSprites[0];
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

    void OnMove(InputValue movementValue)
    {
        movementInput = movementValue.Get<Vector2>();
    }
}
