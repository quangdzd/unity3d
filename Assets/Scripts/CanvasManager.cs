using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : SingletonDestroy<CanvasManager>
{
    public List<Canvas> canvasList;
    public void OnDisableAll()
    {
        foreach (Canvas c in canvasList)
        {
            c.gameObject.SetActive(false);
        }
    }

}
