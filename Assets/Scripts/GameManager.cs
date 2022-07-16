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
                }
                stateChangeTimer = 0.25f;
            }
            else if (currentState == state.movement)
            {
                currentState = state.action;
                foreach (Entity entity in entities)
                {
                    entity.ActionTick();
                }
                stateChangeTimer = 0.5f;
            }
            else if (currentState == state.action)
            {
                currentState = state.final;
                foreach (Entity entity in entities)
                {
                    entity.FinalTick();
                }
                stateChangeTimer = 0f;
            }
            else
            {
                currentState = state.wait;
                stateChangeTimer = 5f;
            }
        }
    }

    public void PlayerMove()
    {
        currentState = state.movement;
        foreach (Entity entity in entities)
        {
            entity.MovementTick();
        }
        stateChangeTimer = 0.25f;
    }
}
