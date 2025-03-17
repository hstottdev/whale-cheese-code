using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minigame : MonoBehaviour
{
    public static void QueueAction(string actionName)
    {
        if(LevelManager.inst != null)
        {
            ActionManager.QueueAction(actionName);
        }
    }

    public static void ResetLevel()
    {
        LevelManager.inst.RestartLevel();
    }
}
