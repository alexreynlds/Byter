using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public TMP_Text coinsText;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        coinsText
            .SetText("Coins:" +
            GameObject
                .Find("Player")
                .GetComponent<PlayerProfile>()
                .currency
                .ToString("0"));
    }
}
