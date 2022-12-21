using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace DialogueSystem
{
    public class DialogueLine : MonoBehaviour
    {
        private GameObject player;

        private PlayerInput playerInput;

        private InputAction continueAction;

        private Text textHolder;

        [Header("Text Options")]
        [SerializeField]
        private string input;

        [SerializeField]
        private Color textColor;

        [SerializeField]
        private Font textFont;

        [SerializeField]
        private float delay;

        [Header("Audio Options")]
        [SerializeField]
        private AudioClip sound;

        [Header("Character Stage")]
        [SerializeField]
        private Sprite leftCharacterSprite;

        [SerializeField]
        private Image leftImageHolder;

        [SerializeField]
        private Sprite rightCharacterSprite;

        [SerializeField]
        private Image rightImageHolder;

        [SerializeField]
        private string talking;

        private Color talkingColor = new Color32(255, 255, 255, 255);

        private Color notTalkingColor = new Color32(90, 90, 90, 255);

        public bool finished;

        public bool continued;

        public GameObject ContinueText;

        private void Awake()
        {
            ContinueText.SetActive(false);
            textHolder = GetComponent<Text>();
            textHolder.text = "";

            leftImageHolder.sprite = leftCharacterSprite;
            rightImageHolder.sprite = rightCharacterSprite;
            leftImageHolder.preserveAspect = true;
            rightImageHolder.preserveAspect = true;

            player = GameObject.Find("Player");
            playerInput = player.GetComponent<PlayerInput>();

            continueAction = playerInput.actions["Interact"];
        }

        private void Start()
        {
            if (talking == "L")
            {
                rightImageHolder.GetComponent<Image>().color = notTalkingColor;
                leftImageHolder.GetComponent<Image>().color = talkingColor;
            }
            else
            {
                leftImageHolder.GetComponent<Image>().color = notTalkingColor;
                rightImageHolder.GetComponent<Image>().color = talkingColor;
            }

            StartCoroutine(WriteText(input,
            textHolder,
            textColor,
            textFont,
            delay,
            sound));

            // rightImageHolder.GetComponent<Image>().color = notTalkingColor;
        }

        protected IEnumerator
        WriteText(
            string input,
            Text textHolder,
            Color textColor,
            Font textFont,
            float delay,
            AudioClip sound
        )
        {
            textHolder.color = textColor;
            textHolder.font = textFont;
            textHolder.text = "";
            continued = false;

            for (int i = 0; i < input.Length; i++)
            {
                textHolder.text += input[i];
                AudioManager.instance.Play(sound, 0.1f);
                yield return new WaitForSeconds(delay);
            }
            ContinueText.SetActive(true);
            yield return new WaitUntil(() =>
                        playerInput.actions["Interact"].triggered);
            finished = true;
        }
    }
}
