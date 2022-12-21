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
            int count =
                rb
                    .Cast(movementInput,
                    movementFilter,
                    castCollisions,
                    moveSpeed * Time.fixedDeltaTime + collisionOffset);
            if (count == 0)
            {
                rb
                    .MovePosition(rb.position +
                    movementInput * moveSpeed * Time.fixedDeltaTime);
            }
        }
    }

    void OnMove(InputValue movementValue)
    {
        movementInput = movementValue.Get<Vector2>();
    }
}
