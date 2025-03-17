using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextWave : MonoBehaviour
{
    [SerializeField] [Tooltip("Example: <b></b>")]string richTextRipple = "<b></b>";
    List<string> richText;
    TextMeshProUGUI tmp;
    public string originalString;
    int charIndex;
    bool awaitExistingRichText;
    [Header("Wave")]
    [SerializeField]float rippleDelay;
    [SerializeField] float waveDelay;
    [SerializeField] float startDelay;

    // Start is called before the first frame update
    void Start()
    {
        tmp = GetComponent<TextMeshProUGUI>();
        originalString = tmp.text;
        SetRichText();
        //it begins
        Invoke("StartNewWave", startDelay);
    }

    void Wave()
    {          
        char currentChar = originalString[charIndex];//defining our current character based on the index

        float delay = rippleDelay;

        if(currentChar == '<')
        {
            awaitExistingRichText = true;
        }

        if (awaitExistingRichText)
        {
            if(currentChar == '>')
            {
                awaitExistingRichText = false;
            }
            delay = 0;
        }
        else
        {
            string currentCharReplacement = richText[0] + currentChar.ToString() + richText[1];//defining the current character newly surrounded by rich text

            string newCompleteString = originalString.Remove(charIndex, 1);//our new string initially equals the original minus our current character

            newCompleteString = newCompleteString.Insert(charIndex, currentCharReplacement);//then we insert the replacement of our new character with the added rich text

            tmp.text = newCompleteString;//then we set the text mesh pro equal to our new complete string
        }

        bool finishedCycle = !(charIndex < originalString.Length - 1);

        if (finishedCycle)
        {
            Invoke("FinishCycle", delay);
        }
        else
        {
            charIndex++;
            Invoke("Wave", delay);
        }
    }

    void SetRichText()
    {
        richText = new List<string>();

        richText.AddRange(richTextRipple.Split('>'));

        richText[0] += ">";
        richText[1] += ">";
        richText.RemoveAt(2);
    }

    void FinishCycle()
    {
        tmp.text = originalString;
        Invoke("StartNewWave", waveDelay);
    }

    private void StartNewWave()
    {
        charIndex = 0;
        Wave();
    }

    private void OnDisable()
    {
        CancelInvoke();
    }

    private void OnEnable()
    {
        if(tmp != null)
        {
            //it begins again
            tmp.text = originalString;
            Invoke("StartNewWave", startDelay);
        }

    }
}
