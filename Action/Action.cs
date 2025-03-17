using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Action", menuName = "Action", order = 0)]
public class Action : ScriptableObject 
{
    public string actionID;
    public RuntimeAnimatorController animatedIcon;
}

