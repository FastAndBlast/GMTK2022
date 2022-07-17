using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using FMODUnity;

public class PressurePlate : MonoBehaviour
{
    public UnityEvent onPress;

    public int requiredNumber;

    Cell cell;
    public bool triggerMultipleTimes;

    bool invoked = false;

    private void Start()
    {
        cell = GetComponentInParent<Room>().GetCell(transform.position);
    }

    private void Update()
    {
        if (!invoked && PlayerController.instance.currentCell == cell)
        {
            if (PlayerController.instance.Number() == requiredNumber || requiredNumber == 0)
            {
                onPress.Invoke();
                invoked = true;
                RuntimeManager.PlayOneShot("event:/Objects/Pressure_Plate");

                // If it is a new segment pressure plate, set the spawn point and initialise the segment
                if (requiredNumber == 0)
                {
                    PlayerController.instance.SetSpawnPoint(cell);
                    cell.room.segment.spawnEnemies();
                }
            }
        }
        if (PlayerController.instance.currentCell != cell && triggerMultipleTimes)
        {
            invoked = false;
        }
    }



}
