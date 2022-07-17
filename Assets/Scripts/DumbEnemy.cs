using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class DumbEnemy : Entity
{
    public cond attackCondition;
    //private List<Cell> path;

    private float timeToDisable = 0.5f;

    EnemyAnimation anim;

    public void Initialise()
    {
        base.Start();
        anim = GetComponent<EnemyAnimation>();
        currentCell = GetComponentInParent<Room>().GetCell(transform.position);
        currentCell.pathable = false;
        //print(currentCell);
        //gameObject.SetActive(false);
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

    public override void ActionTick()
    {
    }
    public override void FinalTick()
    {
    }
    public override void MovementTick()
    {
    }
}
