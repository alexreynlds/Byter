using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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
}

public class EnemyController : MonoBehaviour
{
    [Header("Enemy Info")]
    public EnemyType enemyType;
    public EnemyState currentState = EnemyState.Idle;

    [Header("Enemy Stats")]
    [SerializeField] private float range;
    [SerializeField] private float health;
    [SerializeField] private int damage;

    [Header("Basic Enemy Stats")]
    [SerializeField] private float speed;
    [SerializeField] private bool canMove = true;

    [Header("Ranged Enemy Stats")]
    [SerializeField] private float startTimeBetweenShots;
    [SerializeField] private float projectileSpeed;
    [SerializeField] private float projectileRange;
    [SerializeField] private GameObject projectile;
    private float timeBetweenShots;


    private GameObject player;
    private Vector3 playerPos;
    private Rigidbody2D rb;



    public bool notInRoom = true;

    float totalWeight;

    public List<Spawnable> itemPool = new List<Spawnable>();

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        totalWeight = 0;

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


        if (!notInRoom)
        {
            if (enemyType == EnemyType.Basic)
            {
                currentState = EnemyState.Active;
            }

        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("hit");
            other.gameObject.GetComponent<PlayerStats>().TakeDamage(damage);
            canMove = false;
            TakeKnockback();
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
        health--;
        GetComponent<SpriteRenderer>().color = Color.red;
        Invoke("ResetColor", 0.1f);
        if (health <= 0)
        {
            currentState = EnemyState.Die;
        }
    }

    private void ResetColor()
    {
        GetComponent<SpriteRenderer>().color = Color.white;
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
        playerPos = new Vector3(player.transform.position.x, player.transform.position.y + 0.5f, 0);
        LookAtPlayer();
        if (timeBetweenShots <= 0)
        {
            GameObject bullet = Instantiate(projectile, transform.position, Quaternion.identity);
            bullet.GetComponent<EnemyBulletController>().parent = this.gameObject;
            bullet.GetComponent<EnemyBulletController>().damage = damage;
            bullet.GetComponent<EnemyBulletController>().range = projectileRange;
            bullet.AddComponent<Rigidbody2D>().gravityScale = 0;
            bullet.GetComponent<Rigidbody2D>().velocity = (playerPos - transform.position).normalized * projectileSpeed;
            timeBetweenShots = startTimeBetweenShots;

        }
        else
        {
            timeBetweenShots -= Time.deltaTime;
        }
    }

    public void Die()
    {
        // Die
        DropItem();
        // RoomController.instance.StartCoroutine(RoomController.instance.RoomCoroutine());
        Destroy(gameObject);
    }

    private void LookAtPlayer()
    {

        Vector2 direction = playerPos - transform.position;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;

        Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 5 * Time.deltaTime);
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
