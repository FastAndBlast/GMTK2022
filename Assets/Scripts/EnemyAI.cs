using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum enemyAction { move, windup, attack }

public class EnemyAI : Entity
{
    [Header("Turns for enemy to finish attack")]
    public float windupLength = 2f;


    public enemyAction nextAction;

    //[HideInInspector]
    private float windupLeft = 3;

    //private List<Cell> path;

    public override void MovementTick()
    {
        if (nextAction == enemyAction.move)
        {
            List<Cell> path = currentCell.FindPath(PlayerController.instance.currentCell);

            if (path.Count == 0)
            {
                return;
            }

            // Start enemy movement animation
        }
    }

    public override void ActionTick()
    {
        if (nextAction == enemyAction.attack)
        {
            //ATTACK
            nextAction = enemyAction.move;
        }
    }

    public override void FinalTick()
    {
        if (nextAction == enemyAction.windup)
        {
            windupLeft -= 1;
            if (windupLeft < 1)
            {
                nextAction = enemyAction.attack;
            }
        }
        else
        {
            if (Vector3.Distance(transform.position, PlayerController.instance.transform.position) < 2)
            {
                nextAction = enemyAction.attack;
            }
        }

        //Display whatever action is gonna happen
    }

    public override void OnHealthChange(int before, int after)
    {
        print("Enemy damaged: " + after);
    }
}
