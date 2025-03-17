using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(SpriteRenderer))]
public class colourFadeRenderer : MonoBehaviour
{
    public float fadeTime;
    public float delay;
    public bool disableWhenFaded;
    [SerializeField] bool fade;
    SpriteRenderer spriteRenderer;
    float startAlpha;

    [SerializeField] [Range(0, 1)] float targetAlpha = 0;
    // Start is called before the first frame update
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        startAlpha = spriteRenderer.color.a;
    }

    public static void FadeObject(GameObject objectToFade,bool destroyAfterFading = true)
    {
        try
        {
            foreach (SpriteRenderer s in objectToFade.GetComponentsInChildren<SpriteRenderer>())
            {
                s.gameObject.AddComponent<colourFadeRenderer>();
                colourFadeRenderer fader = s.GetComponent<colourFadeRenderer>();
                fader.fadeTime = 10f;
                fader.disableWhenFaded = true;
                fader.StartFading();
            }

            foreach (Graphic g in objectToFade.GetComponentsInChildren<Graphic>())
            {
                g.gameObject.AddComponent<colourFade>();
                colourFade fader = g.GetComponent<colourFade>();
                fader.fadeTime = 10f;
                fader.disableWhenFaded = true;
                fader.StartFading();
            }

            if (destroyAfterFading)
            {
                Destroy(objectToFade, 1);
            }
        }
        catch
        {
            Debug.LogWarning("Cannot Fade Object: " + objectToFade + ", it may have no graphics on itself or it's children");
        }


    }

    public void StartFading()
    {
        fade = true;
        reset();
    }

    void Fade()
    {
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, Mathf.Lerp(spriteRenderer.color.a, targetAlpha, fadeTime * Time.deltaTime));
    }

    // Update is called once per frame
    void Update()
    {
        if (fade)
        {
            Invoke("Fade", delay);

            if (spriteRenderer.color.a < 0.05 && disableWhenFaded)
            {
                gameObject.SetActive(false);
            }
        }
    }
    public void reset()
    {
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, startAlpha);
    }

    private void OnDisable()
    {
        reset();
    }

    private void OnEnable()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        reset();
    }
}
