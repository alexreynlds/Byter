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

    [Header("Player Stats")]
    [SerializeField]
    public float moveSpeed = 1.5f;

    private Vector2 movement;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();

        moveAction = playerInput.actions["Move"];
        interactAction = playerInput.actions["Interact"];

        canMove = true;
        canInteract = true;
    }

    private void Update()
    {
        movement = moveAction.ReadValue<Vector2>();
        if (canMove)
        {
            transform.Translate(movement * moveSpeed * Time.deltaTime);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("DialogueTrigger"))
        {
            // If the conversation isnt triggerable i.e cutscene, required convo then just play it.
            if (!other.gameObject.GetComponent<DialogueTrigger>().Triggerable)
            {
                other
                    .gameObject
                    .GetComponent<DialogueTrigger>()
                    .TriggerDialogue(canInteract);
            }
            else
            // Otherwise, only trigger the conversation upon user input.
            {
                interactAction.performed += ctx =>
                    other
                        .gameObject
                        .GetComponent<DialogueTrigger>()
                        .TriggerDialogue(canInteract);
            }
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
