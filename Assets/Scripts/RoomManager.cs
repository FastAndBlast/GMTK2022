using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public static RoomManager instance;
    public List<Room> rooms;

    void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < rooms.Count; i++)
        {
            for (int j = 0; j < rooms.Count; j++)
            {
                if (i == j)
                {
                    continue;
                }
                rooms[i].SetPathability();
                rooms[i].JoinRooms(rooms[j]);
            }
        }
    }
}
