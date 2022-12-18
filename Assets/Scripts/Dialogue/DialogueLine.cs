using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace DialogueSystem
{
    public class DialogueLine : DialogueBaseClass
    {
        private Text textHolder;

        [Header("Text Options")]
        [SerializeField] private string input;

        [SerializeField] private Color textColor;

        [SerializeField] private Font textFont;

        [SerializeField] private float delay;

        [Header("Audio Options")]
        [SerializeField] private AudioClip sound;

        private void Start()
        {
            textHolder = GetComponent<Text>();

            StartCoroutine(WriteText(input, textHolder, textColor, textFont, delay, sound));
        }
    }
}
