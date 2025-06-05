using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLocation : MonoBehaviour
{
    private Vector2Int location;
    public Vector2Int Location {
        get{return location;}
        set{location=value;}
    }

    public void RegisterLocationOnMap(GameObject gameObject)
    {
        PlacementManager.Instance.RegisterLocation(Location , gameObject);
    }
    public void UnregisterLocationOnMap()
    {
        PlacementManager.Instance.UnregisterLocation(Location);
    }
    



}
