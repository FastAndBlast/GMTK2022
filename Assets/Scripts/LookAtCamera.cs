using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{


    private void Update()
    {
        transform.forward = transform.position - Camera.main.transform.position;
    }
}
