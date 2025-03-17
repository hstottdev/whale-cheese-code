using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    public string levelName; 
    public string levelNumberName;

    public List<string> inputKeysAllowed;

    public float minigameInterval = 1;
    public bool allowMinigamesRepeats;
    public List<GameObject> minigames;


    void Start() 
    {
        LevelManager.inst.currentLevel = this;
    }
}


