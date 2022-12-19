using System.Collections;
using UnityEngine;

namespace DialogueSystem
{
    public class DialogueHolder : MonoBehaviour
    {
        [SerializeField]
        private GameObject lineHolder;

        private void Awake()
        {
            StartCoroutine(dialogueSequence());
        }

        private IEnumerator dialogueSequence()
        {
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
