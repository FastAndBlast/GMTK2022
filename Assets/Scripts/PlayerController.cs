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

    public int damage;

    public Room startingRoom;

    public static PlayerController instance;

    [HideInInspector]
    public Transform rotationAfterAnimation;

    //private int number;
    private Entity target;

    public int number
    {
        get
        {
            Vector3 up = rotationAfterAnimation.InverseTransformDirection(Vector3.up);

            if (up == Vector3.up)
            {
                return 1;
            }
            else if (up == Vector3.right)
            {
                return 2;
            }
            else if (up == Vector3.forward)
            {
                return 3;
            }
            else if (up == Vector3.back)
            {
                return 4;
            }
            else if (up == Vector3.left)
            {
                return 5;
            }
            else
            {
                return 6;
            }
        }
    }
    

    private void Awake()
    {
        instance = this;

        rotationAfterAnimation = new GameObject().transform;//Instantiate(GameObject.).transform;
    }

    protected override void Start()
    {
        health = maxHealth;

        GameManager.entities.Insert(0, this);

        anim = GetComponent<PlayerAnimation>();

        //currentCell.neighbors = new Cell[4] { currentCell, currentCell, currentCell, currentCell };

        //MoveCell(targetCell);
        currentCell = startingRoom.GetCell(transform.position);
        currentCell.pathable = false;
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
                if (currentCell.neighbors[(int)direction.up] != null)
                {
                    if (currentCell.neighbors[(int)direction.up].pathable)
                    {
                        MoveCell(currentCell.neighbors[(int)direction.up]);
                        anim.StartMove(Vector2.up);
                    }
                    else
                    {
                        Entity enemy = GetEnemyAtCell(currentCell.neighbors[(int)direction.up]);
                        if (enemy != null)
                        {
                            target = enemy;
                        }
                    }
                }
            }
            else if (actions[0] == playerAction.right)
            {
                if (currentCell.neighbors[(int)direction.right] != null)
                {
                    if (currentCell.neighbors[(int)direction.right].pathable)
                    {
                        MoveCell(currentCell.neighbors[(int)direction.right]);
                        anim.StartMove(Vector2.right);
                    }
                    else
                    {
                        Entity enemy = GetEnemyAtCell(currentCell.neighbors[(int)direction.right]);
                        if (enemy != null)
                        {
                            target = enemy;
                        }
                    }
                }
            }
            else if (actions[0] == playerAction.down)
            {
                if (currentCell.neighbors[(int)direction.down] != null)
                {
                    if (currentCell.neighbors[(int)direction.down].pathable)
                    {
                        MoveCell(currentCell.neighbors[(int)direction.down]);
                        anim.StartMove(Vector2.down);
                    }
                    else
                    {
                        Entity enemy = GetEnemyAtCell(currentCell.neighbors[(int)direction.down]);
                        if (enemy != null)
                        {
                            target = enemy;
                        }
                    }
                }
            }
            else if (actions[0] == playerAction.left)
            {
                if (currentCell.neighbors[(int)direction.left] != null)
                {
                    if (currentCell.neighbors[(int)direction.left].pathable)
                    {
                        MoveCell(currentCell.neighbors[(int)direction.left]);
                        anim.StartMove(Vector2.left);
                    }
                    else
                    {
                        Entity enemy = GetEnemyAtCell(currentCell.neighbors[(int)direction.left]);
                        if (enemy != null)
                        {
                            target = enemy;
                        }
                    }
                }
            }

            //print(number);
        }
    }

    public override void ActionTick()
    {
        if (actions.Count > 0)
        {
            if (target != null)
            {
                anim.StartAttack(target);
                target.GetHit(this, damage);
                target = null;
            }
            
            /*
            if (actions[0] == playerAction.attack)
            {
                // TODO: IMPLEMENT ATTACK
            }
            else if (actions[0] == playerAction.block)
            {
                // TODO: IMPLEMENT BLOCK
            }
            */
        }
    }

    public override void FinalTick()
    {
        if (actions.Count > 0)
        {
            actions.RemoveAt(0);
        }
    }

    public Entity GetEnemyAtCell(Cell cell)
    {
        Entity entity = null;

        for (int i = 1; i < GameManager.entities.Count; i++)
        {
            if (GameManager.entities[i].currentCell == cell)
            {
                entity = GameManager.entities[i];
            }
        }

        return entity;
    }

    public override void GetHit(Entity entity, int damage)
    {
        Health -= damage;

        Vector3 vecDir = currentCell.position - entity.currentCell.position;

        vecDir.y = vecDir.z;

        int length = 0;

        Cell targetCell = currentCell;

        Dictionary<Vector2, direction> vec2dir = new Dictionary<Vector2, direction>()
        {
            { Vector2.up, direction.up },
            { Vector2.right, direction.right },
            { Vector2.down, direction.down },
            { Vector2.left, direction.left }
        };

        direction dir = vec2dir[vecDir];

        for (int i = 0; i < 3; i++)
        {
            if (targetCell.neighbors[(int)dir] != null && targetCell.neighbors[(int)dir].pathable)
            {
                targetCell = targetCell.neighbors[(int)dir];
                length++;
            }
            else
            {
                break;
            }
        }

        if (length > 0)
        {
            MoveCell(targetCell);
            anim.StartKnockback(vecDir, length);
        }
    }

    public override void GetHit(Vector2 vecDir, int damage)
    {
        Health -= damage;

        //Vector3 vecDir = currentCell.position - entity.currentCell.position;

        //vecDir.y = vecDir.z;

        int length = 0;

        Cell targetCell = currentCell;

        Dictionary<Vector2, direction> vec2dir = new Dictionary<Vector2, direction>()
        {
            { Vector2.up, direction.up },
            { Vector2.right, direction.right },
            { Vector2.down, direction.down },
            { Vector2.left, direction.left }
        };

        direction dir = vec2dir[vecDir];

        for (int i = 0; i < 3; i++)
        {
            if (targetCell.neighbors[(int)dir] != null && targetCell.neighbors[(int)dir].pathable)
            {
                targetCell = targetCell.neighbors[(int)dir];
                length++;
            }
            else
            {
                break;
            }
        }

        print(length);

        print(currentCell.position);

        if (length > 0)
        {
            MoveCell(targetCell);
            print(targetCell.position);
            anim.StartKnockback(vecDir, length);
        }
    }

    public override void OnHealthChange(int before, int after)
    {
        print("Player health was changed, please implement hearts: " + after);
    }
}
