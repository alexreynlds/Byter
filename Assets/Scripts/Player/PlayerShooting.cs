using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooting : MonoBehaviour
{
    Vector2 shootInput;

    [SerializeField]
    private Sprite[] headSprites;

    [SerializeField]
    private GameObject head;

    [SerializeField]
    private GameObject bulletPrefab;

    private float attackDamage;
    private float lastFire;
    private float fireDelay;
    private float projectileSpeed;

    [SerializeField]
    private bool isShooting = false;

    private void Awake()
    {
        attackDamage = GetComponent<PlayerStats>().attackDamage;
        projectileSpeed = GetComponent<PlayerStats>().projectileSpeed;
        fireDelay = GetComponent<PlayerStats>().attackSpeed;
    }

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

        if (isShooting && Time.time > lastFire + fireDelay)
        {
            ShootBullet(shootInput.x, shootInput.y);
            lastFire = Time.time;
        }

        UpdateStats();
    }

    private void OnFire(InputValue shootDir)
    {
        shootInput = shootDir.Get<Vector2>();

        if (shootInput.x > 0)
        {
            shootInput.x = 1;
        }
        else if (shootInput.x < 0)
        {
            shootInput.x = -1;
        }
        else
        {
            shootInput.x = 0;
        }

        if (shootInput.y > 0)
        {
            shootInput.y = 1;
        }
        else if (shootInput.y < 0)
        {
            shootInput.y = -1;
        }
        else
        {
            shootInput.y = 0;
        }
        if (shootInput != Vector2.zero)
        {
            isShooting = true;
        }
        else
        {
            isShooting = false;
        }
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

    private void ShootBullet(float x, float y)
    {
        GameObject bullet = Instantiate(
            bulletPrefab,
            new Vector2(transform.position.x, transform.position.y + 0.3f),
            Quaternion.identity
        );
        bullet.AddComponent<Rigidbody2D>().gravityScale = 0;
        bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(x, y) * projectileSpeed;
        GetComponent<PlayerAudioManager>().PlayShootSound();
    }

    private void UpdateStats()
    {
        attackDamage = GetComponent<PlayerStats>().attackDamage;
        projectileSpeed = GetComponent<PlayerStats>().projectileSpeed;
        fireDelay = GetComponent<PlayerStats>().attackSpeed;
    }
}
