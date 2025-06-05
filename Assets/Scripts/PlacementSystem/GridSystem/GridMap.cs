using System.Collections;
using System.Collections.Generic;
using System.Text;
using Unity.Mathematics;
using UnityEngine;

public class GridMap
{
    public Vector2Int start;
    public Vector2Int end;

    public Vector2Int cell;
    public int [ , ] entitiesId;
    public GameObject [ , ] entities;

    public GridMap( Vector2Int start , Vector2Int end , Vector2Int cell  )
    {
        this.start = start;
        this.end = end;
        this.cell = cell;

        int width = Mathf.Abs( (int)(start.y - end.y));
        int height = Mathf.Abs((int)(start.x - end.x));

        this.entitiesId = new int[(int)(width/cell.x), (int)(height/cell.y)];
        this.entities = new GameObject[(int)(width/cell.x), (int)(height/cell.y)];

        // Debug.Log("Row :" + entitiesId.GetLength(0) + " ; Col : " +entitiesId.GetLength(1)  );


    }

    public int GetLocation(Vector2Int location)
    {

        return entitiesId[location.x , location.y];
    }
    public GameObject GetObjLocation(Vector2Int location)
    {

        return entities[location.x , location.y];
    }
    public GameObject[,] GetEntities()
    {
        return entities;
    }
    public int[,] GetEntitiesId()
    {
        return entitiesId;
    }



    public void RegisterLocation(Vector2Int location, int id, GameObject gameObject)
    {
        entitiesId[location.x, location.y] = id;
        entities[location.x, location.y] = gameObject;

        // Debug.Log("Vị trí là" + location.x + " " + location.y);
        //      StringBuilder sb = new StringBuilder();
        //     for (int i = 0; i < entitiesId.GetLength(0); i++) // Duyệt qua các hàng
        //     {
        //         for (int j = 0; j < entitiesId.GetLength(1); j++) // Duyệt qua các cột
        //         {
        //             sb.Append(entitiesId[i, j] + "\t"); // Thêm phần tử vào StringBuilder
        //         }
        //         sb.AppendLine(); // Thêm dòng mới sau mỗi hàng
        //     }

        // Debug.Log(sb.ToString()); // In toàn bộ mảng ra màn hình
    }

    public void UnregisterLocation(Vector2Int location)
    {
        entitiesId[location.x , location.y] = 0;
        entities[location.x , location.y] = null;
    }


    
}
