using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int currency;

    public int gameLevel;

    public Vector2 playerPosition;

    public GameData()
    {
        gameLevel = 0;
        playerPosition = Vector2.zero;
        currency = 0;
    }
}
