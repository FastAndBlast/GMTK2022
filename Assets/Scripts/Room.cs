using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum direction
{
    up,
    left,
    down,
    right
}

public class Cell
{
    public Cell[] neighbors = new Cell[4];
    // The position is relative to the parent
    public Vector3 position;
}

public class Object
{
    public bool pathable;
}

public class Room : MonoBehaviour
{
    public Room test;
    public int width;
    public int height;

    private List<Object> contents;

    private Cell[,] roomGrid;

    public Vector3Int originPos;
    public Vector3Int topRight;

    private void Start()
    {
        roomGrid = new Cell[width, height];
        originPos = Vector3Int.FloorToInt(gameObject.transform.position);
        topRight = originPos + width * Vector3Int.right + height * Vector3Int.forward;
        MakeCells();
        JoinRooms(test);
    }

    private void MakeCells()
    {
        Cell first = new Cell();
        first.position = originPos;
        roomGrid[0, 0] = first;
        Cell last = first;
        for (int i = 1; i < width; i++)
        {
            // Create a cell moving to the right
            Cell newCell = new Cell();
            newCell.position = last.position + Vector3.right;
            roomGrid[i, 0] = newCell;
            last.neighbors[(int)direction.right] = newCell;
            newCell.neighbors[(int)direction.left] = newCell;

            // Extend that cells column upwards
            last = newCell;
            for (int j = 1; j < height; j++)
            {
                Cell upCell = new Cell();
                roomGrid[i, j] = upCell;
                upCell.position = last.position + Vector3.forward;
                last.neighbors[(int)direction.down] = upCell;
                upCell.neighbors[(int)direction.up] = last;
                last = upCell;
            }
            last = newCell;
        }
    }

    private void OnDrawGizmos()
    {
        originPos = Vector3Int.FloorToInt(gameObject.transform.position);
        Vector3Int centre = new Vector3Int(originPos.x +  width / 2, 1, originPos.z + height / 2);
        Vector3Int size = new Vector3Int(width, 2, height);
        Gizmos.DrawWireCube(centre, size);
    }

    private void JoinRooms(Room other)
    // We assume that the rooms don't overlap
    {
        // Rooms connect right and up, connections on the other sides are
        // achieved by calling this on other one

        // First check if there are any possible intersections

        if (topRight == other.originPos)
        {
            // Diagonal rooms can't connect
            return;
        }

        // Vertical
        int start = other.originPos.x - originPos.x;
        int end = other.topRight.x - originPos.x;
        
        // There is no overlap
        if (end < 0 || start > width)
        {
            return;
        }

        List<int> ours = new List<int>();
        List<int> thiers = new List<int>();
        for (int i = 0; i < width; i++)
        {
            if (start <= i && end <= i)
            {
                ours.Add(i);
                thiers.Add(i - start);
                Debug.Log(i + " " + (i - start));
            }
        }
    }

    // private void JoinRooms(Room other)
    // // We assume that the rooms don't overlap
    // {
    //     // Rooms connect right and up, connections on the other sides are
    //     // achieved by calling this on other one

    //     int vertOverlap = -1;
    //     int horiOverlap = -1;

    //     // First check if there are any possible intersections
    //     Vector3Int topRight = originPos + width * Vector3Int.right + height * Vector3Int.forward;

    //     if (topRight == other.originPos)
    //     {
    //         // Diagonal rooms can't connect
    //         return;
    //     }

    //     if (topRight.x <= other.originPos.x && originPos.x >= other.originPos.x)
    //     {
    //         horiOverlap = topRight.x - other.originPos.x;
    //     }
    //     if (topRight.z <= other.originPos.z && originPos.z >= other.originPos.z)
    //     {
    //         vertOverlap = topRight.z - other.originPos.z;
    //     }

    //     // The two have no connection
    //     if (vertOverlap < 0 || horiOverlap < 0)
    //     {
    //         return;
    //     }


    //     if (vertOverlap > 0 && horiOverlap > 0)
    //     {
    //         Debug.LogError("Two rooms are overlapping, this is not allowed");
    //         return;
    //     }

    //     if (vertOverlap == 0)
    //     {
    //         // The connection is along the top edge

    //         // Account for the other object not reaching the top right
    //         int stop = Mathf.Min(horiOverlap, other.height);
    //         //for (int i = topRight.z - horiOverlap; i < topRight.z - stop; i++)
    //         for(int i = 0; i < stop; i++)
    //         {
    //             int index = topRight.z - horiOverlap + i;
    //             Cell ours = roomGrid[width - 1, index];
    //             Cell thiers = other.roomGrid[0, i];

    //         }
    //     }
    //     else
    //     {
    //         // The connection is along the right edge
    //     }
    // }
}