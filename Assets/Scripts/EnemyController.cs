using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum EnemyState
{
    Idle,
    Follow,
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
    [Header("Enemy Stats")]
    [SerializeField] private float range;
    [SerializeField] private float speed;
    [SerializeField] private float health;
    [SerializeField] private int damage;
    [SerializeField] private bool canMove = true;

    private GameObject player;
    private Rigidbody2D rb;

    public EnemyState currentState = EnemyState.Idle;
    public EnemyType enemyType;

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
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case EnemyState.Idle:
                // Do nothing
                break;

            case EnemyState.Follow:
                BasicFollow();
                break;

            case EnemyState.Die:
                Die();
                break;
        }

        if (!notInRoom)
        {
            if (enemyType == EnemyType.Basic)
            {
                currentState = EnemyState.Follow;
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

    void BasicFollow()
    {
        // Follow
        if (canMove)
        {
            Vector3 direction = (player.transform.position - transform.position).normalized;
            rb.velocity = direction * speed;
        }
        LookAtPlayer();
    }

    public void Die()
    {
        // Die
        DropItem();
        RoomController.instance.StartCoroutine(RoomController.instance.RoomCoroutine());
        Destroy(gameObject);
    }

    private void LookAtPlayer()
    {
        Vector2 direction = player.transform.position - transform.position;

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
