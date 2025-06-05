using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawGid 
{
    private int rols;
    private int cols;

    private Vector3 start;
    private Vector2Int cell ;

    public DrawGid(int rols, int cols , Vector3 start , Vector2Int cell)
    {
        this.rols = rols;
        this.cols = cols;
        this.cell = cell;
        this.start = start - new Vector3(cell.x/2 , 0 , cell.y/2);
    }
    public void Draw(Material material)
    {
        GameObject gameObject = new GameObject();
        MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer= gameObject.AddComponent<MeshRenderer>();
        meshRenderer.material = material;



        Mesh mesh= new Mesh();

        Vector3[] vertices = new Vector3[ (cols+1)*2 + (rols +1 ) *2 ];
        int[] indicies = new int[vertices.Length];
        int v = 0 ;
        for (int i = 0; i <= rols ; i++ )
        {
            vertices[v] = start + new Vector3(0,0.1f,i* cell.y);

            indicies[v] = v;
            v++;

            vertices[v] = start + new Vector3( cols*cell.x,0.1f,i* cell.y);

            indicies[v] = v;
            v++;

        }

        for (int i = 0; i <= cols ; i++ )
        {
            vertices[v] = start + new Vector3(i*cell.x,0.1f,0);
            indicies[v] = v;
            v++;

            vertices[v] = start + new Vector3( i*cell.x,0.1f,rols* cell.y);
            indicies[v] = v;
            v++;

        }

        mesh.vertices = vertices;
        mesh.SetIndices(indicies, MeshTopology.Lines, 0);
        meshFilter.mesh = mesh;


    }
    
}
