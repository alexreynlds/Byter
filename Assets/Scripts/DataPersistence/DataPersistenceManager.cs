using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DataPersistenceManager : MonoBehaviour
{
    [Header("File Sotrage Config")]
    [SerializeField]
    private string fileName;

    [SerializeField]
    private bool useEncryption;

    public static DataPersistenceManager instance { get; private set; }

    private GameData gameData;

    private List<IDataPersistence> dataPersistenceObjects;

    private FileDataHandler dataHandler;

    private void Awake()
    {
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad (gameObject);
            }
            else
            {
                Destroy (gameObject);
            }
        }
    }

    private void Start()
    {
        this.dataHandler =
            new FileDataHandler(Application.persistentDataPath,
                fileName,
                useEncryption);

        dataPersistenceObjects = FindAllDataPersistenceObjects();

        if (GameObject.Find("GameManager") != null)
        {
            if (
                GameObject
                    .Find("GameManager")
                    .GetComponent<GameManager>()
                    .Loaded ==
                false
            )
            {
                NewGame();
            }
            else
            {
                LoadGame();
            }
        }
        else
        {
            NewGame();
        }
        // NewGame();
        // LoadGame();
    }

    public void NewGame()
    {
        this.gameData = new GameData();
    }

    public void LoadGame()
    {
        this.gameData = this.dataHandler.Load();
        if (this.gameData == null)
        {
            NewGame();
        }

        foreach (IDataPersistence dataPersistence in dataPersistenceObjects)
        {
            dataPersistence.LoadData(this.gameData);
        }
    }

    public void SaveGame()
    {
        foreach (IDataPersistence dataPersistence in dataPersistenceObjects)
        {
            dataPersistence.SaveData(ref this.gameData);
        }
        dataHandler.Save (gameData);
    }

    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistenceList =
            FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();
        return new List<IDataPersistence>(dataPersistenceList);
    }

    // private void OnApplicationQuit()
    // {
    //     SaveGame();
    // }

    // void OnLevelWasLoaded(int level)
    // {
    //     Debug.Log("swag");
    //     if (instance == null)
    //     {
    //         instance = this;
    //         DontDestroyOnLoad (gameObject);
    //     }
    //     else
    //     {
    //         Destroy (gameObject);
    //     }

    //     this.dataHandler =
    //         new FileDataHandler(Application.persistentDataPath,
    //             fileName,
    //             useEncryption);

    //     dataPersistenceObjects = FindAllDataPersistenceObjects();

    //     this.gameData = this.dataHandler.Load();
    //     if (this.gameData == null)
    //     {
    //         NewGame();
    //     }
    // }
}
