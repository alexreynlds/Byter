using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DialogueBaseClass : MonoBehaviour
{
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
            AudioManager.instance.Play(sound);
            yield return new WaitForSeconds(delay);
        }
    }

}
