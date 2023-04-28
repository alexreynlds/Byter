using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public int roomW;

    public int roomH;

    public int X;

    public int Y;

    // private bool updatedDoors = false;

    public Room(int x, int y)
    {
        X = x;
        Y = y;
    }

    public Door topDoor;

    public Door bottomDoor;

    public Door leftDoor;

    public Door rightDoor;

    public List<Door> doors = new List<Door>();

    public bool isActive;

    // Start is called before the first frame update
    void Start()
    {
        // Ensure that RoomController exists
        if (RoomController.instance == null)
        {
            Debug.Log("RoomController.instance is null");
            return;
        }

        // Add doors to the list
        Door[] ds = GetComponentsInChildren<Door>();
        foreach (Door d in ds)
        {
            doors.Add(d);
            switch (d.doorDir)
            {
                case Door.DoorDir.up:
                    topDoor = d;
                    break;
                case Door.DoorDir.down:
                    bottomDoor = d;
                    break;
                case Door.DoorDir.left:
                    leftDoor = d;
                    break;
                case Door.DoorDir.right:
                    rightDoor = d;
                    break;
            }
        }

        // Add this room to the list of loaded rooms
        RoomController.instance.RegisterRoom(this);
    }

    void Update()
    {
        // if (!updatedDoors)
        // {
        //     RemoveUnusedDoors();
        //     updatedDoors = true;
        // }
    }

    public void RemoveUnusedDoors()
    {
        // Remove unused doors
        foreach (Door door in doors)
        {
            switch (door.doorDir)
            {
                case Door.DoorDir.up:
                    if (GetRoom("u") == null)
                    {
                        // Disable the door
                        door.gameObject.transform.Find("Door").gameObject.SetActive(false);

                        // if (GetRoom("d") != null)
                        // {
                        // Enable the wall
                        door.gameObject.transform.Find("Wall").gameObject.SetActive(true);
                        // }
                    }
                    else
                    {
                        if (GetRoom("u").name.Contains("ItemRoom"))
                        {
                            Debug.Log("Epic");
                            door.gameObject.transform
                                .Find("Door")
                                .GetComponent<SpriteRenderer>()
                                .color = Color.yellow;
                            door.GetComponent<Door>().keycardLocked = true;
                        }
                        else if (GetRoom("u").name.Contains("End"))
                        {
                            Debug.Log("Epic");
                            door.gameObject.transform
                                .Find("Door")
                                .GetComponent<SpriteRenderer>()
                                .color = Color.black;
                        }
                    }
                    break;
                case Door.DoorDir.down:
                    if (GetRoom("d") == null)
                    {
                        // Disable the door
                        door.gameObject.transform.Find("Door").gameObject.SetActive(false);

                        // if (GetRoom("u") != null)
                        // {
                        // Enable the wall
                        door.gameObject.transform.Find("Wall").gameObject.SetActive(true);
                        // }
                    }
                    else
                    {
                        if (GetRoom("d").name.Contains("ItemRoom"))
                        {
                            Debug.Log("Epic");
                            door.gameObject.transform
                                .Find("Door")
                                .GetComponent<SpriteRenderer>()
                                .color = Color.yellow;
                            door.GetComponent<Door>().keycardLocked = true;
                        }
                        else if (GetRoom("d").name.Contains("End"))
                        {
                            Debug.Log("Epic");
                            door.gameObject.transform
                                .Find("Door")
                                .GetComponent<SpriteRenderer>()
                                .color = Color.black;
                        }
                    }
                    break;
                case Door.DoorDir.left:
                    if (GetRoom("l") == null)
                    {
                        // Disable the door
                        door.gameObject.transform.Find("Door").gameObject.SetActive(false);

                        // if (GetRoom("r") != null)
                        // {
                        // Enable the wall
                        door.gameObject.transform.Find("Wall").gameObject.SetActive(true);
                        // }
                    }
                    else
                    {
                        if (GetRoom("l").name.Contains("ItemRoom"))
                        {
                            Debug.Log("Epic");
                            door.gameObject.transform
                                .Find("Door")
                                .GetComponent<SpriteRenderer>()
                                .color = Color.yellow;
                            door.GetComponent<Door>().keycardLocked = true;
                        }
                        else if (GetRoom("l").name.Contains("End"))
                        {
                            Debug.Log("Epic");
                            door.gameObject.transform
                                .Find("Door")
                                .GetComponent<SpriteRenderer>()
                                .color = Color.black;
                        }
                    }
                    break;
                case Door.DoorDir.right:
                    if (GetRoom("r") == null)
                    {
                        // Disable the door
                        door.gameObject.transform.Find("Door").gameObject.SetActive(false);

                        // if (GetRoom("l") != null)
                        // {
                        // Enable the wall
                        door.gameObject.transform.Find("Wall").gameObject.SetActive(true);
                        // }
                    }
                    else
                    {
                        if (GetRoom("r").name.Contains("ItemRoom"))
                        {
                            Debug.Log("Epic");
                            door.gameObject.transform
                                .Find("Door")
                                .GetComponent<SpriteRenderer>()
                                .color = Color.yellow;
                            door.GetComponent<Door>().keycardLocked = true;
                        }
                        else if (GetRoom("r").name.Contains("End"))
                        {
                            Debug.Log("Epic");
                            door.gameObject.transform
                                .Find("Door")
                                .GetComponent<SpriteRenderer>()
                                .color = Color.black;
                        }
                    }
                    break;
            }
        }
    }

    public Room GetRoom(string dir)
    {
        if (dir == "u")
        {
            if (RoomController.instance.DoesRoomExist(X, Y + 1))
            {
                return RoomController.instance.FindRoom(X, Y + 1);
            }
            return null;
        }
        else if (dir == "d")
        {
            if (RoomController.instance.DoesRoomExist(X, Y - 1))
            {
                return RoomController.instance.FindRoom(X, Y - 1);
            }
            return null;
        }
        else if (dir == "l")
        {
            if (RoomController.instance.DoesRoomExist(X - 1, Y))
            {
                return RoomController.instance.FindRoom(X - 1, Y);
            }
            return null;
        }
        else if (dir == "r")
        {
            if (RoomController.instance.DoesRoomExist(X + 1, Y))
            {
                return RoomController.instance.FindRoom(X + 1, Y);
            }
            return null;
        }
        else
        {
            return null;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(
            GetRoomCenter() - new Vector3(0.5f, 0.5f, 0),
            new Vector3(roomW, roomH, 0)
        );
    }

    public Vector3 GetRoomCenter()
    {
        return new Vector3(X * roomW, Y * roomH, 0);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            RoomController.instance.OnPlayerEnterRoom(this);
        }
    }
}
