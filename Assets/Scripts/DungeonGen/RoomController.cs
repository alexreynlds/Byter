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
    // public static RoomController instance;

    // string currentLevelName = "Floor1";

    // RoomInfo currentLoadRoomData;

    // Room currentRoom;

    // Queue<RoomInfo> loadRoomQueue = new Queue<RoomInfo>();

    // public List<Room> loadedRooms = new List<Room>();

    // bool isLoadingRoom = false;

    // // Special Rooms
    // bool spawnedBossRoom = false;
    // bool spawnedShopRoom = false;
    // bool spawnedItemRoom = false;

    // private Vector2 u = new Vector2(0, 1);
    // private Vector2 d = new Vector2(0, -1);
    // private Vector2 l = new Vector2(-1, 0);
    // private Vector2 r = new Vector2(1, 0);

    // private List<Vector2> directions;



    // bool updatedRooms = false;

    public static RoomController instance;
    private string currentLevelName = "Floor1";
    private RoomInfo currentLoadRoomData;
    private Room currentRoom;
    private Queue<RoomInfo> loadRoomQueue = new Queue<RoomInfo>();
    private List<Room> loadedRooms = new List<Room>();
    private bool isLoadingRoom = false;
    private bool spawnedBossRoom = false;
    private bool spawnedShopRoom = false;
    private bool spawnedItemRoom = false;
    private List<Vector2> directions = new List<Vector2> { new Vector2(0, 1), new Vector2(0, -1), new Vector2(-1, 0), new Vector2(1, 0) };
    private bool updatedRooms = false;

    void Awake()
    {
        instance = this;
        GameObject.Find("Player").GetComponent<PlayerInput>().enabled = false;
    }

    void Update()
    {
        UpdateRoomQueue();
    }

    void UpdateRoomQueue()
    {
        if (isLoadingRoom)
            return;
        if (loadRoomQueue.Count == 0)
        {
            if (!spawnedBossRoom)
            {
                StartCoroutine(SpawnBossRoom());
            }

            if (!spawnedShopRoom)
            {
                StartCoroutine(SpawnShopRoom());
            }

            if (!spawnedItemRoom)
            {
                StartCoroutine(SpawnItemRoom());
            }
            else if (spawnedBossRoom && !updatedRooms && spawnedShopRoom && spawnedItemRoom)
            {

                StartCoroutine(FinishUp());
                UpdateRooms();
                updatedRooms = true;
            }
            return;
        }

        currentLoadRoomData = loadRoomQueue.Dequeue();
        isLoadingRoom = true;
        StartCoroutine(LoadRoomRoutine(currentLoadRoomData));
    }

    IEnumerator FinishUp()
    {
        yield return new WaitForSeconds(1.0f);
        foreach (Room room in loadedRooms)
        {
            room.RemoveUnusedDoors();

        }
        GameObject.Find("Player").GetComponent<PlayerStats>().wipeInventory();
        GameObject.Find("Player").GetComponent<PlayerInput>().enabled = true;
    }

    IEnumerator SpawnBossRoom()
    {
        spawnedBossRoom = true;
        yield return new WaitForSeconds(0.5f);
        string bossRoom = "End" + Random.Range(0, 1).ToString();
        // Debug.Log(bossRoom);
        SpawnSpecialRoom(bossRoom);
    }

    IEnumerator SpawnShopRoom()
    {
        spawnedShopRoom = true;
        yield return new WaitForSeconds(0.7f);
        SpawnSpecialRoom("Shop");
    }

    IEnumerator SpawnItemRoom()
    {
        spawnedItemRoom = true;
        yield return new WaitForSeconds(0.6f);
        SpawnSpecialRoom("ItemRoom");
    }

    public void SpawnSpecialRoom(string roomType)
    {
        bool roomFound = false;
        int newX = 0;
        int newY = 0;
        int maxIterations = 50;
        int minimum = 0;

        if (roomType == "end")
        {
            minimum = loadedRooms.Count - 1;
        }

        while (!roomFound && maxIterations > 0)
        {
            Room randomRoom = loadedRooms[Random.Range(0, loadedRooms.Count - 1)];

            while (randomRoom.name.Contains("Shop") || randomRoom.name.Contains("ItemRoom"))
            {
                randomRoom = loadedRooms[Random.Range(0, loadedRooms.Count - 1)];
            }

            while (randomRoom.name.Contains("End"))
            {
                randomRoom = loadedRooms[Random.Range(loadedRooms.Count / 2, loadedRooms.Count - 1)];
            }

            newX = randomRoom.X;
            newY = randomRoom.Y;

            foreach (Vector2 direction in directions)
            {
                List<Vector2> tempDirections = new List<Vector2>(directions);
                Vector2 tempDirection = tempDirections[Random.Range(0, tempDirections.Count)];

                int checkX = newX + (int)tempDirection.x;
                int checkY = newY + (int)tempDirection.y;

                if (!DoesRoomExist(checkX, checkY))
                {
                    newX = checkX;
                    newY = checkY;
                    roomFound = true;
                    break;
                }

                tempDirections.Remove(tempDirection);
            }

            maxIterations--;
        }

        if (roomFound)
        {
            LoadRoom(roomType, newX, newY);
        }
    }


    public void ItemRoomUnlockDoors()
    {
        Room itemRoom = loadedRooms.FirstOrDefault(room => room.name.Contains("ItemRoom"));
        if (itemRoom == null) return;

        int x = itemRoom.X;
        int y = itemRoom.Y;

        if (DoesRoomExist(x, y + 1))
        {
            loadedRooms.FirstOrDefault(room => room.X == x && room.Y == y + 1)?.UnlockDoors();
        }
        if (DoesRoomExist(x, y - 1))
        {
            loadedRooms.FirstOrDefault(room => room.X == x && room.Y == y - 1)?.UnlockDoors();
        }
        if (DoesRoomExist(x - 1, y))
        {
            loadedRooms.FirstOrDefault(room => room.X == x - 1 && room.Y == y)?.UnlockDoors();
        }
        if (DoesRoomExist(x + 1, y))
        {
            loadedRooms.FirstOrDefault(room => room.X == x + 1 && room.Y == y)?.UnlockDoors();
        }
    }

    public void LoadRoom(string name, int x, int y)
    {
        if (DoesRoomExist(x, y))
        {
            return;
        }

        RoomInfo newRoomData = new RoomInfo
        {
            name = name,
            X = x,
            Y = y
        };

        loadRoomQueue.Enqueue(newRoomData);
    }

    IEnumerator LoadRoomRoutine(RoomInfo roomInfo)
    {
        string roomName = currentLevelName + roomInfo.name;
        AsyncOperation loadRoom = SceneManager.LoadSceneAsync(roomName, LoadSceneMode.Additive);

        while (loadRoom.isDone == false)
        {
            yield return null;
        }
    }

    public void RegisterRoom(Room room)
    {
        if (!DoesRoomExist(currentLoadRoomData.X, currentLoadRoomData.Y))
        {
            room.transform.position = new Vector3(
                currentLoadRoomData.X * room.roomW,
                currentLoadRoomData.Y * room.roomH,
                0
            );

            room.X = currentLoadRoomData.X;
            room.Y = currentLoadRoomData.Y;
            room.name = $"{currentLevelName}_{currentLoadRoomData.name} {room.X}, {room.Y}";
            room.transform.parent = transform;

            isLoadingRoom = false;

            if (loadedRooms.Count == 0)
            {
                CameraController.instance.currentRoom = room;
            }

            loadedRooms.Add(room);
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
            EnemyController[] enemies = room.GetComponentsInChildren<EnemyController>();

            if (room != currentRoom)
            {
                foreach (EnemyController enemy in enemies)
                {
                    enemy.notInRoom = true;
                }
            }

            foreach (Door door in room.GetComponentsInChildren<Door>())
            {
                door.Open();
                door.UpdateDoorData();
            }

            if (room == currentRoom && enemies.Length > 0)
            {
                Debug.Log("Enemies in roomLL: " + enemies.Length);
                foreach (EnemyController enemy in enemies)
                {
                    enemy.notInRoom = false;
                }

                foreach (Door door in room.GetComponentsInChildren<Door>())
                {
                    door.Close();
                    door.UpdateDoorData();
                }
            }
        }
    }


    public void spawnItem(GameObject item, Vector3 position)
    {
        Instantiate(item, position, Quaternion.identity);
    }
}
