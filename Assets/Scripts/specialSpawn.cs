using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class specialSpawn : MonoBehaviour
{
    public GameObject evenEnemy;

    public GameObject oddEnemy;

    public bool spawned = false;
    public bool even;


    public void allTrigger()
    {
        spawned = !spawned;

        bool lastEven = even;
        even = (PlayerController.instance.Number() % 2) == 0;

        GameObject enemy;
        if (even)
        {
            enemy = evenEnemy;
        }
        else
        {
            enemy = oddEnemy;
        }
        DumbEnemy controller = enemy.GetComponent<DumbEnemy>();
        if (spawned)
        {
            enemy.SetActive(true);
            controller.Initialise();
        }
        else
        {
            enemy.SetActive(false);
            controller.currentCell.pathable = true;
            if (GameManager.entities.Contains(controller))
            {
                GameManager.entities.Remove(controller);
            }
        }
    }
}
