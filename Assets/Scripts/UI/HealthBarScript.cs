using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarScript : MonoBehaviour
{
    [SerializeField] private GameObject startHealthContainer;
    [SerializeField] private GameObject middleHealthContainer;
    [SerializeField] private GameObject endHealthContainer;

    [SerializeField] private Sprite[] startHealthSprites;

    [SerializeField] private Sprite[] middleHealthSprites;

    [SerializeField] private Sprite[] endHealthSprites;

    GameObject[] middleHealths;

    List<GameObject> healthSprites = new List<GameObject>();

    private int playerHealth;
    private int maxPlayerHealth;
    // int tempHealth;

    void Start()
    {
        playerHealth = GameObject.Find("Player").GetComponent<PlayerStats>().currentHealth;
        maxPlayerHealth = GameObject.Find("Player").GetComponent<PlayerStats>().maxHealth;
        CreateHealthBar();
        UpdateHealthbar();
    }

    void CreateHealthBar()
    {
        GameObject startHealth = Instantiate(startHealthContainer, new Vector3(0, 0, 0), Quaternion.identity, transform);
        startHealth.GetComponent<RectTransform>().anchoredPosition = new Vector3(-265, 160, 0);
        healthSprites.Add(startHealth);

        int middleHealthCount = maxPlayerHealth - 4;


        for (int i = 0; i < middleHealthCount / 2; i++)
        {
            GameObject middleHealth = Instantiate(middleHealthContainer, new Vector3(0, 0, 0), Quaternion.identity, transform);
            middleHealth.GetComponent<RectTransform>().anchoredPosition = new Vector3(healthSprites[healthSprites.Count - 1].GetComponent<RectTransform>().anchoredPosition.x + 30, 160, 0);
            healthSprites.Add(middleHealth);
        }

        GameObject endHealth = Instantiate(endHealthContainer, new Vector3(0, 0, 0), Quaternion.identity, transform);
        endHealth.GetComponent<RectTransform>().anchoredPosition = new Vector3(healthSprites[healthSprites.Count - 1].GetComponent<RectTransform>().anchoredPosition.x + 30, 160, 0);
        healthSprites.Add(endHealth);

    }

    void Update()
    {
        if (GameObject.Find("Player").GetComponent<PlayerStats>().currentHealth != playerHealth)
        {
            playerHealth = GameObject.Find("Player").GetComponent<PlayerStats>().currentHealth;
            UpdateHealthbar();
        }
        else if (GameObject.Find("Player").GetComponent<PlayerStats>().maxHealth != maxPlayerHealth)
        {
            maxPlayerHealth = GameObject.Find("Player").GetComponent<PlayerStats>().maxHealth;
            foreach (GameObject health in healthSprites)
            {
                Destroy(health);
            }
            healthSprites.Clear();
            CreateHealthBar();
            UpdateHealthbar();
        }
    }

    void UpdateHealthbar()
    {
        int tempHealth = playerHealth;

        for (int i = 0; i < healthSprites.Count; i++)
        {
            if (tempHealth > 0)
            {
                if (tempHealth >= 2)
                {
                    if (i == 0)
                    {
                        healthSprites[i].GetComponent<Image>().sprite = startHealthSprites[2];
                    }
                    else if (i == healthSprites.Count - 1)
                    {
                        healthSprites[i].GetComponent<Image>().sprite = endHealthSprites[2];
                    }
                    else
                    {
                        healthSprites[i].GetComponent<Image>().sprite = middleHealthSprites[2];
                    }

                    tempHealth -= 2;
                }
                else
                {
                    if (i == 0)
                    {
                        healthSprites[i].GetComponent<Image>().sprite = startHealthSprites[tempHealth];
                    }
                    else if (i == healthSprites.Count - 1)
                    {
                        healthSprites[i].GetComponent<Image>().sprite = endHealthSprites[tempHealth];
                    }
                    else
                    {
                        healthSprites[i].GetComponent<Image>().sprite = middleHealthSprites[tempHealth];
                    }
                    tempHealth -= tempHealth;
                }
            }
            else
            {
                if (i == 0)
                {
                    healthSprites[i].GetComponent<Image>().sprite = startHealthSprites[0];
                }
                else if (i == healthSprites.Count - 1)
                {
                    healthSprites[i].GetComponent<Image>().sprite = endHealthSprites[0];
                }
                else
                {
                    healthSprites[i].GetComponent<Image>().sprite = middleHealthSprites[0];
                }
            }
        }
    }
}
