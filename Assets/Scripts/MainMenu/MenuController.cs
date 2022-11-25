using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    public GameObject MainMenuItems;
    public GameObject OptionsMenuItems;
    public GameObject Model;
    public Animator anim;
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

    public void GoToOptions(){
        MainMenuItems.GetComponent<Animator>().Play("MainOut");
        OptionsMenuItems.GetComponent<Animator>().Play("OptionsIn");
        Model.GetComponent<Animator>().Play("ModelOut");
    }

    public void GoToMain(){
        MainMenuItems.GetComponent<Animator>().Play("MainIn");
        OptionsMenuItems.GetComponent<Animator>().Play("OptionsOut");
    }
}
