using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionSlotUI : MonoBehaviour
{
    [SerializeField] Animator animator;
    public Action currentAction;
    RuntimeAnimatorController emptySlotAnimator;
    // Start is called before the first frame update
    void Start()
    {
        emptySlotAnimator = animator.runtimeAnimatorController;
    }

    public void UpdateCurrentAction(Action action)
    {
        currentAction = action;
        if (currentAction != null)
        {
            bool isNulls = animator == null || action.animatedIcon == null;

            if (!isNulls)
            {
                animator.runtimeAnimatorController = action.animatedIcon;
            } 
        }
        else//action is null
        {
            animator.runtimeAnimatorController = emptySlotAnimator;
        }
    }
}
