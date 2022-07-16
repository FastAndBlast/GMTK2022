using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum Action { IDLE, UP, RIGHT, DOWN, LEFT, ATTACK, BLOCK }

public class PlayerController : Entity
{
    private List<Action> actions;

    private bool horizontalAxisDown = false;
    private bool verticalAxisDown = false;



    protected override void Start()
    {
        base.Start();
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
        if (Input.GetAxisRaw("Vertical") != 0)
        {
            if (!verticalAxisDown)
            {
                //movementVector = Vector2.up * Input.GetAxisRaw("Vertical");
                nextAction = Action.RIGHT - (int)Input.GetAxisRaw("Vertical");
            }
            verticalAxisDown = true;
        }

        if (actions.Count < 1)
        {
            actions.Add(nextAction);
        }
        else
        {
            actions[1] = nextAction;
        }

    }

}
