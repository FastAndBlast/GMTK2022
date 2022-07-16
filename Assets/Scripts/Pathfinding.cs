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
            int lowestCost = q.costh;
            for (int i = 0; i < open.Count; i++)
            {
                if (open[i].costh < lowestCost)
                {
                    q = open[i];
                    lowestCost = q.costh;
                }
            }

            open.Remove(q);

            for (int i = 0; i < q.neighbors.Length; i++)
            {
                Cell neighbor = q.neighbors[i];
                if (!(neighbor != null && neighbor.pathable))
                {
                    continue;
                }
                neighbor.updateCost(origin, q);
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

                int newF = neighbor.costh + neighbor.calcHeuristic(target);
                if (newF < neighbor.costf)
                {
                    neighbor.costf = newF;
                }

                if (open.Contains(neighbor))
                {
                    continue;
                }

                if (closed.Contains(neighbor))
                {
                    continue;
                }
                
                open.Add(neighbor);
            }

            closed.Add(q);
        }
        // No path exists
        return null;
    }
}
