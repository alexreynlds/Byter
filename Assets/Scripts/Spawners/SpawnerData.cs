using UnityEngine;

[
    CreateAssetMenu(
        fileName = "SpawnerData",
        menuName = "Spawners/Spawner",
        order = 0)
]
public class SpawnerData : ScriptableObject
{
    public GameObject itemToSpawn;

    public int minSpawn;

    public int maxSpawn;
}
