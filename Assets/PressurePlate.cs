using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PressurePlate : MonoBehaviour
{
    public UnityEvent onPress;

    public int requiredNumber;

    Cell cell;

    bool invoked = false;

    private void Start()
    {
        cell = GetComponentInParent<Room>().GetCell(transform.position);
    }

    private void Update()
    {
        if (!invoked && PlayerController.instance.currentCell == cell)
        {
            if (PlayerController.instance.number == requiredNumber)
            {
                onPress.Invoke();
                invoked = true;
            }
        }
    }



}
