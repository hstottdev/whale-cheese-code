using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class spriteButton : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    Color normalColor;

    [SerializeField] Color onHoverColor;
    [SerializeField] Color onClickColor;
    [SerializeField] UnityEvent onClick;

    [SerializeField] UnityEvent onEnter;
    [SerializeField] UnityEvent onExit;

    public bool interactable = true;


    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        normalColor = GetComponent<SpriteRenderer>().color;
    }

    public void Click()
    {
        onClick.Invoke();
    }

    private void OnMouseDown()
    {
        if (interactable)
        {
            spriteRenderer.color = onClickColor;
        }
    }

    private void OnMouseUp()
    {
        if (interactable)
        {
            spriteRenderer.color = onHoverColor;
            onClick.Invoke();
        }
    }

    private void OnMouseEnter()
    {
        if (interactable)
        {
            spriteRenderer.color = onHoverColor;
            onEnter.Invoke();
        }
    }

    private void OnMouseExit()
    {
        if (interactable)
        {
            spriteRenderer.color = normalColor;
            onExit.Invoke();
        }
    }


}
