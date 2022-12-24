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

    // public static DataPersistenceManager instance { get; private set; }
    private GameData gameData;

    private List<IDataPersistence> dataPersistenceObjects;

    private FileDataHandler dataHandler;

    private void Awake()
    {
        {
        }
    }

    private void Start()
    {
        this.dataHandler =
            new FileDataHandler(Application.persistentDataPath,
                fileName,
                useEncryption);

        dataPersistenceObjects = FindAllDataPersistenceObjects();
        LoadGame();
        // NewGame();
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
        // Debug.Log (dataPersistenceObjects);
        foreach (IDataPersistence dataPersistence in dataPersistenceObjects)
        {
            dataPersistence.SaveData(ref this.gameData);
        }
        dataHandler.Save (gameData);
    }

    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects =
            FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();

        foreach (IDataPersistence dataPersistence in dataPersistenceObjects)
        {
            Debug.Log(dataPersistence.ToString());
        }

        return new List<IDataPersistence>(dataPersistenceObjects);
    }
}
