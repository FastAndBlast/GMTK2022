using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Segment : MonoBehaviour
{
    public Room[] rooms;
    public EnemyAI[] enemies;
    public Cell[] spwanPoints;
    // Start is called before the first frame update
    void Awake()
    {
        rooms = GetComponentsInChildren<Room>();
        for (int i = 0; i < rooms.Length; i++)
        {
            rooms[i].segment = this;
        }
        enemies = GetComponentsInChildren<EnemyAI>();

        spwanPoints = new Cell[enemies.Length];
        for (int i = 0; i < enemies.Length; i++)
        {
            spwanPoints[i] = enemies[i].currentCell;
        }
    }

    public void spawnEnemies()
    {
        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i].gameObject.transform.position = spwanPoints[i].position;
            enemies[i].gameObject.SetActive(true);
            enemies[i].MoveCell(spwanPoints[i]);
            enemies[i].Health = enemies[i].maxHealth;
            enemies[i].Start();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
