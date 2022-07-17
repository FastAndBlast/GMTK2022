using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackHorizontalPosition : MonoBehaviour
{
    public Transform target;

    //private Vector3 originalDirection;

    //private Vector3 focusPoint;

    public static TrackHorizontalPosition instance;

    public Animator swordAnim;
    public Animator shieldAnim;

    private Vector3 lastDirection;

    private float slerpTime;

    private void Awake()
    {
        instance = this;
        //anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        transform.position = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z);

        float closestDist = Mathf.Infinity;
        int closestIndex = -1;

        for (int i = 1; i < GameManager.entities.Count; i++)
        {
            float dist = Vector3.Distance(transform.position, GameManager.entities[i].transform.position);//currentCell.position);
            if (dist < closestDist)
            {
                closestDist = dist;
                closestIndex = i;
            }
        }

        if (closestIndex == -1)
        {
            //transform.forward = Vector3.Slerp();

            transform.forward = Vector3.Slerp(lastDirection, Vector3.forward, slerpTime);//Vector3.forward;
            slerpTime += Time.deltaTime;
        }
        else
        {
            transform.forward = GameManager.entities[closestIndex].transform.position -
                new Vector3 (transform.position.x, 0, transform.position.z);
            lastDirection = transform.forward;
            slerpTime = 0;
        }

    }

    public void StartAttack()
    {
        swordAnim.SetBool("swordAttack", true);
        StartCoroutine(ResetSword());
        //anim.SetBool("isBlocking", false);
    }

    public void StartBlock()
    {
        //anim.SetBool("isAttacking", false);
        shieldAnim.SetBool("shieldHit", true);
        StartCoroutine(ResetShield());
    }

    IEnumerator ResetSword()
    {
        float time = 0.1f;
        while (time > 0)
        {
            time -= Time.deltaTime;
            yield return null;
        }
        swordAnim.SetBool("swordAttack", false);
        yield break;
    }

    IEnumerator ResetShield()
    {
        float time = 0.1f;
        while (time > 0)
        {
            time -= Time.deltaTime;
            yield return null;
        }
        shieldAnim.SetBool("shieldHit", false);
        yield break;
    }

}
