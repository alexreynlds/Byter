using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace DialogueSystem
{
    public class DialogueBaseClass : MonoBehaviour
    {
        public bool finished;

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

            for (int i = 0; i < input.Length; i++)
            {
                textHolder.text += input[i];
                AudioManager.instance.Play (sound);
                yield return new WaitForSeconds(delay);
            }
            yield return new WaitUntil(() => Input.GetMouseButton(0));
            finished = true;
        }
    }
}
