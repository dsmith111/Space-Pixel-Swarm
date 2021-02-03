using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDrag : MonoBehaviour
{

    private Vector3 Origin; // place where mouse is first pressed
    private Vector3 Diference; // change in position of mouse relative to origin

    void LateUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Origin = MousePos();
        }
        if (Input.GetMouseButton(0))
        {
            Diference = MousePos() - transform.position;
            transform.position = Origin - Diference;
        }

    }
    // return the position of the mouse in world coordinates (helper method)
    Vector3 MousePos()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }


}