using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyBarScript : MonoBehaviour
{
    [SerializeField] private GameObject startEnergyContainer;
    [SerializeField] private GameObject endEnergyContainer;
    [SerializeField] private Sprite[] startEnergySprites;
    [SerializeField] private Sprite[] endEnergySprites;
    private int playerEnergy;
    List<GameObject> energySprites = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        playerEnergy = GameObject.Find("Player").GetComponent<PlayerStats>().currentEnergy;
        CreateEnergyBar();

    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.Find("Player").GetComponent<PlayerStats>().currentEnergy != playerEnergy)
        {
            playerEnergy = GameObject.Find("Player").GetComponent<PlayerStats>().currentEnergy;
            UpdateEnergyBar();
        }
    }

    void CreateEnergyBar()
    {
        GameObject startEnergy = Instantiate(startEnergyContainer, new Vector3(0, 0, 0), Quaternion.identity, transform);
        startEnergy.GetComponent<RectTransform>().anchoredPosition = new Vector3(-295, 110, 0);
        energySprites.Add(startEnergy);

        GameObject endEnergy = Instantiate(endEnergyContainer, new Vector3(0, 0, 0), Quaternion.identity, transform);
        endEnergy.GetComponent<RectTransform>().anchoredPosition = new Vector3(-295, 70, 0);
        energySprites.Add(endEnergy);
        UpdateEnergyBar();
    }

    void UpdateEnergyBar()
    {
        // Debug.Log("Updating Energy Bar. Player health is " + playerEnergy + ".");
        if (playerEnergy == 4)
        {
            energySprites[0].GetComponent<Image>().sprite = startEnergySprites[2];
            energySprites[1].GetComponent<Image>().sprite = endEnergySprites[2];
        }
        else if (playerEnergy == 3)
        {
            energySprites[0].GetComponent<Image>().sprite = startEnergySprites[2];
            energySprites[1].GetComponent<Image>().sprite = endEnergySprites[1];
        }
        else if (playerEnergy == 2)
        {
            energySprites[0].GetComponent<Image>().sprite = startEnergySprites[2];
            energySprites[1].GetComponent<Image>().sprite = endEnergySprites[0];
        }
        else if (playerEnergy == 1)
        {
            energySprites[0].GetComponent<Image>().sprite = startEnergySprites[1];
            energySprites[1].GetComponent<Image>().sprite = endEnergySprites[0];
        }
        else if (playerEnergy == 0)
        {
            energySprites[0].GetComponent<Image>().sprite = startEnergySprites[0];
            energySprites[1].GetComponent<Image>().sprite = endEnergySprites[0];
        }
    }
}
