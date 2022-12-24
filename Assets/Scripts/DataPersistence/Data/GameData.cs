using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int currency;

    public Vector2 playerPosition;

    public GameData()
    {
        playerPosition = Vector2.zero;
        currency = 0;
    }
}
