using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class PopupWindowScript : MonoBehaviour
{
    [System.Serializable]
    struct popupItem
    {
        public string headingText;
        public string text;
    }

    public Text popupHeading;
    public Text popupText;

    private GameObject window;
    private Animator popupAni;

    private Queue<popupItem> popupQueue;
    private bool isActive = false;
    private Coroutine queueChecker;

    private void Start()
    {
        window = transform.GetChild(0).gameObject;
        popupAni = window.GetComponent<Animator>();
        window.SetActive(false);
        popupQueue = new Queue<popupItem>();
    }

    public void AddToQueue(string headingText, string text)
    {
        // Debug.Log(headingText + ' ' + text);
        popupQueue.Enqueue(new popupItem { headingText = headingText, text = text });
        if (queueChecker == null)
        {
            queueChecker = StartCoroutine(CheckQueue());
        }
    }

    private void ShowPopup(popupItem popupItem)
    {
        Debug.Log(popupItem.headingText + ' ' + popupItem.text);
        isActive = true;
        window.SetActive(true);
        popupHeading.text = popupItem.headingText;
        popupText.text = popupItem.text;
        popupAni.Play("PopupAnimation");
    }

    private IEnumerator CheckQueue()
    {
        do
        {
            ShowPopup(popupQueue.Dequeue());
            do
            {
                yield return null;
            } while (!popupAni.GetCurrentAnimatorStateInfo(0).IsTag("Idle"));
        } while (popupQueue.Count > 0);
        isActive = false;
        window.SetActive(false);
        queueChecker = null;
    }
}
