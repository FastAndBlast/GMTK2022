using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum state { wait, movement, action, final };

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public static List<Entity> entities = new List<Entity>();

    public static state currentState = state.wait;

    public float stateChangeTimer = 5f;

    public float movementTime;
    public float actionTime;
    public float waitTime;

    private void Awake()
    {
        instance = this;
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
                    print(entity.name);
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
    }

    public void PlayerMove()
    {
        currentState = state.movement;
        foreach (Entity entity in entities)
        {
            entity.MovementTick();
        }
        stateChangeTimer = movementTime;
    }
}
