using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum Action { IDLE, UP, RIGHT, DOWN, LEFT, ATTACK, BLOCK }

public class PlayerController : Entity
{
    private List<Action> actions = new List<Action>();

    private bool horizontalAxisDown = false;
    private bool verticalAxisDown = false;

    PlayerAnimation anim;

    protected override void Start()
    {
        base.Start();
        anim = GetComponent<PlayerAnimation>();
    }


    private void Update()
    {
        //Vector2 movementVector = Vector2.zero;

        Action nextAction = Action.IDLE;

        if (Input.GetAxisRaw("Horizontal") != 0)
        {
            if (!horizontalAxisDown)
            {
                //movementVector = Vector2.right * Input.GetAxisRaw("Vertical");
                nextAction = Action.DOWN - (int)Input.GetAxisRaw("Horizontal");
            }
            horizontalAxisDown = true;
        }
        else
        {
            horizontalAxisDown = false;
        }

        if (Input.GetAxisRaw("Vertical") != 0)
        {
            if (!verticalAxisDown)
            {
                //movementVector = Vector2.up * Input.GetAxisRaw("Vertical");
                nextAction = Action.RIGHT - (int)Input.GetAxisRaw("Vertical");
            }
            verticalAxisDown = true;
        }
        else
        {
            verticalAxisDown = false;
        }

        if (Input.GetButtonDown("Attack"))
        {
            nextAction = Action.ATTACK;
        }

        if (nextAction != Action.IDLE)
        {
            if (actions.Count < 2)
            {
                actions.Add(nextAction);
            }
            else
            {
                actions[1] = nextAction;
            }
        }

        if (actions.Count > 0 && GameManager.currentState == State.WAIT)
        {
            GameManager.instance.PlayerMove();
        }
    }

    public override void MovementTick()
    {
        if (actions.Count > 0) // && actions[0] > Action.IDLE && actions[0] < Action.ATTACK)
        {
            // Movement actions
            if (actions[0] == Action.UP)
            {
                anim.StartAnimation(Vector2.up);
            }
            else if (actions[0] == Action.RIGHT)
            {
                anim.StartAnimation(Vector2.right);
            }
            else if (actions[0] == Action.DOWN)
            {
                anim.StartAnimation(Vector2.down);
            }
            else if (actions[0] == Action.LEFT)
            {
                anim.StartAnimation(Vector2.left);
            }
        }
    }

    public override void ActionTick()
    {
        if (actions.Count > 0)
        {
            if (actions[0] == Action.ATTACK)
            {
                // TODO: IMPLEMENT ATTACK
            }
            else if (actions[0] == Action.BLOCK)
            {
                // TODO: IMPLEMENT BLOCK
            }
        }
    }

    public override void ParticleTick()
    {
        actions.RemoveAt(0);
    }

    public override void OnHealthChange(int before, int after)
    {
        print("Player health was changed, please implement hearts: " + after);
    }
}
