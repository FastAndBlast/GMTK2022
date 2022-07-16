using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State { WAIT, MOVEMENT, ACTION, PARTICLE };

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public static List<Entity> entities = new List<Entity>();

    public static State currentState = State.WAIT;

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
            if (currentState == State.WAIT)
            {
                currentState = State.MOVEMENT;
                foreach (Entity entity in entities)
                {
                    entity.MovementTick();
                }
                stateChangeTimer = 0.25f;
            }
            else if (currentState == State.MOVEMENT)
            {
                currentState = State.ACTION;
                foreach (Entity entity in entities)
                {
                    entity.ActionTick();
                }
                stateChangeTimer = 0.5f;
            }
            else if (currentState == State.ACTION)
            {
                currentState = State.PARTICLE;
                foreach (Entity entity in entities)
                {
                    entity.ParticleTick();
                }
                stateChangeTimer = 0f;
            }
            else
            {
                currentState = State.WAIT;
                stateChangeTimer = 5f;
            }
        }
    }

    public void PlayerMove()
    {
        currentState = State.MOVEMENT;
        foreach (Entity entity in entities)
        {
            entity.MovementTick();
        }
        stateChangeTimer = 0.25f;
    }
}
