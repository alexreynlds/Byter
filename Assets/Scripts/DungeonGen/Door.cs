using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public enum DoorDir
    {
        up,
        down,
        left,
        right
    }

    public DoorDir doorDir;
    public GameObject doorCollider;

    private GameObject player;

    private float widthOffset = 1.75f;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            switch (doorDir) {
                case DoorDir.up:
                    player.transform.position = new Vector2(transform.position.x, transform.position.y + widthOffset);
                    break;
                case DoorDir.down:
                    player.transform.position = new Vector2(transform.position.x, transform.position.y - widthOffset);
                    break;
                case DoorDir.left:
                    player.transform.position = new Vector2(transform.position.x - widthOffset, transform.position.y);
                    break;
                case DoorDir.right:
                    player.transform.position = new Vector2(transform.position.x + widthOffset, transform.position.y);
                    break;
            }
        }
    }
}
