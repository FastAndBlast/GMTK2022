using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    Cell cell;

    public float timeToOpen = 0.5f;

    public float endY;

    float timeLeft;

    Transform doorTransform;

    Vector3 originalPosition;

    bool closing = false;

    private void Start()
    {
        print(transform.position);
        cell = GetComponentInParent<Room>().GetCell(transform.position);

        cell.pathable = false;
        doorTransform = transform.Find("Door");
        originalPosition = doorTransform.position;
    }

    private void Update()
    {
        if (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;

            if (closing)
            {
                doorTransform.position = new Vector3(doorTransform.position.x, Mathf.Lerp(originalPosition.y, endY, (timeLeft - timeToOpen) / timeToOpen), doorTransform.position.z);
            }
            else
            {
                doorTransform.position = new Vector3(doorTransform.position.x, Mathf.Lerp(endY, originalPosition.y, (timeLeft - timeToOpen) / timeToOpen), doorTransform.position.z);
            }
        }
    }

    public void Open()
    {
        cell.pathable = true;
        timeLeft = timeToOpen;
        closing = false;
    }

    public void Close()
    {
        cell.pathable = false;
        timeLeft = timeToOpen;
        closing = true;
    }


}
