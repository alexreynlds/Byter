using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpawner : MonoBehaviour
{
    public int openingDirection;

    // 1 --> need bottom door
    // 2 --> need top door
    // 3 --> need left door
    // 4 --> need right door
    private RoomTemplates templates;

    private int rand;

    private bool spawned = false;

    void Awake()
    {
        templates =
            GameObject
                .FindGameObjectWithTag("Rooms")
                .GetComponent<RoomTemplates>();
    }

    void Start()
    {
        Invoke("Spawn", 0.4f);
    }

    void Spawn()
    {
        if (!spawned)
        {
            if (openingDirection == 1)
            {
                // Need to spawn a room with a BOTTOM door
                Instantiate(templates
                    .bottomRooms[Random.Range(0, templates.bottomRooms.Length)],
                transform.position,
                templates
                    .bottomRooms[Random.Range(0, templates.bottomRooms.Length)]
                    .transform
                    .rotation);
            }
            else if (openingDirection == 2)
            {
                // Need to spawn a room with a TOP door
                Instantiate(templates
                    .topRooms[Random.Range(0, templates.topRooms.Length)],
                transform.position,
                templates
                    .topRooms[Random.Range(0, templates.topRooms.Length)]
                    .transform
                    .rotation);
            }
            else if (openingDirection == 3)
            {
                // Need to spawn a room with a LEFT door
                Instantiate(templates
                    .leftRooms[Random.Range(0, templates.leftRooms.Length)],
                transform.position,
                templates
                    .leftRooms[Random.Range(0, templates.leftRooms.Length)]
                    .transform
                    .rotation);
            }
            else if (openingDirection == 4)
            {
                // Need to spawn a room with a RIGHT door
                Instantiate(templates
                    .rightRooms[Random.Range(0, templates.rightRooms.Length)],
                transform.position,
                templates
                    .rightRooms[Random.Range(0, templates.rightRooms.Length)]
                    .transform
                    .rotation);
            }
            spawned = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        templates =
            GameObject
                .FindGameObjectWithTag("Rooms")
                .GetComponent<RoomTemplates>();
        if (other.CompareTag("SpawnPoint"))
        {
            if (
                other.GetComponent<RoomSpawner>().spawned == false &&
                spawned == false
            )
            {
                Instantiate(templates.closedRoom,
                transform.position,
                Quaternion.identity);
                Destroy(other.gameObject);
            }
            spawned = true;
        }
    }
}
