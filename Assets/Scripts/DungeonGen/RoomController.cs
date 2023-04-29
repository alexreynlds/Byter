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

    // Special Rooms
    bool spawnedBossRoom = false;
    bool spawnedShopRoom = false;
    bool spawnedItemRoom = false;

    private Vector2 u = new Vector2(0, 1);
    private Vector2 d = new Vector2(0, -1);
    private Vector2 l = new Vector2(-1, 0);
    private Vector2 r = new Vector2(1, 0);

    private List<Vector2> directions;



    bool updatedRooms = false;

    void Awake()
    {
        instance = this;
        GameObject.Find("Player").GetComponent<PlayerInput>().enabled = false;
        directions = new List<Vector2>();
        directions.Add(u);
        directions.Add(d);
        directions.Add(l);
        directions.Add(r);
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

                StartCoroutine(test());
                UpdateRooms();
                updatedRooms = true;
            }
            return;
        }

        currentLoadRoomData = loadRoomQueue.Dequeue();
        isLoadingRoom = true;
        StartCoroutine(LoadRoomRoutine(currentLoadRoomData));
    }

    IEnumerator test()
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
        if (loadRoomQueue.Count == 0)
        {
            int i = loadedRooms.Count - 1;
            int newX,
                newY;
            bool roomFound = false;
            Room bossRoom = loadedRooms[i];
            newX = bossRoom.X;
            newY = bossRoom.Y;
            while (!roomFound)
            {
                int tempX = Mathf.Abs(newX);
                int tempY = Mathf.Abs(newY);
                if (tempX > tempY)
                {
                    if (newX > 0)
                    {
                        newX++;
                    }
                    else
                    {
                        newX--;
                    }
                }
                else
                {
                    if (newY > 0)
                    {
                        newY++;
                    }
                    else
                    {
                        newY--;
                    }
                }
                if (!DoesRoomExist(newX, newY))
                {
                    roomFound = true;
                }
            }
            LoadRoom("End", newX, newY);
        }
    }

    IEnumerator SpawnShopRoom()
    {
        spawnedShopRoom = true;
        yield return new WaitForSeconds(0.7f);
        FindEmptyRoomSpace("Shop");
        // int newX,
        //     newY;
        // bool roomFound = false;
        // Room shopRoom = loadedRooms[Random.Range(0, loadedRooms.Count - 3)];

        // newX = shopRoom.X;
        // newY = shopRoom.Y;

        // while (!roomFound)
        // {
        //     int tempX = Mathf.Abs(newX);
        //     int tempY = Mathf.Abs(newY);
        //     if (tempX > tempY)
        //     {
        //         if (newX > 0)
        //         {
        //             newX++;
        //         }
        //         else
        //         {
        //             newX--;
        //         }
        //     }
        //     else
        //     {
        //         if (newY > 0)
        //         {
        //             newY++;
        //         }
        //         else
        //         {
        //             newY--;
        //         }
        //     }
        //     if (!DoesRoomExist(newX, newY))
        //     {
        //         roomFound = true;
        //     }
        // }
        // LoadRoom("Shop", newX, newY);
    }

    IEnumerator SpawnItemRoom()
    {
        spawnedItemRoom = true;
        yield return new WaitForSeconds(0.6f);
        FindEmptyRoomSpace("ItemRoom");

        // Room itemRoom = loadedRooms[Random.Range(0, loadedRooms.Count - 2)];
        // int newX,
        //     newY;
        // bool roomFound = false;
        // newX = itemRoom.X;
        // newY = itemRoom.Y;
        // while (!roomFound)
        // {
        //     int tempX = Mathf.Abs(newX);
        //     int tempY = Mathf.Abs(newY);
        //     if (tempX > tempY)
        //     {
        //         if (newX > 0)
        //         {
        //             newX++;
        //         }
        //         else
        //         {
        //             newX--;
        //         }
        //     }
        //     else
        //     {
        //         if (newY > 0)
        //         {
        //             newY++;
        //         }
        //         else
        //         {
        //             newY--;
        //         }
        //     }
        //     if (!DoesRoomExist(newX, newY))
        //     {
        //         roomFound = true;
        //     }
        // }
        // LoadRoom("ItemRoom", newX, newY);
    }

    public void FindEmptyRoomSpace(string roomType)
    {
        bool roomFound = false;
        int newX = 0;
        int newY = 0;
        int maxIterations = 50;

        while (!roomFound && maxIterations > 0)
        {
            Room randomRoom = loadedRooms[Random.Range(0, loadedRooms.Count - 1)];
            newX = randomRoom.X;
            newY = randomRoom.Y;

            Debug.Log(directions.Count);

            foreach (Vector2 direction in directions)
            {
                List<Vector2> tempDirections = new List<Vector2>(directions);
                Vector2 tempDirection = tempDirections[Random.Range(0, tempDirections.Count)];

                Debug.Log(tempDirection);
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
            Debug.Log("Found empty room space. Iterations left: " + maxIterations + ".");
            LoadRoom(roomType, newX, newY);
        }
    }

    public void LoadRoom(string name, int x, int y)
    {
        // Check if room already exists
        if (DoesRoomExist(x, y))
            return;

        RoomInfo newRoomData = new RoomInfo();
        newRoomData.name = name;
        newRoomData.X = x;
        newRoomData.Y = y;

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
            room.name =
                currentLevelName + "_" + currentLoadRoomData.name + " " + room.X + ", " + room.Y;
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

    public void spawnItem(GameObject item, Vector3 position)
    {
        Instantiate(item, position, Quaternion.identity);
    }
}
