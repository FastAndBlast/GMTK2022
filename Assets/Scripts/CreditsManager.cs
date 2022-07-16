using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsManager : MonoBehaviour
{
    public float delay;
    public float scrollSpeed;

    // Update is called once per frame
    void Update()
    {
        if (delay > 0)
        {
            delay -= Time.deltaTime;
        }
        else
        {
            transform.position -= Vector3.up * scrollSpeed * Time.deltaTime;
        }
    }
}
