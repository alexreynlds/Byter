using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneTrigger : MonoBehaviour
{
    private GameObject player;

    [Header("Cutscene Options")]
    [SerializeField]
    private GameObject cutsceneDialogue;

    public AudioClip cutsceneMusic;

    public bool hasBeenTriggered;

    void awake()
    {
        this.enabled = false;
    }

    public void TriggerCutscene(bool canInteract)
    {
        if (!hasBeenTriggered)
        {
            Instantiate(cutsceneDialogue, GameObject.Find("Canvas").transform);

            hasBeenTriggered = true;
        }
    }
}
