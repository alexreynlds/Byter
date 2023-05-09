using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormBossBodyScript : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerStats>().TakeDamage(1.0f);
        }
    }
}
