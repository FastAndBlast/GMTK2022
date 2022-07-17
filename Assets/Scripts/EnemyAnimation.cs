using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    private Animator anim;

    [Header("Moving")]

    public Transform rootTransform;

    public float timeLength = 0.25f;

    [Header("Knockback")]
    public float knockbackTime = 0.5f;

    private int playing = 0;

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
        if (playing == 1)
        {
            rootTransform.position = Vector3.Lerp(originalPosition, targetPosition, (timeLength - curTime) / timeLength);

            if (curTime > 0)
            {
                curTime -= Time.deltaTime;
            }
            else
            {
                playing = 0;
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
        playing = 1;
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
