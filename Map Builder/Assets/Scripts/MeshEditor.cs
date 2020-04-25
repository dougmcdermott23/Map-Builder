using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshEditor : MonoBehaviour
{
    Mesh originalMesh;
    Mesh clonedMesh;
    MeshFilter meshFilter;
    MeshRenderer meshRenderer;
    int[] triangles;

    [HideInInspector]
    public Vector3[] vertices;

    [HideInInspector]
    public bool isCloned = false;

    public float radius = 0.2f;
    public float pull = 0.3f;
    public float handleSize = 0.03f;
    public List<int>[] connectedVertices;
    public List<Vector3[]> allTriangleList;
    public bool moveVertexPoint = true;

    private void Start()
    {
        InitMesh();
    }

    public void InitMesh()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();
        originalMesh = meshFilter.sharedMesh;
        clonedMesh = new Mesh();

        clonedMesh.name = "clone";
        clonedMesh.vertices = originalMesh.vertices;
        clonedMesh.triangles = originalMesh.triangles;
        clonedMesh.normals = originalMesh.normals;
        clonedMesh.uv = originalMesh.uv;
        meshFilter.mesh = clonedMesh;

        vertices = clonedMesh.vertices;
        triangles = clonedMesh.triangles;
        isCloned = true;
        Debug.Log("Init and Cloned");
    }

    public void Reset()
    {
        if (clonedMesh != null && originalMesh != null)
        {
            clonedMesh.vertices = originalMesh.vertices;
            clonedMesh.triangles = originalMesh.triangles;
            clonedMesh.normals = originalMesh.normals;
            clonedMesh.uv = originalMesh.uv;
            meshFilter.mesh = clonedMesh;

            vertices = clonedMesh.vertices;
            triangles = clonedMesh.triangles;
        }
    }

    public void DoAction(int index, float moveAmount)
    {
        Vector3 newPos = vertices[index] + new Vector3(0, moveAmount, 0);
        PullOneVertex(index, newPos);
    }

    private void PullOneVertex(int index, Vector3 newPos)
    {
        vertices[index] = newPos;
        clonedMesh.vertices = vertices;
        clonedMesh.RecalculateNormals();
        DrawMesh();
    }

    public void DrawMesh()
    {
        meshFilter.sharedMesh = null;
        meshFilter.sharedMesh = clonedMesh;

        GetComponent<MeshCollider>().sharedMesh = null;
        GetComponent<MeshCollider>().sharedMesh = meshFilter.mesh;
    }
}
