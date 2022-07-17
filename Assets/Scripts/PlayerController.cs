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

        currentCell = new Cell();
        currentCell.neighbors = new Cell[4] { currentCell, currentCell, currentCell, currentCell };
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
                if (currentCell.neighbors[(int)direction.up].pathable)
                {
                    currentCell = currentCell.neighbors[(int)direction.up];
                    anim.StartMove(Vector2.up);
                    //anim.StartKnockback(Vector2.up, 2);
                }
            }
            else if (actions[0] == playerAction.right)
            {
                if (currentCell.neighbors[(int)direction.right].pathable)
                {
                    currentCell = currentCell.neighbors[(int)direction.right];
                    anim.StartMove(Vector2.right);
                }
            }
            else if (actions[0] == playerAction.down)
            {
                if (currentCell.neighbors[(int)direction.down].pathable)
                {
                    currentCell = currentCell.neighbors[(int)direction.down];
                    anim.StartMove(Vector2.down);
                }
            }
            else if (actions[0] == playerAction.left)
            {
                if (currentCell.neighbors[(int)direction.left].pathable)
                {
                    currentCell = currentCell.neighbors[(int)direction.left];
                    anim.StartMove(Vector2.left);
                }
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

    public override void GetHit(Entity entity, int damage)
    {
        Health -= damage;

        Vector3 dir = currentCell.position - entity.currentCell.position;

        dir.y = dir.z;

        int length = 0;

        Cell targetCell = currentCell;

        if (dir.x == 1)
        {
            for (int i = 0; i < 3; i++)
            {
                if (targetCell.neighbors[(int)direction.right] != null)
                {
                    targetCell = targetCell.neighbors[(int)direction.right];
                    length++;
                }
            }
        }
        else if (dir.x == -1)
        {
            for (int i = 0; i < 3; i++)
            {
                if (targetCell.neighbors[(int)direction.left] != null)
                {
                    targetCell = targetCell.neighbors[(int)direction.left];
                    length++;
                }
            }
        }
        else if (dir.y == 1)
        {
            for (int i = 0; i < 3; i++)
            {
                if (targetCell.neighbors[(int)direction.up] != null)
                {
                    targetCell = targetCell.neighbors[(int)direction.up];
                    length++;
                }
            }
        }
        else if (dir.y == -1)
        {
            for (int i = 0; i < 3; i++)
            {
                if (targetCell.neighbors[(int)direction.down] != null)
                {
                    targetCell = targetCell.neighbors[(int)direction.down];
                    length++;
                }
            }
        }

        if (length > 0)
        {
            anim.StartKnockback(dir, length);
        }
    }

    public override void GetHit(Vector2 dir, int damage)
    {
        Health -= damage;

        //Vector3 dir = currentCell.position - entity.currentCell.position;

        //dir.y = dir.z;

        int length = 0;

        Cell targetCell = currentCell;

        if (dir.x == 1)
        {
            for (int i = 0; i < 3; i++)
            {
                if (targetCell.neighbors[(int)direction.right] != null)
                {
                    targetCell = targetCell.neighbors[(int)direction.right];
                    length++;
                }
            }
        }
        else if (dir.x == -1)
        {
            for (int i = 0; i < 3; i++)
            {
                if (targetCell.neighbors[(int)direction.left] != null)
                {
                    targetCell = targetCell.neighbors[(int)direction.left];
                    length++;
                }
            }
        }
        else if (dir.y == 1)
        {
            for (int i = 0; i < 3; i++)
            {
                if (targetCell.neighbors[(int)direction.up] != null)
                {
                    targetCell = targetCell.neighbors[(int)direction.up];
                    length++;
                }
            }
        }
        else if (dir.y == -1)
        {
            for (int i = 0; i < 3; i++)
            {
                if (targetCell.neighbors[(int)direction.down] != null)
                {
                    targetCell = targetCell.neighbors[(int)direction.down];
                    length++;
                }
            }
        }

        if (length > 0)
        {
            anim.StartKnockback(dir, length);
        }
    }

    public override void OnHealthChange(int before, int after)
    {
        print("Player health was changed, please implement hearts: " + after);
    }
}
