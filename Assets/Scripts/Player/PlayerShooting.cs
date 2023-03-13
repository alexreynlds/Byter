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

    public GameObject bulletPrefab;

    public GameObject bulletSpawn;

    private bool isFiring;

    private bool canFire;

    private float fireSpeed;

    private float fireTimer;

    public PlayerInputActions controls;

    public float bulletForce;

    private void Awake()
    {
        controls = new PlayerInputActions();
    }

    void Start()
    {
        playerHeadObject = GameObject.Find("PlayerHead");

        playerHeadSpriteRenderer =
            playerHeadObject.GetComponent<SpriteRenderer>();

        bulletSpawn = GameObject.Find("BulletSpawn");

        isFiring = false;
        fireTimer = fireSpeed;
        canFire = true;
        fireSpeed = GetComponent<PlayerStats>().fireSpeed;

        controls.Player.Shoot.performed += ctx =>
            doShoot(ctx.ReadValue<Vector2>());
        controls.Player.Shoot.canceled += ctx =>
            doShoot(ctx.ReadValue<Vector2>());
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
            playerHeadSpriteRenderer.sprite = headSprites[2];
            playerHeadSpriteRenderer.flipX = true;
            bulletSpawn.transform.rotation = Quaternion.Euler(0, 0, -90);
        }
        else if (shootDir == "left")
        {
            playerHeadSpriteRenderer.sprite = headSprites[2];
            playerHeadSpriteRenderer.flipX = false;
            bulletSpawn.transform.rotation = Quaternion.Euler(0, 0, 90);
        }
        else if (shootDir == "up")
        {
            playerHeadSpriteRenderer.sprite = headSprites[1];
            playerHeadSpriteRenderer.flipX = false;
            bulletSpawn.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (shootDir == "down")
        {
            playerHeadSpriteRenderer.sprite = headSprites[0];
            playerHeadSpriteRenderer.flipX = false;
            bulletSpawn.transform.rotation = Quaternion.Euler(0, 0, 180);
        }
        else if (shootDir == "none")
        {
            playerHeadSpriteRenderer.sprite = headSprites[0];
            playerHeadSpriteRenderer.flipX = false;
            bulletSpawn.transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        // Shooting
        if (isFiring && canFire)
        {
            canFire = false;
            fireTimer = fireSpeed;
            GameObject bullet =
                Instantiate(bulletPrefab,
                transform.position,
                Quaternion.Euler(0, 0, 0));
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.AddForce(bulletSpawn.transform.up * bulletForce, ForceMode2D.Impulse);
        }
        if (fireTimer >= 0)
        {
            fireTimer -= 0.05f;
        }
        else
        {
            canFire = true;
            fireTimer = fireSpeed;
        }
    }

    void doShoot(Vector2 shootVal)
    {
        shootInput = shootVal;
        isFiring = !isFiring;
    }

    void OnTest(Vector2 shootVal)
    {
        Debug.Log("epic");
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }
}
