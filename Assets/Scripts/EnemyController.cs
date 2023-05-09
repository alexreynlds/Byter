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
    WormBoss,
    TrojanBoss
}

public class EnemyController : MonoBehaviour
{
    public ItemPoolData itemPoolData;

    [Header("Enemy Info")]
    public EnemyType enemyType;
    public EnemyState currentState = EnemyState.Idle;

    [Header("Enemy Stats")]
    [SerializeField]
    private float range;

    [SerializeField]
    public float health;

    public float maxHealth;

    [SerializeField]
    private int damage;

    private float originalHealth;

    private float originalProjectileSpeed;

    private float originalSpeed;

    private float originalAttackSpeed;

    private float originalAttackRange;

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
    public float timeBetweenShots;

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

    private AudioSource audioSource;

    [SerializeField]
    private AudioClip takeDamageSound;

    [SerializeField]
    private GameObject healthBarPrefab;

    private GameObject healthBar;

    private bool spawnedHealthBar = false;

    private float lastDifficulty;

    private bool hasBeenDamaged = false;

    [Header("Trojan Boss")]
    public GameObject trojanEnemyPrefab;
    public float trojanSpawnEnemyInterval = 10f;
    private float trojanSpawnEnemyTimer = 0f;
    public float trojanShootInterval = 1f;
    public float bulletDelay = 0.4f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        totalWeight = 0;
        Physics2D.queriesStartInColliders = false;
        audioSource = GetComponent<AudioSource>();

        maxHealth = health;

        originalHealth = health;
        originalProjectileSpeed = projectileSpeed;
        originalSpeed = speed;
        originalAttackSpeed = startTimeBetweenShots;
        originalAttackRange = range;


        if (itemPoolData != null)
        {
            foreach (Spawnable spawnable in itemPoolData.itemPool)
            {
                totalWeight += spawnable.weight;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        notInRoom = true;

        if (DDASystem.instance)
        {
            lastDifficulty = DDASystem.instance.currentDifficulty;
        }

        // Set up enemy stats for ranged
        if (enemyType == EnemyType.Ranged || enemyType == EnemyType.TrojanBoss)
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
        // Update the enemies states based on the current state
        if (DDASystem.instance)
        {
            if (DDASystem.instance.currentDifficulty != lastDifficulty)
            {
                lastDifficulty = DDASystem.instance.currentDifficulty;
                if (!hasBeenDamaged && enemyType != EnemyType.WormBoss && enemyType != EnemyType.TrojanBoss)
                    health = originalHealth * DDASystem.instance.currentDifficulty;
                if (enemyType == EnemyType.Ranged)
                {
                    startTimeBetweenShots = originalAttackSpeed / DDASystem.instance.currentDifficulty;
                    projectileSpeed = originalProjectileSpeed * DDASystem.instance.currentDifficulty;
                    projectileRange = originalAttackRange * DDASystem.instance.currentDifficulty;
                    if (DDASystem.instance.currentDifficulty >= 1.5)
                    {
                        damage = 2;
                    }
                    else
                    {
                        damage = 1;
                    }
                }
                else if (enemyType == EnemyType.Basic)
                {
                    speed = originalSpeed * DDASystem.instance.currentDifficulty;
                    if (DDASystem.instance.currentDifficulty >= 1.5)
                    {
                        damage = 2;
                    }
                    else
                    {
                        damage = 1;
                    }
                }
                else if (enemyType == EnemyType.TrojanBoss)
                {
                    startTimeBetweenShots = originalAttackSpeed / DDASystem.instance.currentDifficulty;
                    // projectileSpeed = originalProjectileSpeed * DDASystem.instance.currentDifficulty;
                    // projectileRange = originalAttackRange * DDASystem.instance.currentDifficulty;
                }
            }
        }


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
        else if (enemyType == EnemyType.TrojanBoss)
        {
            switch (currentState)
            {
                case EnemyState.Idle:
                    // Do nothing
                    break;

                case EnemyState.Active:
                    TrojanBossActive();
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
            else if (enemyType == EnemyType.WormBoss)
            {
                other.gameObject.GetComponent<PlayerStats>().TakeDamage(damage);
            }
            else if (enemyType == EnemyType.TrojanBoss)
            {
                other.gameObject.GetComponent<PlayerStats>().TakeDamage(damage);
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
        hasBeenDamaged = true;
        audioSource.PlayOneShot(takeDamageSound);
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

        if (enemyType.Equals(EnemyType.WormBoss) || enemyType.Equals(EnemyType.TrojanBoss))
        {
            if (DDASystem.instance)
            {
                if (DDASystem.instance.currentDifficulty < 1)
                {
                    health -= damage * 2;
                }
                else
                {
                    health -= damage;
                }
            }
            else
            {
                health -= damage;
            }
        }
        else
        {
            health -= damage;
        }

        Invoke("ResetColor", 0.1f);
        if (health <= 0)
        {
            currentState = EnemyState.Die;
        }
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

    private void TrojanBossActive()
    {
        if (!spawnedHealthBar)
        {
            spawnedHealthBar = true;

            healthBar = Instantiate(
                healthBarPrefab,
                new Vector3(0, -30.0f, 0),
                Quaternion.identity
            );
            healthBar.transform.SetParent(GameObject.Find("InGameUI").transform, false);
            healthBar.GetComponent<BossHealthBarScript>().SetBoss(this.gameObject);
        }

        trojanSpawnEnemyTimer += Time.deltaTime;

        if (trojanSpawnEnemyTimer >= trojanSpawnEnemyInterval)
        {
            trojanSpawnEnemyTimer = 0.0f;
            SpawnTrojanEnemy();
        }

        playerPos = new Vector3(player.transform.position.x, player.transform.position.y, 0);
        LookAtPlayer();

        if (timeBetweenShots <= 0)
        {
            Invoke("ShootBullet", 0f);
            Invoke("ShootBullet", 0.4f);
            Invoke("ShootBullet", 0.8f);
            timeBetweenShots = startTimeBetweenShots;
        }
        else
        {
            timeBetweenShots -= Time.deltaTime;
        }
    }

    private void ShootBullet()
    {
        Debug.Log("ShootBullet");
        GameObject bullet = Instantiate(
                projectile,
                transform.position,
                Quaternion.identity
            );
        bullet.GetComponent<EnemyBulletController>().parent = this.gameObject;
        bullet.GetComponent<EnemyBulletController>().damage = damage;
        bullet.GetComponent<EnemyBulletController>().range = projectileRange;
        bullet.transform.localScale = new Vector2(0.5f, 0.5f);
        bullet.AddComponent<Rigidbody2D>().gravityScale = 0;
        bullet.GetComponent<Rigidbody2D>().velocity =
            (playerPos - transform.position).normalized * projectileSpeed;
    }

    private void SpawnTrojanEnemy()
    {
        Instantiate(
            trojanEnemyPrefab,
            new Vector3(transform.position.x + 1.5f, transform.position.y, 0),
            Quaternion.identity
        );

        Instantiate(
           trojanEnemyPrefab,
           new Vector3(transform.position.x - 1.5f, transform.position.y, 0),
           Quaternion.identity
       );
    }
    private void WormBossActive()
    {
        if (!spawnedHealthBar)
        {
            spawnedHealthBar = true;

            healthBar = Instantiate(
                healthBarPrefab,
                new Vector3(0, -30.0f, 0),
                Quaternion.identity
            );
            healthBar.transform.SetParent(GameObject.Find("InGameUI").transform, false);
            healthBar.GetComponent<BossHealthBarScript>().SetBoss(this.gameObject);
        }

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
            Destroy(healthBar);
            Instantiate(endPortal, transform.position, Quaternion.identity);
        }
        else if (enemyType == EnemyType.TrojanBoss)
        {
            Destroy(healthBar);
            Instantiate(endPortal, transform.position, Quaternion.identity);
        }
        else
        {
            DropItem();
        }

        if (DDASystem.instance)
        {
            GameObject.Find("RoomController").GetComponent<DDASystem>().enemiesKilled++;
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
        float cumulativeWeight = itemPoolData.itemPool[0].weight;

        while (pick > cumulativeWeight && chosenIndex < itemPoolData.itemPool.Count - 1)
        {
            chosenIndex++;
            cumulativeWeight += itemPoolData.itemPool[chosenIndex].weight;
        }

        if (itemPoolData.itemPool[chosenIndex].gameObject != null)
        {
            RoomController.instance.spawnItem(
                itemPoolData.itemPool[chosenIndex].gameObject,
                transform.position
            );
        }
        else
        {
            return;
        }

        if (DDASystem.instance)
        {
            if (DDASystem.instance.currentDifficulty < 1)
            {
                int extraDrops = Random.Range(0, 2);
                if (extraDrops == 1)
                {
                    bool itemFound = false;
                    while (!itemFound)
                    {
                        pick = Random.Range(0, totalWeight);
                        chosenIndex = 0;
                        cumulativeWeight = itemPoolData.itemPool[0].weight;

                        while (pick > cumulativeWeight && chosenIndex < itemPoolData.itemPool.Count - 1)
                        {
                            chosenIndex++;
                            cumulativeWeight += itemPoolData.itemPool[chosenIndex].weight;
                        }

                        if (itemPoolData.itemPool[chosenIndex].gameObject != null)
                        {
                            itemFound = true;
                            RoomController.instance.spawnItem(
                                itemPoolData.itemPool[chosenIndex].gameObject,
                                new Vector3(transform.position.x, transform.position.y + 0.5f, 0)
                            );
                        }
                        else
                        {
                            return;
                        }
                    }
                }


            }
        }
    }
}
