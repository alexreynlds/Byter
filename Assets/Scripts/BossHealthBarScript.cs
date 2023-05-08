using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;


public class BossHealthBarScript : MonoBehaviour
{
    [SerializeField] private GameObject currentBoss;
    public Image healthBar;
    private bool bossDead = false;
    private float healthAmount = 100f;

    public void SetBoss(GameObject boss)
    {
        currentBoss = boss;

        // transform.Find("BossName").GetComponent<Text>().text = boss.gameObject.name;
        transform.Find("BossName").GetComponent<Text>().text = System.Text.RegularExpressions.Regex.Replace(boss.gameObject.name, "(\\B[A-Z])", " $1");
    }

    private void Update()
    {
        if (!bossDead)
        {
            healthBar.fillAmount = currentBoss.GetComponent<EnemyController>().health / 100.0f;
            if (healthBar.fillAmount <= 0)
            {
                bossDead = true;
                healthBar.fillAmount = 0;
            }
        }

    }
}
