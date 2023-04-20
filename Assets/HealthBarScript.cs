using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarScript : MonoBehaviour
{
    [SerializeField] private GameObject[] fullHealthSprites;
    [SerializeField] private GameObject[] halfHealthSprites;
    [SerializeField] private GameObject[] emptyHealthSprites;

    GameObject startHealth;
    GameObject endHealth;

    void Start()
    {
        startHealth = Instantiate(fullHealthSprites[0], new Vector3(0, 0, 0), Quaternion.identity, transform);
        startHealth.GetComponent<RectTransform>().anchoredPosition = new Vector3(-250, 155, 0);
        
        GameObject temp = Instantiate(fullHealthSprites[1], new Vector3(0, 0, 0), Quaternion.identity, transform);
        temp.GetComponent<RectTransform>().anchoredPosition = new Vector3(-210, 155, 0);
        
        endHealth = Instantiate(fullHealthSprites[2], new Vector3(0, 0, 0), Quaternion.identity, transform);
        endHealth.GetComponent<RectTransform>().anchoredPosition = new Vector3(-170, 155, 0);

    }

    void Update()
    {
        
    }
}
