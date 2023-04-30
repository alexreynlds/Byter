using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletController : MonoBehaviour
{
    public int damage;
    public GameObject parent;
    public float range;
    // Start is called before the first frame update
    void Start()
    {
        Collider2D collider1 = gameObject.GetComponent<Collider2D>();
        Collider2D collider2 = parent.GetComponent<Collider2D>();
        Physics2D.IgnoreCollision(collider1, collider2);
        StartCoroutine(DestroyBullet());
    }

    IEnumerator DestroyBullet()
    {
        yield return new WaitForSeconds(range);
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponentInParent<PlayerStats>().TakeDamage(damage);
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "PlayerBody")
        {
            other.gameObject.GetComponentInParent<PlayerStats>().TakeDamage(damage);
            Destroy(gameObject);
        }
        else if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponentInParent<PlayerStats>().TakeDamage(damage);
            Destroy(gameObject);
        }
        else if (other.gameObject.tag == "Door")
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
