using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public enum EnemyState
{
    Idle,
    Active,
    Die
};

public enum EnemyType
{
    Basic,
    Ranged,
    Tank,
    Small,
    WormBoss
}

public class EnemyController : MonoBehaviour
{
    [Header("Enemy Info")]
    public EnemyType enemyType;
    public EnemyState currentState = EnemyState.Idle;

    [Header("Enemy Stats")]
    [SerializeField]
    private float range;

    [SerializeField]
    private float health;

    [SerializeField]
    private int damage;

    [Header("Basic Enemy Stats")]
    [SerializeField]
    private float speed;

    [SerializeField]
    private bool canMove = true;

    [Header("Ranged Enemy Stats")]
    [SerializeField]
    private float startTimeBetweenShots;

    [SerializeField]
    private float projectileSpeed;

    [SerializeField]
    private float projectileRange;

    [SerializeField]
    private GameObject projectile;

    [SerializeField]
    private GameObject firePoint;

    [SerializeField]
    private LayerMask Mask;
    private float timeBetweenShots;

    [Header("Worm Boss Stats")]
    public GameObject bodyPart;
    public GameObject endPortal;

    private List<GameObject> bodyParts = new List<GameObject>();
    private List<Vector2> directions = new List<Vector2>
    {
        new Vector2(0, 1),
        new Vector2(0, -1),
        new Vector2(-1, 0),
        new Vector2(1, 0)
    };
    private Vector3 targetPos;

    private bool hasTarget = false;
    public int bodyLength = 5;
    private List<Vector3> previousPos = new List<Vector3>();
    public float wormBossMovementSpeed = 0.01f; // the movement speed of the boss

    private GameObject player;
    private Vector3 playerPos;
    private Rigidbody2D rb;

    public bool notInRoom = true;

    float totalWeight;

    public List<Spawnable> itemPool = new List<Spawnable>();

    private AudioSource audioSource;

    [SerializeField]
    private AudioClip takeDamageSound;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        totalWeight = 0;
        Physics2D.queriesStartInColliders = false;
        audioSource = GetComponent<AudioSource>();
        foreach (Spawnable spawnable in itemPool)
        {
            totalWeight += spawnable.weight;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        notInRoom = true;

        // Set up enemy stats for ranged
        if (enemyType == EnemyType.Ranged)
        {
            timeBetweenShots = startTimeBetweenShots;
        }
        else if (enemyType == EnemyType.WormBoss)
        {
            for (int i = 0; i < bodyLength; i++)
            {
                GameObject temp = Instantiate(
                    bodyPart,
                    new Vector3(transform.position.x, transform.position.y, 0),
                    Quaternion.identity,
                    transform
                );
                bodyParts.Add(temp);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyType == EnemyType.Basic)
        {
            switch (currentState)
            {
                case EnemyState.Idle:
                    // Do nothing
                    break;

                case EnemyState.Active:
                    BasicActive();
                    break;

                case EnemyState.Die:
                    Die();
                    break;
            }
        }
        else if (enemyType == EnemyType.Ranged)
        {
            switch (currentState)
            {
                case EnemyState.Idle:
                    // Do nothing
                    break;

                case EnemyState.Active:
                    RangedActive();
                    break;

                case EnemyState.Die:
                    Die();
                    break;
            }
        }
        else if (enemyType == EnemyType.WormBoss)
        {
            switch (currentState)
            {
                case EnemyState.Idle:
                    // Do nothing
                    break;

                case EnemyState.Active:
                    WormBossActive();
                    break;

                case EnemyState.Die:
                    Die();
                    break;
            }
        }

        if (!notInRoom)
        {
            currentState = EnemyState.Active;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (enemyType == EnemyType.Basic)
            {
                // Debug.Log("hit");
                other.gameObject.GetComponent<PlayerStats>().TakeDamage(damage);
                canMove = false;
                TakeKnockback();
            }
        }
    }

    public void TakeKnockback()
    {
        rb.velocity = Vector2.zero;
        Vector2 direction = (transform.position - player.transform.position).normalized;
        rb.AddForce(direction * 2.0f, ForceMode2D.Impulse);
        StartCoroutine(StopKnockback());
    }

    private IEnumerator StopKnockback()
    {
        yield return new WaitForSeconds(0.5f);
        rb.velocity = Vector2.zero;
        canMove = true;
    }

    public void TakeDamage(float damage)
    {
        // if (!enemyType.Equals(EnemyType.WormBoss))
        // {
        //     audioSource.PlayOneShot(takeDamageSound);
        //     health--;
        //     GetComponent<SpriteRenderer>().color = Color.red;
        //     Invoke("ResetColor", 0.1f);
        //     if (health <= 0)
        //     {
        //         currentState = EnemyState.Die;
        //     }
        // }
        // else
        // {
        audioSource.PlayOneShot(takeDamageSound);
        health -= damage;
        GetComponent<SpriteRenderer>().color = Color.red;

        if (enemyType.Equals(EnemyType.WormBoss))
        {
            foreach (Transform child in transform)
            {
                child.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
            }
        }
        else
        {
            transform.GetComponent<SpriteRenderer>().color = Color.red;
        }

        Invoke("ResetColor", 0.1f);
        if (health <= 0)
        {
            currentState = EnemyState.Die;
        }
        // }
    }

    private void ResetColor()
    {
        if (!enemyType.Equals(EnemyType.WormBoss))
        {
            GetComponent<SpriteRenderer>().color = Color.white;
        }
        else
        {
            transform.GetComponent<SpriteRenderer>().color = Color.white;
            foreach (Transform child in transform)
            {
                child.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
            }
        }
    }

    void BasicActive()
    {
        // Follow
        if (canMove)
        {
            Vector3 direction = (player.transform.position - transform.position).normalized;
            rb.velocity = direction * speed;
        }
        LookAtPlayer();
    }

    void RangedActive()
    {
        playerPos = new Vector3(player.transform.position.x, player.transform.position.y, 0);

        if (rb.velocity.magnitude > 0)
        {
            rb.velocity = Vector2.zero;
        }

        if (CheckLOS())
        {
            LookAtPlayer();
            if (timeBetweenShots <= 0)
            {
                GameObject bullet = Instantiate(
                    projectile,
                    transform.position,
                    Quaternion.identity
                );
                bullet.GetComponent<EnemyBulletController>().parent = this.gameObject;
                bullet.GetComponent<EnemyBulletController>().damage = damage;
                bullet.GetComponent<EnemyBulletController>().range = projectileRange;
                bullet.AddComponent<Rigidbody2D>().gravityScale = 0;
                bullet.GetComponent<Rigidbody2D>().velocity =
                    (playerPos - transform.position).normalized * projectileSpeed;
                timeBetweenShots = startTimeBetweenShots;
            }
            else
            {
                timeBetweenShots -= Time.deltaTime;
            }
        }
    }

    private bool CheckLOS()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player == null)
            return false;

        Vector3 direction = player.transform.position - firePoint.transform.position;
        float distance = direction.magnitude;

        RaycastHit2D hit = Physics2D.Raycast(
            firePoint.transform.position,
            direction.normalized,
            Mathf.Infinity,
            Mask
        );
        if (hit.collider == null)
            return false;
        // Debug.DrawRay(firePoint.transform.position, direction.normalized * distance, Color.red);

        if (
            hit.collider.gameObject.CompareTag("Player")
            || hit.collider.gameObject.CompareTag("PlayerBody")
        )
        {
            return true;
        }
        // Debug.Log("No LOS");
        return false;
    }

    private void WormBossActive()
    {
        if (!hasTarget)
        {
            findNextPos();
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, targetPos, wormBossMovementSpeed);
            for (int i = 0; i < bodyParts.Count; i++)
            {
                if (i == 0)
                {
                    if (previousPos.Count > 0)
                    {
                        bodyParts[i].transform.position = previousPos[previousPos.Count - 1];
                    }
                }
                else
                {
                    if (previousPos.Count > i - 1)
                    {
                        bodyParts[i].transform.position = previousPos[previousPos.Count - i];
                    }
                }
            }
            if (Vector3.Distance(transform.position, targetPos) < 0.001f)
            {
                hasTarget = false;
                previousPos.Add(transform.position);
            }
        }

        if (previousPos.Count > bodyLength)
        {
            previousPos.RemoveAt(0);
        }
    }

    private void findNextPos()
    {
        bool foundPos = false;
        int iterations = 0;
        while (!foundPos && iterations < 100)
        {
            Vector2 tempDir = directions[Random.Range(0, directions.Count)];
            RaycastHit2D hit = Physics2D.Raycast(transform.position, tempDir, 1f);
            if (
                hit.collider == null
                || hit.collider.gameObject.CompareTag("Player")
                || hit.collider.gameObject.CompareTag("PlayerBody")
            )
            {
                bool foundBody = false;
                for (int i = 0; i < bodyParts.Count; i++)
                {
                    Vector3 bodyPos = bodyParts[i].transform.position;
                    if (
                        Vector3.Distance(
                            bodyPos,
                            transform.position + new Vector3(tempDir.x, tempDir.y, 0)
                        ) < 0.1f
                        || Vector3.Distance(
                            bodyPos,
                            transform.position + new Vector3(tempDir.x * 2, tempDir.y * 2, 0)
                        ) < 0.1f
                        || Vector3.Distance(
                            bodyPos,
                            transform.position + new Vector3(tempDir.x * 3, tempDir.y * 3, 0)
                        ) < 0.1f
                    )
                    {
                        // Debug.Log("Found body");
                        foundBody = true;
                        break;
                    }
                }

                if (!foundBody)
                {
                    foundPos = true;
                    targetPos = transform.position + new Vector3(tempDir.x, tempDir.y, 0);
                }
            }
            iterations++;
        }
        hasTarget = true;
    }

    public void Die()
    {
        if (enemyType == EnemyType.WormBoss)
        {
            Instantiate(endPortal, transform.position, Quaternion.identity);
        }
        else
        {
            DropItem();
        }

        player.GetComponent<PlayerAudioManager>().DeathSound();
        RoomController.instance.StartCoroutine(RoomController.instance.RoomCoroutine());
        Destroy(gameObject);
    }

    private void LookAtPlayer()
    {
        Vector2 direction = playerPos - transform.position;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;

        Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            5 * Time.deltaTime
        );
    }

    private void DropItem()
    {
        float pick = Random.Range(0, totalWeight);
        int chosenIndex = 0;
        float cumulativeWeight = itemPool[0].weight;

        while (pick > cumulativeWeight && chosenIndex < itemPool.Count - 1)
        {
            chosenIndex++;
            cumulativeWeight += itemPool[chosenIndex].weight;
        }

        if (itemPool[chosenIndex].gameObject != null)
        {
            RoomController.instance.spawnItem(itemPool[chosenIndex].gameObject, transform.position);
        }
        else
        {
            return;
        }
    }
}
