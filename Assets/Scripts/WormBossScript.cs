using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormBossScript : MonoBehaviour
{
    public GameObject bodyPart;

    public EnemyState currentState = EnemyState.Idle;

    private List<GameObject> bodyParts = new List<GameObject>();
    private List<Vector2> directions = new List<Vector2> { new Vector2(0, 1), new Vector2(0, -1), new Vector2(-1, 0), new Vector2(1, 0) };
    private Vector3 targetPos;
    private bool atTarget = false;
    private bool hasTarget = false;
    public int bodyLength = 5;
    public bool notInRoom = true;

    private List<Vector3> previousPos = new List<Vector3>();

    public int health = 5;

    public float movementSpeed = 0.01f; // the movement speed of the boss

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < bodyLength; i++)
        {
            GameObject temp = Instantiate(bodyPart, new Vector3(transform.position.x, transform.position.y, 0), Quaternion.identity, transform);
            bodyParts.Add(temp);

        }
    }

    void Update()
    {
        switch (currentState)
        {
            case EnemyState.Idle:
                // Do nothing
                break;

            case EnemyState.Active:
                BossUpdate();
                break;

            case EnemyState.Die:
                Die();
                break;
        }
        if (!notInRoom)
        {
            currentState = EnemyState.Active;
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        transform.GetComponent<SpriteRenderer>().color = Color.red;
        foreach (Transform child in transform)
        {
            child.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        }

        Invoke("ResetColor", 0.1f);
        if (health <= 0)
        {
            currentState = EnemyState.Die;
        }
    }


    public void ResetColor()
    {
        transform.GetComponent<SpriteRenderer>().color = Color.white;
        foreach (Transform child in transform)
        {
            child.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }

    void BossUpdate()
    {
        if (!hasTarget)
        {
            findNextPos();
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, targetPos, movementSpeed);
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

    void findNextPos()
    {
        bool foundPos = false;
        int iterations = 0;
        while (!foundPos && iterations < 100)
        {
            Vector2 tempDir = directions[Random.Range(0, directions.Count)];
            RaycastHit2D hit = Physics2D.Raycast(transform.position, tempDir, 1f);
            if (hit.collider == null || hit.collider.gameObject.CompareTag("Player") || hit.collider.gameObject.CompareTag("PlayerBody"))
            {
                bool foundBody = false;
                for (int i = 0; i < bodyParts.Count; i++)
                {
                    Vector3 bodyPos = bodyParts[i].transform.position;
                    if (Vector3.Distance(bodyPos, transform.position + new Vector3(tempDir.x, tempDir.y, 0)) < 0.1f ||
                        Vector3.Distance(bodyPos, transform.position + new Vector3(tempDir.x * 2, tempDir.y * 2, 0)) < 0.1f ||
                        Vector3.Distance(bodyPos, transform.position + new Vector3(tempDir.x * 3, tempDir.y * 3, 0)) < 0.1f)
                    {
                        Debug.Log("Found body");
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



}
