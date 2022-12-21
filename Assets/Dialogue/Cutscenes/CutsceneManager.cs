using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneManager : MonoBehaviour
{
    private GameObject player;

    [Header("Cutscene Options")]
    [SerializeField]
    private GameObject cutsceneDialogueBox;

    public AudioClip cutsceneMusic;

    // Start is called before the first frame update
    void Start()
    {
        AudioManager.instance.PlayMusic (cutsceneMusic);
        player = GameObject.Find("Player");
        Instantiate (cutsceneDialogueBox, transform);
        player.GetComponent<PlayerController>().canMove = false;
    }

    // Update is called once per frame
    void Update()
    {
    }
}
