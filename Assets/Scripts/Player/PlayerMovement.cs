using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    Vector2 movementInput;

    Rigidbody2D rb;

    private float moveSpeed;

    public float collisionOffset = 0.05f;

    public ContactFilter2D movementFilter;

    [Header("Sprites")]
    [SerializeField]
    private Sprite[] bodySprites;

    [SerializeField]
    private Sprite[] bottomSprites;

    [SerializeField]
    private GameObject body;

    [SerializeField]
    private GameObject bottom;

    private int currentBottomSprite = 0;

    [SerializeField]
    private float bottomSpriteChangeTimer = 0.25f;

    private float bottomSpriteChangeTimerTemp;
    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        moveSpeed = GetComponent<PlayerStats>().moveSpeed;
        bottomSpriteChangeTimerTemp = bottomSpriteChangeTimer;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // if (movementInput != Vector2.zero)
        // {
        //     bool success = TryMove(movementInput);

        //     if (!success)
        //     {
        //         success = TryMove(new Vector2(movementInput.x, 0));

        //         if (!success)
        //         {
        //             TryMove(new Vector2(0, movementInput.y));
        //         }
        //     }
        //     UpdateSprite();
        // }
        if (movementInput != Vector2.zero)
        {
            rb.MovePosition(rb.position + movementInput * moveSpeed * Time.deltaTime);
            rb.velocity = Vector2.zero;
            // rb.AddForce(movementInput * moveSpeed * Time.deltaTime);
            UpdateSprite();
        }
    }

    void Update()
    {
        if (bottomSpriteChangeTimer > 0)
        {
            bottomSpriteChangeTimer -= Time.deltaTime;
        }
        else
        {
            UpdateBottomSprite();
            bottomSpriteChangeTimer = bottomSpriteChangeTimerTemp;
        }
    }

    private bool TryMove(Vector2 direction)
    {
        int count = rb.Cast(
            direction,
            movementFilter,
            castCollisions,
            moveSpeed * Time.deltaTime + collisionOffset
        );

        if (count == 0)
        {
            rb.MovePosition(rb.position + direction * moveSpeed * Time.deltaTime);
            return true;
        }
        else
        {
            return false;
        }
    }

    void OnMove(InputValue movementValue)
    {
        // movementInput = movementValue.Get<Vector2>();

        movementInput = movementValue.Get<Vector2>();
    }

    void UpdateSprite()
    {
        if (movementInput.x > 0)
        {
            body.GetComponent<SpriteRenderer>().flipX = true;
            body.GetComponent<SpriteRenderer>().sprite = bodySprites[1];
        }
        else if (movementInput.x < 0)
        {
            body.GetComponent<SpriteRenderer>().flipX = false;
            body.GetComponent<SpriteRenderer>().sprite = bodySprites[1];
        }
        else if (movementInput.y > 0)
        {
            body.GetComponent<SpriteRenderer>().flipX = false;
            body.GetComponent<SpriteRenderer>().sprite = bodySprites[2];
        }
        else if (movementInput.y < 0)
        {
            body.GetComponent<SpriteRenderer>().flipX = false;
            body.GetComponent<SpriteRenderer>().sprite = bodySprites[0];
        }
    }

    void UpdateBottomSprite()
    {
        currentBottomSprite++;
        if (currentBottomSprite > 2)
            currentBottomSprite = 0;

        bottom.GetComponent<SpriteRenderer>().sprite = bottomSprites[currentBottomSprite];
    }
}
