using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tutorial : MonoBehaviour
{
    [SerializeField] int levelIndex;
    [SerializeField] List<GameObject> clouds;
    // Start is called before the first frame update
    void Start()
    {
        foreach(GameObject cloud in clouds)
        {
            cloud.SetActive(false);
        }

        if(LevelManager.currentlevelIndex != levelIndex)
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        CloudEnableCheck();
    }

    void CloudEnableCheck()
    {
        //if the bottom card has been disabled by the player
        if (clouds[0].GetComponent<CanvasGroup>().alpha == 0)
        {
            Destroy(clouds[0]);
            clouds.Remove(clouds[0]);
            if (clouds.Count == 0) Destroy(this);
            //remove cloud from list and destroy
        }
        else
        {
            //enable bottom cloud in the list
            clouds[0].SetActive(true);
        }
    }
}
