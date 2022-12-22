using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour, IDataPersistence
{
    public int gameLevel;

    public bool Loaded;

    public static GameManager instance { get; private set; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad (gameObject);
        }
        else if (instance != this)
        {
            Destroy (gameObject);
        }
    }

    public void SaveData(ref GameData data)
    {
        data.gameLevel = gameLevel;
    }

    public void LoadData(GameData data)
    {
        gameLevel = data.gameLevel;
    }
}
