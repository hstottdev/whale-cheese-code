using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class canvasFader : MonoBehaviour
{
    bool fading;
    public CanvasGroup cg;
    [SerializeField] float fadeSpeed;
    [SerializeField] bool fadeOnAwake;
    public float targetAlpha = 1;
    // Start is called before the first frame update
    private void Awake()
    {
        cg = GetComponent<CanvasGroup>();

        if(fadeOnAwake)
        {
            StartFade();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (fading)
        {
            cg.alpha = Mathf.Lerp(cg.alpha, targetAlpha, fadeSpeed * Time.deltaTime);

            if(Mathf.Abs(targetAlpha-cg.alpha) < 0.01f)
            {
                cg.alpha = targetAlpha;
                fading = false;
            }
        }
    }

    public void SetTargetAlpha(float target)
    {
        targetAlpha = target;
    }

    public void StartFade()
    {
        fading = true;
    }

}
