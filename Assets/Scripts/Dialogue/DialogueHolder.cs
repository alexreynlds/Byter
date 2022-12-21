using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DialogueSystem
{
    public class DialogueHolder : MonoBehaviour
    {
        [SerializeField]
        private GameObject lineHolder;

        private GameObject player;

        private void Awake()
        {
            player = GameObject.Find("Player");
            StartCoroutine(dialogueSequence());
        }

        private IEnumerator dialogueSequence()
        {
            player.GetComponent<PlayerController>().EnterDialogue();
            for (int i = 0; i < lineHolder.transform.childCount; i++)
            {
                Deactivate();
                lineHolder.transform.GetChild(i).gameObject.SetActive(true);
                yield return new WaitUntil(() =>
                            lineHolder
                                .transform
                                .GetChild(i)
                                .GetComponent<DialogueLine>()
                                .finished);
            }
            gameObject.SetActive(false);
            player.GetComponent<PlayerController>().ExitDialogue();
            Object.Destroy (gameObject);
        }

        private void Deactivate()
        {
            for (int i = 0; i < lineHolder.transform.childCount; i++)
            {
                lineHolder.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }
}
