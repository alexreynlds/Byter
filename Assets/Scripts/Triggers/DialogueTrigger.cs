using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField]
    private GameObject NPCDialogue;

    [SerializeField]
    public bool Triggerable;

    [Header("One Time Dialogue")]
    [SerializeField]
    public bool IsOneTime;

    public bool hasBeenTriggered;

    public void TriggerDialogue(bool canInteract)
    {
        if (
            canInteract && !IsOneTime ||
            canInteract && IsOneTime && !hasBeenTriggered
        )
        {
            Instantiate(NPCDialogue, GameObject.Find("Canvas").transform);
            if (IsOneTime)
            {
                hasBeenTriggered = true;
            }
        }
    }
}
