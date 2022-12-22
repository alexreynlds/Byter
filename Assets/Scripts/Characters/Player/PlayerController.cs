using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, IDataPersistence
{
    public static PlayerController instance { get; private set; }

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

    public GameObject menu;

    public Vector2 position;

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad (gameObject);
        }
        else if (instance != this)
        {
            Destroy (gameObject);
        }
    }

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();

        moveAction = playerInput.actions["Move"];
        interactAction = playerInput.actions["Interact"];

        canMove = true;
        canInteract = true;
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

    public void OnPause()
    {
        // menu.GetComponent<pauseMenu>().PauseGameAnim();
        // GameObject.Find("PauseMenu").GetComponent<pauseMenu>().PauseGameAnim();
        Resources.FindObjectsOfTypeAll<pauseMenu>()[0].PauseGameAnim();
    }

    public void SaveData(ref GameData data)
    {
        data.playerPosition = this.transform.position;
    }

    public void LoadData(GameData data)
    {
        this.transform.position = data.playerPosition;
    }
}
