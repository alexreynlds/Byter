using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DDASystem : MonoBehaviour
{
    private GameObject player;

    // Tracked player stats
    public float damageTaken;
    public float damageDone;
    public float enemiesKilled;
    public float timesSupered;

    void Awake()
    {
        player = GameObject.Find("Player");
    }

    // Start is called before the first frame update
    void Start() { }

    // Update is called once per frame
    void Update() { }
}
