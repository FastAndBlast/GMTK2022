using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    private Animator anim;

    [Header("Moving")]

    public Transform rootTransform;
    public Transform rotatationRootTransform;

    public float moveTime = 0.25f;

    [Header("Knockback")]
    public float knockbackTime = 0.5f;

    private int playing = 0;

    private float curTime = 1f;

    private Vector3 originalPosition;

    private Vector3 targetPosition;

    private EnemyAI script;

    private float lookTimer = 0f;

    private Vector3 originalDirection;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        originalPosition = rootTransform.position;

        moveTime = GameManager.instance.movementTime;
        knockbackTime = GameManager.instance.actionTime;
        script = GetComponent<EnemyAI>();
    }

    private void Update()
    {
        if (script)
        {
            if ((GameManager.currentState == state.movement && script.nextAction == enemyAction.attack))
            {
                lookTimer = 1.15f;
                originalDirection = rotatationRootTransform.forward;
            }
            lookTimer -= Time.deltaTime;
            if (lookTimer <= 0)
            {
                Vector3 targetDirection =
                new Vector3(PlayerController.instance.transform.position.x, 0, PlayerController.instance.transform.position.z) -
                new Vector3(rootTransform.position.x, 0, rootTransform.position.z);

                rotatationRootTransform.forward = Vector3.Slerp(originalDirection, targetDirection, -lookTimer * 4);
            }
        }

        if (playing == 1)
        {
            rootTransform.position = Vector3.Lerp(originalPosition, targetPosition, (moveTime - curTime) / moveTime);

            if (curTime > 0)
            {
                curTime -= Time.deltaTime;
            }
            else
            {
                rootTransform.position = targetPosition;
                playing = 0;
                StartIdle();
            }
        }
        else if (playing == 2)
        {
            float timeSinceStart = knockbackTime - curTime;
            rootTransform.position = Vector3.Lerp(originalPosition, targetPosition, (1 - Mathf.Pow(1 - (timeSinceStart / knockbackTime), 2)));

            if (curTime > 0)
            {
                curTime -= Time.deltaTime;
            }
            else
            {
                playing = 0;
                rootTransform.transform.position = targetPosition;
                //rootTransform.transform.position = 
                StartIdle();
            }
        }
    }

    public void ResetAnim()
    {
        anim.SetBool("isAttacking", false);
        anim.SetBool("isWalking", false);
        anim.SetBool("isIdle", false);
        anim.SetBool("isHit", false);
        anim.SetBool("isDead", false);
    }

    public void Face(Vector2 vecDir)
    {
        if (vecDir == Vector2.up)
        {
            rootTransform.forward = Vector3.forward;
        }
        else if (vecDir == Vector2.right)
        {
            rootTransform.forward = Vector3.right;
        }
        else if (vecDir == Vector2.down)
        {
            rootTransform.forward = Vector3.back;
        }
        else
        {
            rootTransform.forward = Vector3.left;
        }
    }

    public void StartAttack()
    {
        anim.SetBool("isAttacking", true);
        anim.SetBool("isWalking", false);
        anim.SetBool("isIdle", false);
        anim.SetBool("isHit", false);
    }

    public void StartMove(Vector3 newPosition)
    {
        playing = 1;
        curTime = moveTime;

        originalPosition = rootTransform.position;

        targetPosition = newPosition;
        targetPosition.y = originalPosition.y;
        Vector3 dir = (newPosition - targetPosition).normalized;

        //rootTransform.forward = dir;

        /*
        if (direction == Vector2.up)
        {
            targetPosition = rootTransform.position + Vector3.forward;

        }
        else if (direction == Vector2.right)
        {
            targetPosition = rootTransform.position + Vector3.right;
        }
        else if (direction == Vector2.down)
        {
            targetPosition = rootTransform.position + Vector3.back;
        }
        else
        {
            targetPosition = rootTransform.position + Vector3.left;
        }
        */

        anim.SetBool("isAttacking", false);
        anim.SetBool("isWalking", true);
        anim.SetBool("isIdle", false);
        anim.SetBool("isHit", false);
    }

    public void StartKnockback(Vector2 dir, int length)
    {
        playing = 2;
        curTime = knockbackTime;

        originalPosition = rootTransform.position;

        if (dir == Vector2.up)
        {
            targetPosition = rootTransform.position + Vector3.forward * length;
            //targetRotation = rootTransform.eulerAngles + Vector3.right * 90;
        }
        else if (dir == Vector2.right)
        {
            targetPosition = rootTransform.position + Vector3.right * length;
            //targetRotation = rootTransform.eulerAngles - Vector3.forward * 90;
        }
        else if (dir == Vector2.down)
        {
            targetPosition = rootTransform.position + Vector3.back * length;

            //targetRotation = rootTransform.eulerAngles - Vector3.right * 90;
        }
        else
        {
            targetPosition = rootTransform.position + Vector3.left * length;
            //targetRotation = rootTransform.eulerAngles + Vector3.forward * 90;
        }

        targetPosition.y = originalPosition.y;
    }

    public void StartIdle()
    {
        anim.SetBool("isAttacking", false);
        anim.SetBool("isWalking", false);
        anim.SetBool("isIdle", true);
        anim.SetBool("isHit", false);
    }

    public void StartHit()
    {
        anim.SetBool("isAttacking", false);
        anim.SetBool("isWalking", false);
        anim.SetBool("isIdle", false);
        anim.SetBool("isHit", true);
    }

    public void StartDie()
    {
        anim.SetBool("isAttacking", false);
        anim.SetBool("isWalking", false);
        anim.SetBool("isIdle", false);
        anim.SetBool("isHit", false);
        anim.SetBool("isDead", true);
    }

}
