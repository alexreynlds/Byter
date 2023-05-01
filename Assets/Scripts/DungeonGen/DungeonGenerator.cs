using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DungeonGenerator : MonoBehaviour
{
    public DungeonGenerationData dungeonData;

    private List<Vector2Int> dungeonRooms;

    private void Awake()
    {
        Random.InitState(System.DateTime.Now.Millisecond);

        if (dungeonData == null)
        {
            dungeonData = Resources.Load<DungeonGenerationData>("DungeonGenerationData");
            dungeonRooms = DungeonCrawlerController.GenerateDungeon(dungeonData);
            SpawnRooms(dungeonRooms);
        }
        else
        {
            dungeonData = Resources.Load<DungeonGenerationData>("DungeonGenerationData");
            dungeonRooms = DungeonCrawlerController.GenerateDungeon(dungeonData);
            SpawnRooms(dungeonRooms);
        }

    }

    private void SpawnRooms(IEnumerable<Vector2Int> rooms)
    {
        RoomController.instance.LoadRoom("Start", 0, 0);

        foreach (Vector2Int roomLocation in rooms)
        {
            RoomController.instance.LoadRoom(
                RoomController.instance.GetRandomRoomName(),
                roomLocation.x,
                roomLocation.y
            );
        }
    }

    // public void RegenDungeon()
    // {
    //     this.gameObject.GetComponent<RoomController>().enabled = false;

    //     for (int i = 0; i < RoomController.instance.loadedRooms.Count; i++)
    //     {
    //         string x = RoomController.instance.loadedRooms[i].name;
    //         string[] roomName = x.Split('_');
    //         x = roomName[0] + roomName[1];
    //         x = x.Substring(0, x.Length - 5);
    //         Debug.Log(x);
    //         Destroy(RoomController.instance.loadedRooms[i].gameObject);
    //         SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName(x));
    //     }

    //     // SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName("Floor1Start"));

    //     RoomController.instance.loadedRooms.Clear();
    //     dungeonRooms = DungeonCrawlerController.GenerateDungeon(dungeonData);
    //     SpawnRooms(dungeonRooms);
    // }
}
