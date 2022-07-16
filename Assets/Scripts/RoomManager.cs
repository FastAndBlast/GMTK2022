using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public static RoomManager instance;
    public static List<Room> rooms;

    void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < RoomManager.rooms.Count; i++)
        {
            for (int j = 0; j < RoomManager.rooms.Count; j++)
            {
                if (i == j)
                {
                    continue;
                }
                RoomManager.rooms[i].SetPathability();
                RoomManager.rooms[i].JoinRooms(RoomManager.rooms[j]);
            }
        }
    }
}
