using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class randButton : MonoBehaviour
{
    [SerializeField] TextMeshPro keyText;
    [SerializeField] spriteButton button;
    [SerializeField] string defaultKey;
    string currentKey;

    List<string> availableKeys;

    [SerializeField] bool randomiseKey;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(awaitLevelSpawn());
    }

    // Update is called once per frame
    void Update()
    {
        if(currentKey != null)
        {
            if(Input.GetKeyDown(currentKey))
            {
                button.Click();
            }
        }

    }

    IEnumerator awaitLevelSpawn()
    {
        yield return new WaitUntil(LevelManager.isLevelSpawned);
        DefineAvailableKeys();
        SetKey();
    }


    void DefineAvailableKeys()
    {
        availableKeys = new List<string>();
        try
        {
            availableKeys = LevelManager.inst.currentLevel.inputKeysAllowed;  
        }
        catch
        {
            availableKeys.Add(defaultKey);
        }
    }
    void SetKey()
    {
        if(randomiseKey)
        {
            int randIndex = Random.Range(0, availableKeys.Count);
            currentKey = availableKeys[randIndex];
        }
        else
        {
            currentKey = defaultKey;
        }
        keyText.text = currentKey;
    }
}
