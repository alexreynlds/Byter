using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    top = 0,
    bottom = 1,
    left = 2,
    right = 3
}

public class DungeonCrawlerController : MonoBehaviour
{
    public static List<Vector2Int> positionsVisited = new List<Vector2Int>();

    private static readonly Dictionary<Direction, Vector2Int>
        directionMovementMap =
            new Dictionary<Direction, Vector2Int>()
            {
                { Direction.top, new Vector2Int(0, 1) },
                { Direction.bottom, new Vector2Int(0, -1) },
                { Direction.left, new Vector2Int(-1, 0) },
                { Direction.right, new Vector2Int(1, 0) }
            };

    public static List<Vector2Int>
    GenerateDungeon(DungeonGenerationData dungeonData)
    {
        List<DungeonCrawler> dungeonCrawlers = new List<DungeonCrawler>();

        for (int i = 0; i < dungeonData.numberOfCrawlers; i++)
        {
            dungeonCrawlers.Add(new DungeonCrawler(Vector2Int.zero));
        }

        int iterations =
            Random.Range(dungeonData.iterationMin, dungeonData.iterationMax);

        for (int i = 0; i < iterations; i++)
        {
            foreach (DungeonCrawler dungeonCrawler in dungeonCrawlers)
            {
                Vector2Int newPosition =
                    dungeonCrawler.Move(directionMovementMap);
                if (!positionsVisited.Contains(newPosition))
                {
                    positionsVisited.Add (newPosition);
                }
            }
        }

        return positionsVisited;
    }
}
