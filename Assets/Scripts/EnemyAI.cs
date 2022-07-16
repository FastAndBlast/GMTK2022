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

    EnemyAnimation anim;

    private void Awake()
    {
        anim = GetComponent<EnemyAnimation>();
    }

    public override void MovementTick()
    {
        if (nextAction == enemyAction.move)
        {
            if (Health < 1)
            {
                return;
            }

            List<Cell> path = currentCell.FindPath(PlayerController.instance.currentCell);

            if (path.Count == 0)
            {
                return;
            }

            currentCell = path[1];

            anim.StartMove(currentCell.position);

            // Start enemy movement animation
            /*
            if (currentCell.neighbors[0] == path[1])
            {
                anim.StartMove(Vector2.up);
            }
            else if (currentCell.neighbors[1] == path[1])
            {
                anim.StartMove(Vector2.left);
            }
            else if (currentCell.neighbors[2] == path[1])
            {
                anim.StartMove(Vector2.down);
            }
            else
            {
                anim.StartMove(Vector2.right);
            }
            */
            //
        }
    }

    public override void ActionTick()
    {
        if (Health < 1)
        {
            return;
        }

        if (nextAction == enemyAction.attack)
        {
            //ATTACK
            List<Cell> adjacent = new List<Cell>();

            adjacent.AddRange(currentCell.neighbors);

            adjacent.Add(currentCell.neighbors[1].neighbors[0]);
            adjacent.Add(currentCell.neighbors[1].neighbors[2]);
            adjacent.Add(currentCell.neighbors[3].neighbors[0]);
            adjacent.Add(currentCell.neighbors[3].neighbors[2]);

            if (adjacent.Contains(PlayerController.instance.currentCell))
            {
                PlayerController.instance.Health -= 1;
                // Knockback
            }

            nextAction = enemyAction.move;
        }
    }

    public override void FinalTick()
    {
        if (Health < 1)
        {
            // THIS IS WHERE YOU DIE
            GameManager.entities.Remove(this);
            Destroy(gameObject);
            return;
        }

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

        anim.StartIdle();

        //Display whatever action is gonna happen
    }

    public override void OnHealthChange(int before, int after)
    {
        print("Enemy damaged: " + after);

        if (nextAction != enemyAction.attack)
        {
            anim.StartHit();
        }

        if (Health < 1)
        {
            // DEAD
            anim.StartDie();
        }
    }
}
