using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    up = 0,
    right = 1,
    down = 2,
    left = 3
}

// Data for generating a dungeon
public class DungeonCrawlerController : MonoBehaviour
{
    // List of positions visited by crawlers
    public static List<Vector2Int> positionsVisited = new List<Vector2Int>();

    // Map of directions to movement vectors
    private static readonly Dictionary<Direction, Vector2Int>
        directionMovementMap =
            new Dictionary<Direction, Vector2Int> {
                { Direction.up, new Vector2Int(0, 1) },
                { Direction.right, new Vector2Int(1, 0) },
                { Direction.down, new Vector2Int(0, -1) },
                { Direction.left, new Vector2Int(-1, 0) }
            };

    // Generate a dungeon
    public static List<Vector2Int>
    GenerateDungeon(DungeonGenerationData dungeonData)
    {
        // Create a list of crawlers
        List<DungeonCrawler> crawlers = new List<DungeonCrawler>();

        for (int i = 0; i < dungeonData.numOfCrawlers; i++)
        {
            crawlers.Add(new DungeonCrawler(Vector2Int.zero));
        }

        int iterations =
            Random.Range(dungeonData.iterationMin, dungeonData.iterationMax);

        // Move the crawlers
        for (int i = 0; i < iterations; i++)
        {
            // Debug.
            foreach (DungeonCrawler crawler in crawlers)
            {
                Vector2Int newPos = crawler.move(directionMovementMap);
                positionsVisited.Add(newPos);
            }
        }

        return positionsVisited;
    }
}
