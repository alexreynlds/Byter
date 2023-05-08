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
    public GameObject keycardCollider;
    public GameObject door;

    public Sprite[] doorSprites;

    private GameObject player;

    // private bool playerCanMove = true;

    private float widthOffset = 3.7f;
    private float heightOffset = 3.7f;
    public bool keycardLocked = false;
    public bool bossKeycardLocked = false;

    public bool open = true;
    public bool isShopDoor;
    public bool isItemRoomDoor;
    public bool isBossDoor;

    private Vector2 doorColliderSize;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        door = transform.Find("Door").gameObject;
        door.GetComponent<SpriteRenderer>().sprite = doorSprites[0];
        doorColliderSize = transform.GetComponent<BoxCollider2D>().size;
    }

    private void Update() { }

    public void UpdateDoorData()
    {
        if (!open)
        {
            Close();
        }
        else if (open && !keycardLocked && !isShopDoor && !isItemRoomDoor && !isBossDoor)
        {
            Open();
        }
        else if (open && keycardLocked && !isShopDoor && isItemRoomDoor && !isBossDoor)
        {
            KeycardClose();
        }
        else if (open && !keycardLocked && !isShopDoor && isItemRoomDoor && !isBossDoor)
        {
            KeycardOpen();
        }
        else if (open && !keycardLocked && isShopDoor && !isItemRoomDoor && !isBossDoor)
        {
            ShopRoom();
        }
        else if (
            open
            && !keycardLocked
            && !isShopDoor
            && !isItemRoomDoor
            && isBossDoor
            && !bossKeycardLocked
        )
        {
            BossRoomUnlocked();
        }
        else if (
            open
            && !keycardLocked
            && !isShopDoor
            && !isItemRoomDoor
            && isBossDoor
            && bossKeycardLocked
        )
        {
            BossRoomLocked();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == ("Player") && !keycardLocked && !bossKeycardLocked && open)
        {
            switch (doorDir)
            {
                case DoorDir.up:
                    player.transform.position = new Vector2(
                        player.transform.position.x,
                        player.transform.position.y + heightOffset
                    );
                    break;
                case DoorDir.down:
                    player.transform.position = new Vector2(
                        player.transform.position.x,
                        player.transform.position.y - heightOffset
                    );

                    break;
                case DoorDir.left:
                    player.transform.position = new Vector2(
                        player.transform.position.x - widthOffset,
                        player.transform.position.y
                    );
                    break;
                case DoorDir.right:
                    player.transform.position = new Vector2(
                        player.transform.position.x + widthOffset,
                        player.transform.position.y
                    );
                    break;
            }
        }
        if (other.gameObject.tag == ("Player") && keycardLocked)
        {
            if (other.gameObject.GetComponent<PlayerStats>().keycards > 0)
            {
                other.gameObject.GetComponent<PlayerStats>().keycards -= 1;
                KeycardOpen();
            }
            else
            {
                FindObjectOfType<PopupWindowScript>().AddToQueue("Room Locked!", "Find the key!");
                return;
            }
        }
        if (other.gameObject.tag == ("Player") && bossKeycardLocked)
        {
            if (other.gameObject.GetComponent<PlayerStats>().bossKeycard == true)
            {
                BossRoomUnlocked();
            }
            else
            {
                FindObjectOfType<PopupWindowScript>().AddToQueue("Room Locked!", "Find the key!");
                return;
            }
        }
    }

    public void BossRoomLocked()
    {
        bossKeycardLocked = true;
        doorCollider.SetActive(true);
        // transform.GetComponent<BoxCollider2D>().isTrigger = false;
        door.GetComponent<SpriteRenderer>().sprite = doorSprites[6];
        transform.GetComponent<BoxCollider2D>().size = new Vector2(1.0f, 1.0f);
    }

    public void BossRoomUnlocked()
    {
        bossKeycardLocked = false;
        doorCollider.SetActive(false);
        door.GetComponent<SpriteRenderer>().sprite = doorSprites[5];
        transform.GetComponent<BoxCollider2D>().size = doorColliderSize;
        RoomController.instance.BossRomUnlockDoors();
    }

    public void ShopRoom()
    {
        door.GetComponent<SpriteRenderer>().sprite = doorSprites[4];
    }

    public void KeycardClose()
    {
        keycardLocked = true;
        doorCollider.SetActive(true);
        // transform.GetComponent<BoxCollider2D>().isTrigger = false;
        door.GetComponent<SpriteRenderer>().sprite = doorSprites[2];
        transform.GetComponent<BoxCollider2D>().size = new Vector2(1.0f, 1.0f);
    }

    public void KeycardOpen()
    {
        keycardLocked = false;
        doorCollider.SetActive(false);
        // transform.GetComponent<BoxCollider2D>().isTrigger = true;
        door.GetComponent<SpriteRenderer>().sprite = doorSprites[3];
        transform.GetComponent<BoxCollider2D>().size = doorColliderSize;
        RoomController.instance.ItemRoomUnlockDoors();
    }

    public void Close()
    {
        open = false;
        doorCollider.SetActive(true);
        door.GetComponent<SpriteRenderer>().sprite = doorSprites[1];
    }

    public void Open()
    {
        open = true;
        doorCollider.SetActive(false);
        door.GetComponent<SpriteRenderer>().sprite = doorSprites[0];

        if (keycardLocked)
        {
            KeycardClose();
        }
    }

    // private IEnumerator MovePlayer(Vector2 destination, float speed)
    // {
    //     Debug.Log("Moving player to " + destination);
    //     float startTime = Time.time;
    //     float journeyLength = Vector2.Distance(player.transform.position, destination);
    //     float totalTime = journeyLength / speed;

    //     while (Time.time < startTime + totalTime)
    //     {
    //         float distCovered = (Time.time - startTime) * speed;
    //         float fracJourney = distCovered / journeyLength;
    //         player.transform.position = Vector2.Lerp(player.transform.position, destination, fracJourney);

    //         yield return null;
    //     }

    //     // Ensure the player reaches the destination exactly
    //     player.transform.position = destination;
    // }
}
