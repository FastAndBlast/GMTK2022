using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    protected int health;
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

    public int maxHealth;

    protected virtual void Start()
    {
        health = maxHealth;

        GameManager.entities.Add(this);
    }

    protected virtual void MoveCell(Cell newCell)
    {
        currentCell.pathable = true;
        currentCell = newCell;
        currentCell.pathable = false;
    }

    public abstract void MovementTick();

    public abstract void ActionTick();

    public abstract void FinalTick();

    public abstract void GetHit(Entity entity, int damage);

    public abstract void GetHit(Vector2 dir, int damage);

    // On health change event
    public abstract void OnHealthChange(int before, int after);
}
