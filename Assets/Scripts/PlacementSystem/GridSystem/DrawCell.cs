using System.Collections;
using System.Collections.Generic;
using Pathfinding.Util;
using UnityEngine;

public class DrawCell 
{
    public Vector2Int cell;
    public float border;
    public GameObject cellObject;


    public DrawCell(Vector2Int cell , float border )
    {
        this.cell = cell;
        this.border = border;
        this.cellObject = new GameObject();

    }
    public void Deactivate()
    {
        cellObject.SetActive( false );
    }
    public void Activate()
    {
        cellObject.SetActive(true);
    }
    public void Draw(Material material)
    {
        MeshFilter meshFilter = cellObject.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer= cellObject.AddComponent<MeshRenderer>();
        meshRenderer.material = material;

        Mesh mesh  = new Mesh();
        Vector3[] vertices  = new Vector3[16];
        int[] triangles = new int[8*3];


        int triIndex = 0;

        int v = 0;

        float w = cell.x;
        float h = cell.y;
        float b = border;

        // Outer corners
        vertices[v] = new Vector3(0, 0.1f, 0); //bl
        vertices[v+1] = new Vector3(w, 0.1f, 0); //br
        vertices[v+2] = new Vector3(w, 0.1f, h); //tr
        vertices[v+3] = new Vector3(0, 0.1f, h); //tl

        // Inner corners
        vertices[v+4] = new Vector3(b, 0.1f, b); 
        vertices[v+5] = new Vector3(w - b, 0.1f, b); 
        vertices[v+6] = new Vector3(w - b, 0.1f, h - b); 
        vertices[v+7] = new Vector3(b, 0.1f , h - b); 

        

        // Bottom border
        AddQuad(triangles, ref triIndex, 0, 1, 5, 4);
        // Right border
        AddQuad(triangles, ref triIndex, 5,1,2,6);
        // Top border
        AddQuad(triangles, ref triIndex, 7,6,2,3);
        // Left border
        AddQuad(triangles, ref triIndex, 0,4,7,3);


        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        meshFilter.mesh = mesh;
    }
    private void AddQuad(int[] triangles, ref int t, int bl, int br, int tr, int tl)
    {
        // Triangle 1
        triangles[t++] = bl;
        triangles[t++] = tl;
        triangles[t++] = br;

        // Triangle 2
        triangles[t++] = tl;
        triangles[t++] = tr;
        triangles[t++] = br;
    }
    public void SetPos(Vector3 pos)
    {
        cellObject.transform.position = pos - new Vector3(cell.x /2 , 0 , cell.y/2); 
    }

}
