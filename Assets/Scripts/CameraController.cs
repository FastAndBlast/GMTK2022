using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    public float speed = 0.1f;

    public Transform target;

    private Vector3 offset;

    private float originalY;

    private void Start()
    {
        //target = PlayerController.instance;
        offset = transform.position - target.position;
        originalY = transform.position.y;
    }

    private void FixedUpdate()
    {
        if (!target)
        {
            return;
        }

        Vector3 targetPosition = target.position + offset;
        targetPosition.y = originalY;

        float distance = Vector3.Distance(transform.position, targetPosition) / 2;

        Vector3 newPosition = Vector3.Lerp(transform.position, targetPosition, speed * (1 + distance / 10));

        //newPosition = new Vector3(newPosition.x, newPosition.y, transform.position.z);

        transform.position = newPosition; // + offset;
    }
}
