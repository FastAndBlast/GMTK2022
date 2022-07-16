using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    private Animator anim;

    public Transform rootTransform;

    public float timeLength = 0.25f;

    private bool moving;

    private float curTime = 1f;

    private Vector3 originalPosition;

    private Vector3 targetPosition;

    private void Start()
    {
        anim = GetComponent<Animator>();
        originalPosition = rootTransform.position;
    }

    private void Update()
    {
        if (moving)
        {
            rootTransform.position = Vector3.Lerp(originalPosition, targetPosition, (timeLength - curTime) / timeLength);

            if (curTime > 0)
            {
                curTime -= Time.deltaTime;
            }
            else
            {
                moving = false;
            }
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
        moving = true;
        curTime = timeLength;

        originalPosition = rootTransform.position;

        targetPosition = newPosition;
        Vector3 dir = (newPosition - targetPosition).normalized;

        rootTransform.forward = dir;

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
