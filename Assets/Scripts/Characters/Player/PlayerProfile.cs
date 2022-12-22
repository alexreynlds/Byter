using System.Collections;
using UnityEngine;

public class PlayerProfile : MonoBehaviour, IDataPersistence
{
    public string playerName;

    public int currency;

    public int experience;

    private void Start()
    {
        currency = 0;
    }

    public void OnTest()
    {
        currency += 100;
    }

    public void SaveData(ref GameData data)
    {
        data.currency = currency;
    }

    public void LoadData(GameData data)
    {
        currency = data.currency;
    }
}
