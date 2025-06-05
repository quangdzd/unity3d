using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridtoWorld : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private LayerMask placement_LayerMask;


    public Vector3 MousePosOnMap()
    {
        Vector3 lastPos = new Vector3();
        Vector3 mouse = Input.mousePosition;
        mouse.z = cam.nearClipPlane;
        Ray ray = cam.ScreenPointToRay(mouse);
        RaycastHit hit ;
        if(Physics.Raycast(ray , out hit , 100 ,placement_LayerMask))
        {
            lastPos = hit.point;

        }
        return lastPos;
    }
}
