using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Player Input")]
    private PlayerInput playerInput;

    private InputAction moveAction;

    private InputAction interactAction;

    [Header("Player Info")]
    [SerializeField]
    public bool canMove;

    [SerializeField]
    public bool canInteract;

    public bool canTalk;

    public GameObject talkObjective;

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();

        moveAction = playerInput.actions["Move"];
        interactAction = playerInput.actions["Interact"];

        canMove = true;
        canInteract = true;
    }

    void Update()
    {
        // Debug.Log (test);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "DialogueTrigger")
        {
            other.gameObject.GetComponent<DialogueTrigger>().enabled = true;

            if (!other.gameObject.GetComponent<DialogueTrigger>().Triggerable)
            {
                other
                    .gameObject
                    .GetComponent<DialogueTrigger>()
                    .TriggerDialogue(canInteract);
            }

            // If the conversation isnt triggerable i.e cutscene, required convo then just play it.
            // if (
            //     !other
            //         .gameObject
            //         .GetComponent<DialogueTrigger>()
            //         .Triggerable
            // )
            // {
            //     other
            //         .gameObject
            //         .GetComponent<DialogueTrigger>()
            //         .TriggerDialogue(canInteract);
            // }
            // else
            // // Otherwise, only trigger the conversation upon user input.
            // {
            //     interactAction.performed += ctx =>
            //         other
            //             .gameObject
            //             .GetComponent<DialogueTrigger>()
            //             .TriggerDialogue(canInteract);
            // }
            canTalk = true;
            talkObjective = other.gameObject;
        }
        if (other.tag == "CutsceneTrigger")
        {
            other
                .gameObject
                .GetComponent<CutsceneTrigger>()
                .TriggerCutscene(canInteract);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "DialogueTrigger")
        {
            canTalk = false;
            talkObjective = null;
        }
    }

    // This seems like an inefficient way to handle interactions, come back to it later
    void OnInteract()
    {
        if (canTalk)
        {
            if (
                talkObjective.GetComponent<DialogueTrigger>().Triggerable ||
                talkObjective.GetComponent<DialogueTrigger>().IsOneTime &&
                !talkObjective.GetComponent<DialogueTrigger>().hasBeenTriggered
            )
                talkObjective
                    .GetComponent<DialogueTrigger>()
                    .TriggerDialogue(canInteract);
        }
    }

    public void EnterDialogue()
    {
        canMove = false;
        canInteract = false;
    }

    public void ExitDialogue()
    {
        canMove = true;
        canInteract = true;
    }
}
