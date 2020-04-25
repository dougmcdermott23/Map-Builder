using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundingBox : MonoBehaviour
{

    public MapGenerator mapGenerator;

    public BoxCollider posX;
    public BoxCollider negX;
    public BoxCollider posY;
    public BoxCollider negY;
    public BoxCollider posZ;
    public BoxCollider negZ;

    public float maxY = 50f;

    void Start()
    {
        UpdateBoundingBox();
    }

    void UpdateBoundingBox()
    {
        float mapX = mapGenerator.mapWidth;
        float mapZ = mapGenerator.mapHeight;

        posX.center = new Vector3(mapX/2, maxY/2, 0);
        negX.center = new Vector3(-mapX/2, maxY/2, 0);
        posY.center = new Vector3(0, maxY, 0);
        negY.center = new Vector3(0, 0, 0);
        posZ.center = new Vector3(0, maxY/2, mapZ/2);
        negZ.center = new Vector3(0, maxY/2, -mapZ/2);

        posX.size = new Vector3(0, maxY, mapZ);
        negX.size = new Vector3(0, maxY, mapZ);
        posY.size = new Vector3(mapX, 0, mapZ);
        negY.size = new Vector3(mapX, 0, mapZ);
        posZ.size = new Vector3(mapX, maxY, 0);
        negZ.size = new Vector3(mapX, maxY, 0);
    }
}
