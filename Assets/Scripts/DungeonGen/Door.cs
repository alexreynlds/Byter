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

    private float widthOffset = 3.5f;
    private float heightOffset = 3.5f;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == ("Player"))
        {
            switch (doorDir)
            {

                case DoorDir.up:
                    player.transform.position = new Vector2(player.transform.position.x, player.transform.position.y + heightOffset);
                    break;
                case DoorDir.down:
                    player.transform.position = new Vector2(player.transform.position.x, player.transform.position.y - heightOffset);
                    break;
                case DoorDir.left:
                    player.transform.position = new Vector2(player.transform.position.x - widthOffset, player.transform.position.y);
                    break;
                case DoorDir.right:
                    player.transform.position = new Vector2(player.transform.position.x + widthOffset, player.transform.position.y);
                    break;
            }
        }
    }

    public void Close()
    {
        doorCollider.SetActive(true);
        transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.red;
    }

    public void Open()
    {
        doorCollider.SetActive(false);
        transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.white;
    }
}
