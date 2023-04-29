using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum EnemyState
{
    Idle,
    Follow,
    Die
};
public class EnemyController : MonoBehaviour
{
    GameObject player;
    public EnemyState currentState = EnemyState.Idle;

    [Header("Enemy Stats")]
    [SerializeField] private float range;
    [SerializeField] private float speed;
    [SerializeField] private float health;
    [SerializeField] private int damage;
    [SerializeField] private bool canMove = true;

    public Rigidbody2D rb;

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
                // idle();
                break;
            case EnemyState.Follow:
                // Follow
                Follow();
                break;
            case EnemyState.Die:
                // Die
                Die();
                break;
        }

        if (!notInRoom)
        {
            currentState = EnemyState.Follow;
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

    void Follow()
    {
        // Follow
        if (canMove)
        {
            Vector3 direction = (player.transform.position - transform.position).normalized;
            rb.velocity = direction * speed;
        }
        LookAtPlayer();

        // transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        // if (!InRange())
        // {
        //     currentState = EnemyState.Wander;
        // }
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

        // calculate the angle between the object's forward direction and the direction to the player
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;

        // use Quaternion.Slerp to rotate smoothly towards the player
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
