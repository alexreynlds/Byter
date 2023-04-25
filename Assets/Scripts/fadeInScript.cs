using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class fadeInScript : MonoBehaviour
{
    private Image image;
    private Text text;

    public float fadeSpeed = 0.002f;
    public float waitTime = 3f;
    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        text = transform.GetChild(0).GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if(waitTime > 0)
        {
            waitTime -= Time.deltaTime;
        } else {
             if (image.color.a > 0)
                {
                    image.color = new Color(image.color.r, image.color.g, image.color.b, image.color.a - fadeSpeed);
                    text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a - fadeSpeed);
                }
                if(image.color.a <= 0)
                {
                    Destroy(gameObject);
                }
        }
       
    }


}
