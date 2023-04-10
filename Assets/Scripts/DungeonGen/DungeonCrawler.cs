using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonCrawler
{
    public Vector2Int position { get; set; }

    public DungeonCrawler(Vector2Int startPos)
    {
        position = startPos;
    }

    public Vector2Int
    move(Dictionary<Direction, Vector2Int> directionMovementMap)
    {
        Direction moveDir =
            (Direction) Random.Range(0, directionMovementMap.Count);
        position += directionMovementMap[moveDir];
        return position;
    }
}
