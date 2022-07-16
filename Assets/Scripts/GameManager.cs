using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public static List<Entity> entities;

    private void Awake()
    {
        instance = this;
    }
}
