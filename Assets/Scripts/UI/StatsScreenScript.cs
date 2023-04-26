using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsScreenScript : MonoBehaviour
{
    [Header("Player Stats")]
    [SerializeField] private GameObject maxHealthText;
    [SerializeField] private GameObject attackDamageText;
    [SerializeField] private GameObject attackRangeText;
    [SerializeField] private GameObject attackSpeedText;
    // [SerializeField] private GameObject attackKnockbackText;
    [SerializeField] private GameObject projectileSpeedText;
    [SerializeField] private GameObject moveSpeedText;

    private GameObject player;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        UpdateStats();
    }

    public void UpdateStats()
    {
        maxHealthText.GetComponent<Text>().text = player.GetComponent<PlayerStats>().maxHealth.ToString();
        attackDamageText.GetComponent<Text>().text = player.GetComponent<PlayerStats>().attackDamage.ToString();
        attackRangeText.GetComponent<Text>().text = player.GetComponent<PlayerStats>().attackRange.ToString();
        attackSpeedText.GetComponent<Text>().text = player.GetComponent<PlayerStats>().attackSpeed.ToString();
        // attackKnockbackText.GetComponent<Text>().text =  player.GetComponent<PlayerStats>().attackKnockback.ToString();
        projectileSpeedText.GetComponent<Text>().text = (player.GetComponent<PlayerStats>().projectileSpeed - 2).ToString();
        moveSpeedText.GetComponent<Text>().text = (player.GetComponent<PlayerStats>().moveSpeed - 1).ToString();
    }
}
