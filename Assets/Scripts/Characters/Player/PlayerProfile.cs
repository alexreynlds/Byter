using System.Collections;
using UnityEngine;

[System.Serializable]
public class PlayerProfile : MonoBehaviour
{
    public string playerName;

    public int currency;

    public int experience;

    private void Update()
    {
    }

    public void OnTest()
    {
        currency += 100;
    }

    public void OnSave()
    {
    }

    public void OnLoad()
    {
    }
}
