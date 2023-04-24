using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class R_PlaneGenerator : MonoBehaviour
{
    Mesh mesh;
    new MeshCollider collider;

    Vector3[] vertices;
    int[] triangles;

    public int xSize = 20;
    public int zSize = 20;

    [Header("Terrain Settings")]
    [Range(0, 100)] public float amplitude = 1f;
    [Range(0, 100)] public float frequency = 1f;

    private void Awake()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        CreateShape();
        UpdateMesh();
    }

    private void CreateShape()
    {
        vertices = new Vector3[(xSize + 1) * (zSize + 1)];

        for (int i = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                float y = Mathf.PerlinNoise(x / frequency, z / frequency) * 0.5f;
                y *= amplitude;

                vertices[i] = new Vector3(x, y, z);
                i++;
            }
        }
        triangles = new int[xSize * zSize * 6];
        int vert = 0;
        int tris = 0;
        
        for(int z = 0; z < zSize; z++)
        {
            for (int x = 0; x < xSize; x++)
            {
                triangles[tris] = vert;
                triangles[tris + 1] = vert + xSize + 1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + xSize + 1;
                triangles[tris + 5] = vert + xSize + 2;

                vert++;
                tris += 6;
            }
            vert++;
        }
    }

    private void UpdateMesh()
    {
        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = triangles;

        collider = gameObject.AddComponent<MeshCollider>();
        collider.sharedMesh = mesh;

        mesh.RecalculateNormals();
    }
}
