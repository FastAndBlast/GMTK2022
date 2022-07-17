using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public static RoomManager instance;
    public static List<Room> rooms = new List<Room>();

    void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < RoomManager.rooms.Count; i++)
        {
            RoomManager.rooms[i].SetPathability();
            for (int j = 0; j < RoomManager.rooms.Count; j++)
            {
                if (i == j)
                {
                    continue;
                }
                RoomManager.rooms[i].JoinRooms(RoomManager.rooms[j]);
            }
        }
    }
}
