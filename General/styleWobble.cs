using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class styleWobble : MonoBehaviour
{
    [SerializeField] float interval = 0.5f;
    [SerializeField] FontStyles newStyle;

    TextMeshProUGUI tmp;
    FontStyles myOldStyle;



    // Update is called once per frame
    void Start()
    {
        tmp = GetComponent<TextMeshProUGUI>();
        myOldStyle = tmp.fontStyle;

        Invoke("Wobble", interval);
    }

    void Wobble()
    {
        if(tmp.fontStyle == myOldStyle)
        {
            tmp.fontStyle = newStyle;
        }
        else
        {
            tmp.fontStyle = myOldStyle;
        }

        Invoke("Wobble", interval);
    }
}
