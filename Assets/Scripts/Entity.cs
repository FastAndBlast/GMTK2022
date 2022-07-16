using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    private int health;
    public int Health
    {
        get
        {
            return health;
        }
        set
        {
            OnHealthChange(health, value);
            health = value;
        }
    
    }

    public Cell currentCell;

    private int maxHealth;

    protected virtual void Start()
    {
        maxHealth = health;

        GameManager.entities.Add(this);
    }

    public abstract void MovementTick();

    public abstract void ActionTick();

    public abstract void FinalTick();

    // On health change event
    public abstract void OnHealthChange(int before, int after);
}
