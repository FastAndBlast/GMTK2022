using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    public int health;
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


    private int maxHealth;

    protected virtual void Start()
    {
        maxHealth = health;
    }

    public virtual void MovementTick()
    {

    }

    public virtual void ActionTick()
    {

    }

    public virtual void ParticleTick()
    {

    }

    // On health change event
    public virtual void OnHealthChange(int before, int after)
    {

    }
}
