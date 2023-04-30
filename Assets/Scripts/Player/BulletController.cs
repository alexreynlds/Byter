using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    private float damage;
    // Start is called before the first frame update
    void Start()
    {
        Collider2D collider1 = gameObject.GetComponent<CircleCollider2D>();
        Collider2D collider2 = GameObject.Find("Player").GetComponent<BoxCollider2D>();
        damage = GameObject.Find("Player").GetComponent<PlayerStats>().attackDamage;
        Physics2D.IgnoreCollision(collider1, collider2);
        StartCoroutine(DestroyBullet());
    }

    IEnumerator DestroyBullet()
    {
        yield return new WaitForSeconds(GameObject.Find("Player").GetComponent<PlayerStats>().attackRange);
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            other.gameObject.GetComponent<EnemyController>().TakeDamage(damage);
            Destroy(gameObject);
        }
        if (other.gameObject.tag == "Player")
        {
            return;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Door")
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
