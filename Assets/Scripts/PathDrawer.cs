using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathDrawer : MonoBehaviour
{
    // Class to test pathfinding
    private Cell origin;
    private Cell target;

    public Room room;
    public GameObject endpoint;
    public Room endpointRoom;
    private void OnDrawGizmosSelected()
    {
        origin = room.GetCell(gameObject.transform.position);
        target = endpointRoom.GetCell(endpoint.transform.position);
        List<Cell> path = Pathfinding.Path(target, origin);
        for (int i = 0; i < path.Count; i++)
        {
            Vector3 centre = path[i].position + new Vector3(0.5f, 0, 0.5f);
            Gizmos.DrawWireSphere(centre, 0.4f);
        }           
    }
}
