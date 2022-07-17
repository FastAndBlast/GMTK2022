using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using TMPro;

public enum enemyAction { move, windup, attack }

public enum cond { none, one, two, three, four, five, six, even, odd }

public class EnemyAI : Entity
{
    [Header("Turns for enemy to finish attack")]
    public float windupLength = 2f;

    public int damage;

    public Transform attackIndicators;

    public cond attackCondition;

    public enemyAction nextAction = enemyAction.move;

    //[HideInInspector]
    private float windupLeft = 3;

    //private List<Cell> path;

    private float timeToDisable = 0.5f;

    EnemyAnimation anim;

    public void Initialise()
    {
        anim = GetComponent<EnemyAnimation>();
        currentCell = GetComponentInParent<Room>().GetCell(transform.position);
        currentCell.pathable = false;
        //print(currentCell);
        //gameObject.SetActive(false);
    }

    public override void Start()
    {
        // blank
        if ((int)attackCondition > 0 && (int)attackCondition < 7)
        {
            GetComponentInChildren<TextMeshPro>().text = ((int)attackCondition).ToString();
            print(GetComponentInChildren<TextMeshPro>().text);
        }
    }

    public void ReAwake()
    {
        base.Start();
        nextAction = enemyAction.move;
    }

    public override void MovementTick()
    {
        if (nextAction == enemyAction.move)
        {
            //print(Health);

            if (Health < 1)
            {
                return;
            }

            List<Cell> path = currentCell.FindPath(PlayerController.instance.currentCell);

            //print(currentCell);
            //print(currentCell.position);

            if (path == null)
            {
                return;
            }

            if (path[1].pathable)
            {
                MoveCell(path[1]);

                anim.StartMove(currentCell.position + new Vector3(0.5f, 0, 0.5f));
            }
            //print(currentCell.position);
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
            //List<Cell> adjacent = new List<Cell>();

            //adjacent.AddRange(currentCell.neighbors);

            //adjacent.Add(currentCell.neighbors[1].neighbors[0]);
            //adjacent.Add(currentCell.neighbors[1].neighbors[2]);
            //adjacent.Add(currentCell.neighbors[3].neighbors[0]);
            //adjacent.Add(currentCell.neighbors[3].neighbors[2]);

            //Cell[] big;

            Cell adjacentCell = null;

            foreach (Cell cell in currentCell.neighbors)
            {
                if (cell == PlayerController.instance.currentCell)
                {
                    adjacentCell = cell;
                    break;
                }
            }

            anim.StartAttack();
            RuntimeManager.PlayOneShot("event:/Ghouls/Ghoul_Attack");

            if (adjacentCell != null)
            {
                //PlayerController.instance.Health -= 1;
                PlayerController.instance.GetHit(this, damage);
            }

            nextAction = enemyAction.move;
        }

        if (Vector3.Distance(currentCell.position, PlayerController.instance.currentCell.position) <= 1)
        {
            EnableAttackIndicators();
        }
    }

    public override void FinalTick()
    {
        DisableAttackIndicators();
        if (Health < 1)
        {
            //GameManager.entities.Remove(this);
            // GameManager.flush.Add(this);
            // gameObject.SetActive(false);
            return;
        }

        if (nextAction == enemyAction.windup)
        {
            windupLeft -= 1;
            if (windupLeft < 1)
            {
                nextAction = enemyAction.attack;
                EnableAttackIndicators();
            }
        }
        else
        {
            //anim.Face(Vector2.up);
            if (Vector3.Distance(currentCell.position, PlayerController.instance.currentCell.position) <= 1)
            {
                nextAction = enemyAction.attack;
                EnableAttackIndicators();
            }
        }

        anim.StartIdle();

        //Display whatever action is gonna happen
    }

    public void DisableAttackIndicators()
    {
        foreach (Transform child in attackIndicators)
        {
            child.gameObject.SetActive(false);
        }
    }

    public void EnableAttackIndicators()
    {
        if (currentCell.neighbors[(int)direction.up] != null)
        {
            attackIndicators.Find("Up").gameObject.SetActive(true);
        }
        else
        {
            attackIndicators.Find("Up").gameObject.SetActive(false);
        }
        if (currentCell.neighbors[(int)direction.right] != null)
        {
            attackIndicators.Find("Right").gameObject.SetActive(true);
        }
        else
        {
            attackIndicators.Find("Right").gameObject.SetActive(false);
        }
        if (currentCell.neighbors[(int)direction.down] != null)
        {
            attackIndicators.Find("Down").gameObject.SetActive(true);
        }
        else
        {
            attackIndicators.Find("Down").gameObject.SetActive(false);
        }
        if (currentCell.neighbors[(int)direction.left] != null)
        {
            attackIndicators.Find("Left").gameObject.SetActive(true);
        }
        else
        {
            attackIndicators.Find("Left").gameObject.SetActive(false);
        }
    }

    public bool CheckCondition()
    {
        if ((int)attackCondition > 0 && (int)attackCondition < 7)
        {
            if (PlayerController.instance.Number() == (int)attackCondition)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else if ((int)attackCondition == 7)
        {
            if (PlayerController.instance.Number() % 2 == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else if ((int)attackCondition == 8)
        {
            if (PlayerController.instance.Number() % 2 == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return true;
        }
    }

    public override void GetHit(Entity entity, int damage)
    {
        if (entity.gameObject == PlayerController.instance.gameObject)
        {
            if (!CheckCondition())
            {
                return;
            }
        }

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

        nextAction = enemyAction.move;
    }

    public override void GetHit(Vector2 vecDir, int damage)
    {
        

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

        if (length > 0)
        {
            MoveCell(targetCell);
            anim.StartKnockback(vecDir, length);
        }

        DisableAttackIndicators();

        nextAction = enemyAction.move;

        Health -= damage;
    }

    IEnumerator Disable()
    {
        timeToDisable = 1f;
        while (timeToDisable > 0)
        {
            timeToDisable -= Time.deltaTime;

            yield return null;
        }

        anim.ResetAnim();
        gameObject.SetActive(false);
        yield break;
    }

    public override void OnHealthChange(int before, int after)
    {
        print("Enemy damaged: " + after);


        anim.StartHit();

        if (after < 1)
        {
            // DEAD
            anim.StartDie();
            RuntimeManager.PlayOneShot("event:/Ghouls/Ghoul_Die");
            //GameManager.entities.Remove(this);
            GameManager.flush.Add(this);
            currentCell.pathable = true;
            StartCoroutine(Disable());
        }
    }
}
