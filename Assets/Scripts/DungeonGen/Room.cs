using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public float Width;

    public float Height;

    public int X;

    public int Y;

    public Door leftdoor;

    public Door rightdoor;

    public Door topdoor;

    public Door bottomdoor;

    public List<Door> doors = new List<Door>();

    // Start is called before the first frame update
    void Start()
    {
        if (RoomController.instance == null)
        {
            Debug.Log("RoomController.instance is null");
            return;
        }

        Door[] ds = GetComponentsInChildren<Door>();
        foreach (Door d in ds)
        {
            doors.Add (d);
            switch (d.doorType)
            {
                case Door.DoorType.top:
                    topdoor = d;
                    break;
                case Door.DoorType.bottom:
                    bottomdoor = d;
                    break;
                case Door.DoorType.left:
                    leftdoor = d;
                    break;
                case Door.DoorType.right:
                    rightdoor = d;
                    break;
            }
        }

        RoomController.instance.RegisterRoom(this);
    }

    public void RemoveUnconnectedDoors()
    {
        foreach (Door door in doors)
        {
            switch (door.doorType)
            {
                case Door.DoorType.top:
                    if (GetTop() == null)
                    {
                        door.gameObject.SetActive(false);
                    }
                    break;
                case Door.DoorType.bottom:
                    if (GetBottom() == null)
                    {
                        door.gameObject.SetActive(false);
                    }
                    break;
                case Door.DoorType.left:
                    if (GetLeft() == null)
                    {
                        door.gameObject.SetActive(false);
                    }
                    break;
                case Door.DoorType.right:
                    if (GetRight() == null)
                    {
                        door.gameObject.SetActive(false);
                    }
                    break;
            }
        }
    }

    public Room GetRight()
    {
        if (RoomController.instance.DoesRoomExist(X + 1, Y))
        {
            return RoomController.instance.FindRoom(X + 1, Y);
        }
        return null;
    }

    public Room GetLeft()
    {
        if (RoomController.instance.DoesRoomExist(X - 1, Y))
        {
            return RoomController.instance.FindRoom(X - 1, Y);
        }
        return null;
    }

    public Room GetTop()
    {
        if (RoomController.instance.DoesRoomExist(X, Y + 1))
        {
            return RoomController.instance.FindRoom(X, Y + 1);
        }
        return null;
    }

    public Room GetBottom()
    {
        if (RoomController.instance.DoesRoomExist(X, Y - 1))
        {
            return RoomController.instance.FindRoom(X, Y - 1);
        }
        return null;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(GetRoomCenter(), new Vector3(Width, Height, 0));
    }

    public Vector3 GetRoomCenter()
    {
        return new Vector3(X * Width, Y * Height, 0);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            CameraController.instance.currentRoom = this;
        }
    }
}
