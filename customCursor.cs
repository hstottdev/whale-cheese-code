using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class customCursor : MonoBehaviour
{
    [SerializeField] Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        FollowMouse();
    }

    void FollowMouse()
    {
        if(cam == null)
        {
            transform.position = Input.mousePosition;
        }
        else
        {
            transform.position = cam.ScreenToWorldPoint(Input.mousePosition);
            transform.localPosition = new Vector3(transform.localPosition.x,transform.localPosition.y,0);
        }
        Cursor.visible = false;
    }
}
