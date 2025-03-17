using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Graphic))]
public class colourFade : MonoBehaviour
{
    public float fadeTime;
    public float delay;
    public bool disableWhenFaded;
    [SerializeField] bool fade;
    Graphic graphic;
    float startAlpha;
    // Start is called before the first frame update
    void Awake()
    {
        graphic = GetComponent<Graphic>();
        startAlpha = graphic.color.a;
    }

    public static void FadeObject(GameObject objectToFade,bool destroyAfterFading = true)
    {
        try
        {
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
        graphic.color = new Color(graphic.color.r, graphic.color.g, graphic.color.b, Mathf.Lerp(graphic.color.a, 0, fadeTime * Time.deltaTime));
    }

    // Update is called once per frame
    void Update()
    {
        if (fade)
        {
            Invoke("Fade", delay);

            if (graphic.color.a < 0.05 && disableWhenFaded)
            {
                gameObject.SetActive(false);
            }
        }
    }
    public void reset()
    {
        graphic.color = new Color(graphic.color.r, graphic.color.g, graphic.color.b, startAlpha);
    }

    private void OnDisable()
    {
        reset();
    }

    private void OnEnable()
    {
        graphic = GetComponent<Graphic>();
        reset();
    }
}
