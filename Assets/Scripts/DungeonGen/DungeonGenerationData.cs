using UnityEngine;

[
    CreateAssetMenu(
        fileName = "DungeonGenerationData.asset",
        menuName = "Dungeon Generation Data")
]
public class DungeonGenerationData : ScriptableObject
{
    public int numOfCrawlers;

    public int iterationMax;

    public int iterationMin;
}
