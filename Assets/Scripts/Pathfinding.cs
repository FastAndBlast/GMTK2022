using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding
{
    private static List<Cell> open;
    private static List<Cell> closed;

    public static List<Cell> Path(Cell target, Cell origin)
    {
        open = new List<Cell>();
        closed = new List<Cell>();
        if (target == null || origin == null)
        {
            Debug.LogError("Attempted pathfinding with null target or origin");
            return null;
        }
        origin.costh = 0;
        origin.costf = origin.calcHeuristic(target);
        open.Add(origin);
        while (open.Count > 0)
        {
            Cell q = open[0];
            int lowestCost = q.costf;
            for (int i = 0; i < open.Count; i++)
            {
                if (open[i].costf < lowestCost)
                {
                    q = open[i];
                    lowestCost = q.costf;
                }
            }

            open.Remove(q);

            for (int i = 0; i < q.neighbors.Length; i++)
            {
                Cell neighbor = q.neighbors[i];
                if (neighbor == null || !neighbor.pathable)
                {
                    // Debug.Log("Unpathable, isNull" + (neighbor == null) + " from " + q.position.x + " " + q.position.z + " in direction" + i);
                    continue;
                }
                bool updated = neighbor.updateCost(target, origin, q);
                if (neighbor == target)
                {
                    // Backtrack to reconstruct the path
                    List<Cell> output = new List<Cell>();
                    output.Add(target);
                    Cell current = q;
                    while (current != origin)
                    {
                        output.Add(current);
                        current = current.previous;
                    }
                    output.Add(origin);
                    output.Reverse();
                    return output;
                }

                if (open.Contains(neighbor) && !updated)
                {
                    continue;
                }

                if (closed.Contains(neighbor))
                {
                    if (updated)
                    {
                        closed.Remove(neighbor);
                    }
                    else
                    {
                        continue;
                    }
                }
                
                open.Add(neighbor);
            }
            // Debug.Log("closed " + q.position.x + " " + q.position.z);
            closed.Add(q);
        }
        Debug.Log("No path found");
        // No path exists
        return null;
    }
}
