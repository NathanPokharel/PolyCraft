using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshCollider))]
public class TerrainGenerator : MonoBehaviour
{
    Mesh mesh;
    MeshCollider meshCollider;
    
    Vector3[] vertices;
    int[] triangles;
    public int xSize = 20;
    public int zSize = 20;
    public float noiseScale = 0.3f;
    public float heightMultiplier = 2f;

    void Awake()
    {
        mesh = new Mesh();
        meshCollider = GetComponent<MeshCollider>();
        GetComponent<MeshFilter>().mesh = mesh;
    }

    public void GenerateTerrain(Vector2 offset)
    {
        CreateShape(offset);
        UpdateMesh();
    }

    void CreateShape(Vector2 offset)
    {
        vertices = new Vector3[(xSize+1)*(zSize+1)];
        int i = 0;
        for (int z = 0; z <= zSize; z++){
            for (int x = 0; x <= xSize; x++){
                float y = Mathf.PerlinNoise((x + offset.x) * noiseScale, (z + offset.y) * noiseScale) * heightMultiplier;
                vertices[i] = new Vector3(x, y, z);
                i++;
            }
        }

        int vert = 0;
        int tris = 0;
        triangles = new int[xSize * zSize * 6];

        for (int z = 0; z < zSize; z++){
            for (int x = 0; x < xSize; x++){
                triangles[0 + tris] = 0 + vert;
                triangles[1 + tris] = vert + xSize + 1;
                triangles[2 + tris] = vert + 1;
                triangles[3 + tris] = vert + 1;
                triangles[4 + tris] = vert + xSize + 1;
                triangles[5 + tris] = vert + xSize + 2;
                vert++;
                tris += 6;
            }
            vert++;
        }
    }

    void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        meshCollider.sharedMesh = mesh; // Update the MeshCollider
    }
}

