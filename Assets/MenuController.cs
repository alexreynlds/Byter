using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    public GameObject MainMenuItems;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Test(){
        Debug.Log("test");
    }

    public void ChangeMenu(){
        // MainMenuItems.gameObject.transform.position = new Vector2(10, 10);
        Debug.Log(MainMenuItems.gameObject.transform.position.x);
        while(MainMenuItems.gameObject.transform.position.x != -1000){
            MainMenuItems.gameObject.transform.position += Vector3.left;
        }

    }
}
