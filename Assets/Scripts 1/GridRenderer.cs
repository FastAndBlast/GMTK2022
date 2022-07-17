using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridRenderer : MonoBehaviour
{
    LineRenderer lr;

    public MeshFilter targetMesh;

    // Start is called before the first frame update
    void Start()
    {
        lr = GetComponent<LineRenderer>();

        Vector3[] positions = new Vector3[400];

        /*
        for (int x = 0; x < 20; x++)
        {
            for (int y = 0; y < 20; y++)
            {
                positions[x * 20 + y] = new Vector3(x, 0.1f, y);
            }
        }
        */

        Mesh newMesh = new Mesh();

        lr.BakeMesh(newMesh);

        targetMesh.mesh = newMesh;
        targetMesh.sharedMesh = newMesh;

        //lr.positionCount = 400;
        //lr.SetPositions(positions);

        //print(lr.positionCount);
    }
}
