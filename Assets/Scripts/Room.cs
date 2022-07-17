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
    public Vector3Int position;
    public bool pathable = true;

    private Cell prevOrigin = null;
    public int costh = -1;
    public int costf = -1;
    public Cell previous = null;
    public Room room;
    
    public List<Cell> FindPath(Cell target)
    {
        return Pathfinding.Path(target, this);
    }

    public bool updateCost(Cell target, Cell origin, Cell prev)
    {
        if (origin != prevOrigin)
        {
            costh = prev.costh + 1;
            costf = costh + calcHeuristic(target);
            previous = prev;
            prevOrigin = origin;
            return true;
        }
        else
        {
            if (prev.costh + 1 < costh)
            {
                previous = prev;
                costh = prev.costh + 1;
            }
            int newF = costh + calcHeuristic(target);
            if (newF < costf)
            {
                costf = newF;
                return true;
            }
            return false;
        }
    }

    public int calcHeuristic(Cell target)
    {
        // Could cache this to avoid calling so often
        return Mathf.Abs(position.x - target.position.x) + Mathf.Abs(position.z - target.position.z);
    }
}

public class Room : MonoBehaviour
{
    public int width;
    public int height;

    private List<Transform> obstacles = new List<Transform>();

    public Cell[,] roomGrid;

    public Vector3Int originPos;
    public Vector3Int topRight;
    public Segment segment;

    public void Awake()
    {
        originPos = Vector3Int.FloorToInt(gameObject.transform.position);
        topRight = originPos + width * Vector3Int.right + height * Vector3Int.forward;
        MakeCells();
        RoomManager.rooms.Add(this);

        GetChildrenWithTag(transform, "object");
    }

    private void GetChildrenWithTag(Transform parent, string tag) 
    {
        
        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);
            if (child.tag == tag)
            {
                obstacles.Add(child);
            }
            if (child.childCount > 0)
            {
                GetChildrenWithTag(child, tag);
            }
        }
    }

    private void Start()
    {
    }

    public void MakeCells()
    {
        roomGrid = new Cell[width, height];
        Cell last = null;
        for (int i = 0; i < width; i++)
        {
            // Create a cell moving to the right
            Cell newCell = new Cell();
            roomGrid[i, 0] = newCell;
            newCell.room = this;
            if (i != 0)
            {
                newCell.position = last.position + Vector3Int.right;
                last.neighbors[(int)direction.right] = newCell;
                newCell.neighbors[(int)direction.left] = last;
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
                upCell.position = last.position + Vector3Int.forward;
                last.neighbors[(int)direction.up] = upCell;
                upCell.neighbors[(int)direction.down] = last;
                last = upCell;
                upCell.room = this;

                if (i > 0)
                {
                    Cell left = roomGrid[i - 1, j];
                    left.neighbors[(int)direction.right] = upCell;
                    upCell.neighbors[(int)direction.left] = left;
                }
            }
            last = newCell;
        }
    }

    public Cell GetCell(Vector3 position)
    {
        Vector3Int pos = Vector3Int.FloorToInt(position) - originPos;
        if (pos.x >= 0 && pos.z >= 0 && pos.x < width && pos.z < height)
        {
            Cell cell = roomGrid[pos.x, pos.z];
            //Debug.Log(cell.position.x + " " + cell.position.z);
            return cell;
        }
        else
        {
            Debug.LogError("An object tried to find its cell in a room it is outside of. You must call GetCell on the correct room.");
            return null;
        }

    }

    private void OnDrawGizmos()
    {
        originPos = Vector3Int.FloorToInt(gameObject.transform.position);
        Vector3 centre = new Vector3(originPos.x + (float)width / 2, 1, originPos.z + (float)height / 2);
        Vector3Int size = new Vector3Int(width, 2, height);
        Gizmos.DrawWireCube(centre, size);
    }

    public void SetPathability()
    {
        for (int i = 0; i < obstacles.Count; i++)
        {
            Transform obj = obstacles[i];
            Cell cell = GetCell(obj.position);
            print("Making cell " + obj.position.x + "," + obj.position.z + " unpathable.");
            if (cell == null)
            {
                return;
            }
            cell.pathable = false;
        }
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

                    //Debug.Log(ours.position + " | " + theirs.position);

                    ours.neighbors[(int)direction.up] = theirs;
                    theirs.neighbors[(int)direction.down] = ours;
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
                    //Debug.Log(i + " " + (i - start));
                    Cell ours = roomGrid[width - 1, i];
                    Cell theirs = other.roomGrid[0, i - start];
                    ours.neighbors[(int)direction.right] = theirs;
                    theirs.neighbors[(int)direction.left] = ours;
                }
            }
        }
    }
}