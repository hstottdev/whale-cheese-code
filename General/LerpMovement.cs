using UnityEngine;
using static Console;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.Events;


public class LerpMovement : MonoBehaviour
{
    public enum movementTypes
    {
        linear,
        easeInOutCubic,

        easeInOutQuint,

        easeOutCubic
    }
    
    #if UNITY_EDITOR
    [InspectorButton("SetAToCurrent")]
    [SerializeField] bool setA;
    #endif
    [SerializeField] Vector3 positionA;

    [Space]

    #if UNITY_EDITOR
    [InspectorButton("SetBToCurrent")]
    [SerializeField] bool setB;
    #endif
    [SerializeField] Vector3 positionB;

    [SerializeField] movementTypes movementType;
    [SerializeField] float lerpSpeed;

    [SerializeField] UnityEvent OnEnable_;
    Vector3 currentEnd;
    Vector3 currentStart;
    void SetAToCurrent()
    {
        positionA = transform.localPosition;
    }

    void SetBToCurrent()
    {
        positionB = transform.localPosition;
    }

    bool moving;
    
    void OnEnable()
    {
        OnEnable_.Invoke();
    }

    public void SetToA()
    {
        //Debug.Log("set to A");
        transform.localPosition = positionA;
    }

    public void SetToB()
    {
        transform.localPosition = positionB;
    }

    public void MoveAToB()
    {
        //Debug.Log("moving A to B");
        currentStart = positionA;
        currentEnd = positionB;
        if(!moving) StartCoroutine(Move());
    }

    public void MoveBToA()
    {
        currentStart = positionB;
        currentEnd = positionA;
        if(!moving) StartCoroutine(Move());
    }

    IEnumerator Move()
    {
        moving = true;
        
        float currentSpeed;

        while(GetProgression(currentStart,currentEnd) < 0.999)
        {
            currentSpeed = Time.deltaTime * GetCurrentLerpSpeed(GetProgression(currentStart,currentEnd)); 

            transform.localPosition = Vector3.Lerp(transform.localPosition,currentEnd,currentSpeed);

            yield return null;
        }
        transform.localPosition = currentEnd;
        moving = false;
    }

        float GetProgression(Vector3 start, Vector3 end)
    {
        float totalDistance = Vector3.Distance(start,end);//diistance from start to end
        float currentDistance = Vector3.Distance(transform.localPosition,start);//distance from start

        return currentDistance / totalDistance;//return as a fraction for progression
    }

    float GetCurrentLerpSpeed(float progression)
    {
        switch(movementType)
        {
            case movementTypes.linear:
                return lerpSpeed;
            case movementTypes.easeInOutCubic:
                return easeInOutCubic(progression)*lerpSpeed;
            case movementTypes.easeInOutQuint:
                return easeInOutQuint(progression)*lerpSpeed;
            case movementTypes.easeOutCubic:
                return easeOutCubic(progression) * lerpSpeed;
        }
        return lerpSpeed;
    }
}
