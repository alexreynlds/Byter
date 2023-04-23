using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class RoomInfo
{
    public string name;

    public int X;

    public int Y;
}

public class RoomController : MonoBehaviour
{
    public static RoomController instance;

    string currentLevelName = "Floor1";

    RoomInfo currentLoadRoomData;

    Room currentRoom;

    Queue<RoomInfo> loadRoomQueue = new Queue<RoomInfo>();

    public List<Room> loadedRooms = new List<Room>();

    bool isLoadingRoom = false;

    bool spawnedBossRoom = false;

    bool finishedLoading = false;


    // bool spawnedShopRoom = false;
    bool updatedRooms = false;

    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        UpdateRoomQueue();
    }

    void UpdateRoomQueue()
    {
        if (isLoadingRoom) return;
        if (loadRoomQueue.Count == 0)
        {
            // Debug.Log("epic");
            if (!spawnedBossRoom)
            {
                StartCoroutine(SpawnBossRoom());
            }

            else if (spawnedBossRoom && !updatedRooms)
            {
                foreach (Room room in loadedRooms)
                {
                    room.RemoveUnusedDoors();
                }
                UpdateRooms();
                updatedRooms = true;

                GameObject.Find("Player").GetComponent<PlayerStats>().wipeInventory();
                GameObject.Find("Player").GetComponent<PlayerInput>().enabled = true;
            }
            return;
        }

        currentLoadRoomData = loadRoomQueue.Dequeue();
        isLoadingRoom = true;
        StartCoroutine(LoadRoomRoutine(currentLoadRoomData));
        // finishedLoading = true;
    }

    IEnumerator SpawnBossRoom()
    {
        spawnedBossRoom = true;
        yield return new WaitForSeconds(0.5f);
        if (loadRoomQueue.Count == 0)
        {
            Room bossRoom = loadedRooms[loadedRooms.Count - 1];
            Vector2Int tempRoom = new Vector2Int(bossRoom.X, bossRoom.Y);
            Destroy(bossRoom.gameObject);
            var roomToRemove =
                loadedRooms.Single(r => r.X == tempRoom.x && r.Y == tempRoom.y);
            loadedRooms.Remove(roomToRemove);
            LoadRoom("End", tempRoom.x, tempRoom.y);
        }
    }

    public void LoadRoom(string name, int x, int y)
    {
        // Check if room already exists
        if (DoesRoomExist(x, y)) return;

        RoomInfo newRoomData = new RoomInfo();
        newRoomData.name = name;
        newRoomData.X = x;
        newRoomData.Y = y;

        loadRoomQueue.Enqueue(newRoomData);
    }

    IEnumerator LoadRoomRoutine(RoomInfo roomInfo)
    {
        string roomName = currentLevelName + roomInfo.name;

        AsyncOperation loadRoom =
            SceneManager.LoadSceneAsync(roomName, LoadSceneMode.Additive);

        while (loadRoom.isDone == false)
        {
            yield return null;
        }
    }

    public void RegisterRoom(Room room)
    {
        if (!DoesRoomExist(currentLoadRoomData.X, currentLoadRoomData.Y))
        {
            room.transform.position =
                new Vector3(currentLoadRoomData.X * room.roomW,
                    currentLoadRoomData.Y * room.roomH,
                    0);

            room.X = currentLoadRoomData.X;
            room.Y = currentLoadRoomData.Y;
            room.name =
                currentLevelName +
                "_" +
                currentLoadRoomData.name +
                " " +
                room.X +
                ", " +
                room.Y;
            room.transform.parent = this.transform;

            isLoadingRoom = false;

            if (loadedRooms.Count == 0)
            {
                CameraController.instance.currentRoom = room;
            }

            loadedRooms.Add(room);
            // room.RemoveUnusedDoors();
        }
        else
        {
            Destroy(room.gameObject);
            isLoadingRoom = false;
        }
    }

    public bool DoesRoomExist(int x, int y)
    {
        return loadedRooms.Find(item => item.X == x && item.Y == y) != null;
    }

    public Room FindRoom(int x, int y)
    {
        return loadedRooms.Find(item => item.X == x && item.Y == y);
    }

    public string GetRandomRoomName()
    {
        string[] possibleRooms = new string[] { "Empty", "Basic1" };

        return possibleRooms[Random.Range(0, possibleRooms.Length)];
    }

    public void OnPlayerEnterRoom(Room room)
    {
        CameraController.instance.currentRoom = room;
        currentRoom = room;

        StartCoroutine(RoomCoroutine());
    }

    public IEnumerator RoomCoroutine()
    {
        yield return new WaitForSeconds(0.2f);
        UpdateRooms();
    }

    public void UpdateRooms()
    {
        foreach (Room room in loadedRooms)
        {
            if (room != currentRoom)
            {
                EnemyController[] enemies = room.GetComponentsInChildren<EnemyController>();

                if (enemies != null)
                {
                    foreach (EnemyController enemy in enemies)
                    {
                        enemy.notInRoom = true;
                    }

                    foreach (Door door in room.GetComponentsInChildren<Door>())
                    {
                        door.Open();
                    }
                }
                else
                {
                    foreach (Door door in room.GetComponentsInChildren<Door>())
                    {
                        door.Open();
                    }
                }
            }
            else
            {
                EnemyController[] enemies = room.GetComponentsInChildren<EnemyController>();

                if (enemies.Length > 0)
                {
                    foreach (EnemyController enemy in enemies)
                    {
                        enemy.notInRoom = false;
                    }

                    foreach (Door door in room.GetComponentsInChildren<Door>())
                    {
                        door.Close();
                    }
                }
                else
                {
                    foreach (Door door in room.GetComponentsInChildren<Door>())
                    {
                        door.Open();
                    }
                }
            }
        }
    }
}
