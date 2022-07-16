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
    public int width;
    public int height;

    private List<Object> contents;

    public Cell[,] roomGrid;

    public Vector3Int originPos;
    public Vector3Int topRight;

    private void Awake()
    {
        roomGrid = new Cell[width, height];
        originPos = Vector3Int.FloorToInt(gameObject.transform.position);
        topRight = originPos + width * Vector3Int.right + height * Vector3Int.forward;
        MakeCells();
        RoomManager.instance.rooms.Add(this);
    }

    private void Start()
    {
    }

    private void MakeCells()
    {
        Cell last = null;
        for (int i = 0; i < width; i++)
        {
            // Create a cell moving to the right
            Cell newCell = new Cell();
            roomGrid[i, 0] = newCell;
            if (i != 0)
            {
                newCell.position = last.position + Vector3.right;
                last.neighbors[(int)direction.right] = newCell;
                newCell.neighbors[(int)direction.left] = newCell;
            }
            else
            {
                newCell.position = originPos;
            }

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
        Vector3 centre = new Vector3(originPos.x + (float)width / 2, 1, originPos.z + (float)height / 2);
        Vector3Int size = new Vector3Int(width, 2, height);
        Gizmos.DrawWireCube(centre, size);
    }

    public void JoinRooms(Room other)
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

        // We check if the edges are along the same line
        if (other.originPos.z == topRight.z)
        {
            // Horizontal
            int start = other.originPos.x - originPos.x;
            int end = other.topRight.x - originPos.x - 1;
            
            // There is no overlap
            if (end < 0 || start > height)
            {
                return;
            }

            //Debug.Log(gameObject.name + " [" + start + "," + end + "]");
            //Debug.Log("[" + 0 + "," + (width - 1) + "]");
            for (int i = 0; i < width; i++)
            {
                if (start <= i && i <= end)
                {
                    //Debug.Log(i + " " + (i - start));
                    Cell ours = roomGrid[i, height - 1];
                    Cell theirs = other.roomGrid[i - start, 0];
                    ours.neighbors[(int)direction.right] = theirs;
                    theirs.neighbors[(int)direction.left] = ours;
                }
            }
        }
        else if (other.originPos.x == topRight.x)
        {
            // Vertical
            int start = other.originPos.z - originPos.z;
            int end = other.topRight.z - originPos.z - 1;
            
            // There is no overlap
            if (end < 0 || start > width)
            {
                return;
            }

            // Debug.Log(gameObject.name + " [" + start + "," + end + "]");
            // Debug.Log("[" + 0 + "," + (height - 1) + "]");
            for (int i = 0; i < height; i++)
            {
                if (start <= i && i <= end)
                {
                    // Debug.Log(i + " " + (i - start));
                    Cell ours = roomGrid[width - 1, i];
                    Cell theirs = other.roomGrid[0, i - start];
                    ours.neighbors[(int)direction.right] = theirs;
                    theirs.neighbors[(int)direction.left] = ours;
                }
            }
        }
    }
}