using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState
{
    Idle,
    Wander,
    Follow,
    Die
};
public class EnemyController : MonoBehaviour
{
    GameObject player;
    public EnemyState currentState = EnemyState.Wander;

    [Header("Enemy Stats")]
    [SerializeField] private float range;
    [SerializeField] private float speed;
    [SerializeField] private float health;

    private bool chooseDir = false;
    // private bool dead = false;
    private Vector3 wanderTarget;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");

    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case EnemyState.Idle:
                // Do nothing
                break;
            case EnemyState.Wander:
                // Wander
                Wander();
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

        if (InRange() && currentState != EnemyState.Die)
        {
            currentState = EnemyState.Follow;
        }
        else if (!InRange() && currentState != EnemyState.Die)
        {
            currentState = EnemyState.Wander;
        }
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

    private bool InRange()
    {
        return Vector3.Distance(transform.position, player.transform.position) <= range;
    }

    private IEnumerator ChooseDir()
    {
        chooseDir = true;
        yield return new WaitForSeconds(Random.Range(2f, 8f));
        wanderTarget = new Vector3(0, 0, Random.Range(0, 360));
        Quaternion nextRot = Quaternion.Euler(wanderTarget);
        transform.rotation = Quaternion.Lerp(transform.rotation, nextRot, Random.Range(0.5f, 2.5f));
        chooseDir = false;
    }

    void Wander()
    {
        // Wander
        if (!chooseDir)
        {
            StartCoroutine(ChooseDir());
        }
        transform.position += -transform.right * speed * Time.deltaTime;
        if (InRange())
        {
            currentState = EnemyState.Follow;
        }
    }

    void Follow()
    {
        // Follow
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        if (!InRange())
        {
            currentState = EnemyState.Wander;
        }
    }

    public void Die()
    {
        // Die
        Destroy(gameObject);
    }
}
