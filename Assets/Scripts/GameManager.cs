using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum state { wait, movement, action, final };

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public static List<Entity> entities = new List<Entity>();
    public static List<Spikes> spikes = new List<Spikes>();

    public static List<Entity> flush = new List<Entity>();
    public static List<Entity> addQueue = new List<Entity>();

    public static state currentState = state.wait;

    public float stateChangeTimer = 5f;

    public float movementTime;
    public float actionTime;
    public float waitTime;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        if (stateChangeTimer > 0)
        {
            stateChangeTimer -= Time.deltaTime;
        }
        else
        {
            if (currentState == state.wait)
            {
                currentState = state.movement;
                foreach (Entity entity in entities)
                {
                    entity.MovementTick();
                    // print(entity.name);
                }
                stateChangeTimer = movementTime;
            }
            else if (currentState == state.movement)
            {
                currentState = state.action;
                foreach (Entity entity in entities)
                {
                    entity.ActionTick();
                }
                foreach (Spikes spike in spikes)
                {
                    spike.ActionTick();
                }
                stateChangeTimer = actionTime;
            }
            else if (currentState == state.action)
            {
                currentState = state.wait;
                foreach (Entity entity in entities)
                {
                    entity.FinalTick();
                }
                stateChangeTimer = waitTime;
            }
            else
            {
                currentState = state.wait;
                stateChangeTimer = waitTime;
            }

            //print("STATE CHANGED: " + currentState);
            //print("STATE TIMER: " + stateChangeTimer);
        }

        foreach (Entity entity in flush)
        {
            entities.Remove(entity);
        }

        flush = new List<Entity>();

        foreach (Entity entity in addQueue)
        {
            if (!entities.Contains(entity))
            {
                entities.Add(entity);
            }
        }

        addQueue = new List<Entity>();
    }

    public void PlayerMove()
    {
        currentState = state.movement;
        foreach (Entity entity in entities)
        {
            entity.MovementTick();
            // print(entity.name);
        }
        stateChangeTimer = movementTime;
    }
}
