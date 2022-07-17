using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Segment : MonoBehaviour
{
    public Room[] rooms;
    public EnemyAI[] enemies;
    public Cell[] spwanPoints;
    // Start is called before the first frame update
    void Start()
    {
        rooms = GetComponentsInChildren<Room>();
        for (int i = 0; i < rooms.Length; i++)
        {
            rooms[i].segment = this;
        }
        enemies = GetComponentsInChildren<EnemyAI>(true);

        spwanPoints = new Cell[enemies.Length];
        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i].Initialise();
            //enemies[i].currentCell = GetComponentInParent<Room>().GetCell(enemies[i].transform.position);
            //enemies[i].currentCell.pathable = false;
            spwanPoints[i] = enemies[i].currentCell;
        }
    }

    public void spawnEnemies()
    {
        for (int i = 0; i < enemies.Length; i++)
        {
            //print(spwanPoints[0]);
            Vector3 newPosition = spwanPoints[i].position + new Vector3(0.5f, 0, 0.5f);
            newPosition.y = enemies[i].gameObject.transform.position.y;
            enemies[i].gameObject.transform.position = newPosition;
            enemies[i].gameObject.SetActive(true);
            enemies[i].MoveCell(spwanPoints[i]);
            enemies[i].Health = enemies[i].maxHealth;
            enemies[i].ReAwake();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
