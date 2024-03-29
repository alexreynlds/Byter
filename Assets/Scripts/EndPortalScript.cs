using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPortalScript : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            RoomController.instance.LoadNextLevel();
            Destroy( transform.gameObject);
        }
    }
}
