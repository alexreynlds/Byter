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
    public float roomsCleared;
    public float averageRoomClearTime;

    // Previous stats
    private float lastDamageTaken = 0f;
    private float lastDamageDone = 0f;
    private float lastEnemiesKilled = 0f;
    private float lastTimesSupered = 0f;
    private float lastRoomsCleared = 0f;
    private float lastAverageRoomClearTime = 0f;

    [Header("Difficulty Settings")]
    public float maxDifficulty = 1.5f;
    public float minDifficulty = 0.5f;
    public float difficultyIncreaseRate = 0.1f;
    public float difficultyDecreaseRate = 0.05f;
    public float timeBetweenDifficultyChanges = 20.0f;


    public float timeSinceLastChange;

    public float gameplayTimer;
    [Header("Current Difficulty")]
    public float currentDifficulty;

    public static DDASystem instance;

    void Awake()
    {
        instance = this;
        player = GameObject.Find("Player");
    }

    // Start is called before the first frame update
    void Start()
    {
        currentDifficulty = 1.0f;
        timeSinceLastChange = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        gameplayTimer += Time.deltaTime;
        timeSinceLastChange += Time.deltaTime;
        averageRoomClearTime = gameplayTimer / roomsCleared;

        if (timeSinceLastChange >= timeBetweenDifficultyChanges)
        {
            timeSinceLastChange = 0.0f;

            // If the player is taking more damage, decrease difficulty
            if ((damageTaken / enemiesKilled) > (lastDamageTaken / lastEnemiesKilled))
            {
                currentDifficulty -= difficultyDecreaseRate;
            }
            else if ((damageTaken / enemiesKilled) < (lastDamageTaken / lastEnemiesKilled))
            {
                currentDifficulty += difficultyIncreaseRate;
            }
            // If the player is taking longer to complete rooms, decrease difficulty
            if (averageRoomClearTime > lastAverageRoomClearTime)
            {
                currentDifficulty -= difficultyDecreaseRate;
            }
            else if (averageRoomClearTime < lastAverageRoomClearTime)
            {
                currentDifficulty += difficultyIncreaseRate;
            }

            // If the player is completing rooms without taking damage, increase difficulty
            if (damageTaken == lastDamageTaken && roomsCleared > lastRoomsCleared)
            {
                currentDifficulty += difficultyIncreaseRate;
            }

            if (timesSupered > lastTimesSupered)
            {
                currentDifficulty += difficultyIncreaseRate;
            }



            if (currentDifficulty > maxDifficulty)
            {
                currentDifficulty = maxDifficulty;
            }
            else if (currentDifficulty < minDifficulty)
            {
                currentDifficulty = minDifficulty;
            }
            timeSinceLastChange = 0.0f;
            UpdateStats();
        }
    }

    private void UpdateStats()
    {
        lastDamageTaken = damageTaken;
        lastDamageDone = damageDone;
        lastEnemiesKilled = enemiesKilled;
        lastTimesSupered = timesSupered;
        lastRoomsCleared = roomsCleared;
        lastAverageRoomClearTime = gameplayTimer / roomsCleared;
    }

    public float GetCurrentDifficulty()
    {
        return currentDifficulty;
    }
}
