using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum playerAction { idle, up, right, down, left, attack, block }

public class PlayerController : Entity
{
    private List<playerAction> actions = new List<playerAction>();

    private bool horizontalAxisDown = false;
    private bool verticalAxisDown = false;

    PlayerAnimation anim;

    public static PlayerController instance;

    private void Awake()
    {
        instance = this;
    }

    protected override void Start()
    {
        base.Start();
        anim = GetComponent<PlayerAnimation>();
    }


    private void Update()
    {
        //Vector2 movementVector = Vector2.zero;

        playerAction nextAction = playerAction.idle;

        if (Input.GetAxisRaw("Horizontal") != 0)
        {
            if (!horizontalAxisDown)
            {
                //movementVector = Vector2.right * Input.GetAxisRaw("Vertical");
                nextAction = playerAction.down - (int)Input.GetAxisRaw("Horizontal");
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
                nextAction = playerAction.right - (int)Input.GetAxisRaw("Vertical");
            }
            verticalAxisDown = true;
        }
        else
        {
            verticalAxisDown = false;
        }

        if (Input.GetButtonDown("Attack"))
        {
            nextAction = playerAction.attack;
        }

        if (nextAction != playerAction.idle)
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

        if (actions.Count > 0 && GameManager.currentState == state.wait)
        {
            GameManager.instance.PlayerMove();
        }
    }

    public override void MovementTick()
    {
        if (actions.Count > 0) // && actions[0] > playerAction.idle && actions[0] < playerAction.attack)
        {
            // Movement actions
            if (actions[0] == playerAction.up)
            {
                anim.StartAnimation(Vector2.up);
            }
            else if (actions[0] == playerAction.right)
            {
                anim.StartAnimation(Vector2.right);
            }
            else if (actions[0] == playerAction.down)
            {
                anim.StartAnimation(Vector2.down);
            }
            else if (actions[0] == playerAction.left)
            {
                anim.StartAnimation(Vector2.left);
            }
        }
    }

    public override void ActionTick()
    {
        if (actions.Count > 0)
        {
            if (actions[0] == playerAction.attack)
            {
                // TODO: IMPLEMENT ATTACK
            }
            else if (actions[0] == playerAction.block)
            {
                // TODO: IMPLEMENT BLOCK
            }
        }
    }

    public override void FinalTick()
    {
        actions.RemoveAt(0);
    }

    public override void OnHealthChange(int before, int after)
    {
        print("Player health was changed, please implement hearts: " + after);
    }
}
