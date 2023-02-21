using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBulletScript : MonoBehaviour
{
    private float lifeSpan;

    void Start()
    {
        // Get the shooting range from the PlayerStats script and set it as the lifespan of the bullet
        lifeSpan =
            GameObject
                .Find("PlayerCharacter")
                .GetComponent<PlayerStats>()
                .shootingRange;
    }

    void FixedUpdate()
    {
        if (lifeSpan >= 0)
        {
            lifeSpan -= 0.1f;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
