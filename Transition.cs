using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Transition : MonoBehaviour
{
    [SerializeField] float moveOutDelay;
    [SerializeField] float moveInDelay;
    [SerializeField] List<LerpMovement> curtains = new List<LerpMovement>();
    [SerializeField] UnityEvent onEnable;

    void OnEnable()
    {
        onEnable.Invoke();
    }

    public void MoveIn()
    {
        foreach(LerpMovement l in curtains)
        {
            l.SetToA();
            l.Invoke("MoveAToB",moveInDelay);
        }
    }

    public void MoveOut()
    {
        foreach(LerpMovement l in curtains)
        {
            l.SetToB();
            l.Invoke("MoveBToA",moveOutDelay);
        }
    }


}
