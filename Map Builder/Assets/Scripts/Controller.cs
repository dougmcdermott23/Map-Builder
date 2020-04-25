using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public MapGenerator mapGenerator;
    public MeshEditor meshEditor;

    public LayerMask collisionMask;
    public LayerMask selectionMask;

    float playerBuffer = 0.015f;

    bool vertexSelected;
    int vertexIndexSelected;

    public void Move(Vector3 inputMovement)
    {
        Vector3 movePlayer = new Vector3();
        RaycastHit hit;
        Ray ray = new Ray(transform.position, inputMovement.normalized);

        if (Physics.Raycast(ray, out hit, inputMovement.magnitude, collisionMask))
        {
            float moveDistance = hit.distance - playerBuffer;
            Vector3 moveDirection = inputMovement.normalized;
            movePlayer = moveDirection * moveDistance;
        }
        else
        {
            movePlayer = inputMovement;
        }

        transform.position += movePlayer;
    }

    public void SelectQuad()
    {
        RaycastHit hit;

        if (GetMeshPoint(out hit))
            HighlightObject(hit.collider, hit.point);
    }

    public void UnselectQuad()
    {
        mapGenerator.selectedPoint = new Vector2(-1, -1);
        mapGenerator.GenerateMap();
    }

    public void SetVertex()
    {
        RaycastHit hit;

        if (GetMeshPoint(out hit))
        {
            vertexIndexSelected = IntegerClosestIndex(hit.collider, hit.point);
            vertexSelected = true;
        }
    }

    public void UnsetVertex()
    {
        vertexSelected = false;
    }

    public void MoveVertex(float vertical)
    {
        meshEditor.DoAction(vertexIndexSelected, vertical);
    }

    bool GetMeshPoint(out RaycastHit hit)
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.farClipPlane));

        if (Physics.Linecast(transform.position, mouseWorldPos, out hit, selectionMask))
            return true;
        else
            return false;
    }

    void HighlightObject(Collider collider, Vector3 point)
    {
        float mapWidth = mapGenerator.mapWidth;
        float mapHeight = mapGenerator.mapHeight;

        Mesh mesh = collider.GetComponent<MeshFilter>().mesh;

        Vector2 vertexIndex = VectorClosestIndex(collider, point);

        int index = (int)(vertexIndex.y * mapWidth + vertexIndex.x);
        Vector3 nearestVertex = mesh.vertices[index];

        if (point.x <= nearestVertex.x && point.z > nearestVertex.z)
            mapGenerator.selectedPoint = new Vector2(vertexIndex.x - 1, vertexIndex.y - 1);
        if (point.x > nearestVertex.x && point.z > nearestVertex.z)
            mapGenerator.selectedPoint = new Vector2(vertexIndex.x, vertexIndex.y - 1);
        if (point.x <= nearestVertex.x && point.z <= nearestVertex.z)
            mapGenerator.selectedPoint = new Vector2(vertexIndex.x - 1, vertexIndex.y);
        if (point.x > nearestVertex.x && point.z <= nearestVertex.z)
            mapGenerator.selectedPoint = new Vector2(vertexIndex.x, vertexIndex.y);

        mapGenerator.GenerateMap();
    }

    Vector2 VectorClosestIndex(Collider collider, Vector3 point)
    {
        float mapWidth = mapGenerator.mapWidth;
        float mapHeight = mapGenerator.mapHeight;

        Mesh mesh = collider.GetComponent<MeshFilter>().mesh;
        Vector3 nearestVertex = new Vector3();
        float minDistance = float.PositiveInfinity;

        foreach (Vector3 vertex in mesh.vertices)
        {
            float distance = Vector3.Distance(vertex, point);

            if (distance < minDistance)
            {
                minDistance = distance;
                nearestVertex = vertex;
            }
        }

        return new Vector2((mapWidth - 1) / 2 + nearestVertex.x, (mapHeight - 1) / 2 - nearestVertex.z);
    }

    int IntegerClosestIndex(Collider collider, Vector3 point)
    {
        float mapWidth = mapGenerator.mapWidth;
        float mapHeight = mapGenerator.mapHeight;

        Vector2 vertexIndex = VectorClosestIndex(collider, point);
        return (int)(vertexIndex.y * mapWidth + vertexIndex.x);
    }
}
