using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Spawnable
{
    public GameObject gameObject;

    public float weight;
}

public class ItemSpawner : MonoBehaviour
{
    // [System.Serializable]
    // public struct Spawnable
    // {
    //     public GameObject gameObject;

    //     public float weight;
    // }
    // [System.Serializable]
    public List<Spawnable> itemPool = new List<Spawnable>();

    public ItemPoolData itemPoolData;

    float totalWeight;

    void Awake()
    {
        totalWeight = 0;
        foreach (Spawnable spawnable in itemPool)
        {
            totalWeight += spawnable.weight;
        }
        // foreach (Spawnable spawnable in itemPoolData.itemPool)
        // {
        //     totalWeight += spawnable.weight;
        // }
    }

    void Start()
    {
        float pick = Random.Range(0, totalWeight);
        int chosenIndex = 0;
        float cumulativeWeight = itemPool[0].weight;

        while (pick > cumulativeWeight && chosenIndex < itemPool.Count - 1)
        {
            chosenIndex++;
            cumulativeWeight += itemPool[chosenIndex].weight;
        }

        // float pick = Random.Range(0, totalWeight);
        // int chosenIndex = 0;
        // float cumulativeWeight = itemPoolData.itemPool[0].weight;

        // while (pick > cumulativeWeight && chosenIndex < itemPoolData.itemPool.Count - 1)
        // {
        //     chosenIndex++;
        //     cumulativeWeight += itemPoolData.itemPool[chosenIndex].weight;
        // }
    }
}
