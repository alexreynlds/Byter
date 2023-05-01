using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRoomSpawner : MonoBehaviour
{
    [System.Serializable]
    public struct RandomSpawner
    {
        public string name;

        public SpawnerData spawnerData;
    }

    public GridController grid;

    public RandomSpawner[] spawners;

    void Start()
    {
        // grid = GetComponentInChildren<GridController>();
    }

    public void InitialiseObjectSpawning()
    {
        foreach (RandomSpawner data in spawners)
        {
            SpawnObjects(data);
        }
    }

    void SpawnObjects(RandomSpawner data)
    {
        if (grid.availablePositions.Count > 1)
        {


            int randomIteration =
                Random
                    .Range(data.spawnerData.minSpawn,
                    data.spawnerData.maxSpawn + 1);

            for (int i = 0; i < randomIteration; i++)
            {
                int randomPos = Random.Range(0, grid.availablePositions.Count - 1);
                GameObject go =
                    Instantiate(data.spawnerData.itemToSpawn,
                    grid.availablePositions[randomPos],
                    Quaternion.identity,
                    transform) as
                    GameObject;

                grid.availablePositions.RemoveAt(randomPos);
            }
        }
        else
        {
            GameObject go =
                   Instantiate(data.spawnerData.itemToSpawn,
                   grid.availablePositions[0],
                   Quaternion.identity,
                   transform) as
                   GameObject;
        }
    }
}
