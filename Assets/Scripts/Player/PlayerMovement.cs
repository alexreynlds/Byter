using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public ContactFilter2D movementFilter;

    public float collisionOffset = 0.01f;

    public Sprite[] bodySprites;

    public Sprite[] headSprites;

    private GameObject playerBodyObject;

    private SpriteRenderer playerBodySpriteRenderer;

    Vector2 movementInput;

    float moveSpeed;

    Rigidbody2D rb;

    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerBodyObject = GameObject.Find("PlayerBody");

        playerBodySpriteRenderer =
            playerBodyObject.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        moveSpeed = GetComponent<PlayerStats>().moveSpeed;
    }

    void FixedUpdate()
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

        if (movementInput.x > 0)
        {
            playerBodySpriteRenderer.sprite = bodySprites[2];
            playerBodySpriteRenderer.flipX = false;
        }
        else if (movementInput.x < 0)
        {
            playerBodySpriteRenderer.sprite = bodySprites[2];
            playerBodySpriteRenderer.flipX = true;
        }
        else if (movementInput.y > 0)
        {
            playerBodySpriteRenderer.sprite = bodySprites[1];
        }
        else if (movementInput.y < 0)
        {
            playerBodySpriteRenderer.sprite = bodySprites[0];
        }
        else
        {
            playerBodySpriteRenderer.sprite = bodySprites[0];
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
