using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public Transform rootTransform;

    [Header("Moving")]
    public float moveTime = 0.25f;

    // Jump height of 0.2 makes it look like its rolling
    public float jumpHeight = 0.2f;

    public float stretchAmount = 0.3f;

    [Header("Knockback")]
    public float knockbackTime = 0.5f;

    public int playing;

    private float curTime = 1f;

    private Vector3 targetPosition;
    private Vector3 targetScale;
    //private Vector3 targetRotation;

    private Vector3 rotationAxis;
    
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private Vector3 originalScale;

    private void Start()
    {
        originalScale = rootTransform.localScale;

        moveTime = GameManager.instance.movementTime;
        knockbackTime = GameManager.instance.actionTime;
    }

    public void StartMove(Vector2 direction)
    {
        //transform.position += Vector3.one;
        playing = 1;
        curTime = moveTime;

        originalPosition = rootTransform.position;
        originalRotation = rootTransform.rotation; //rootTransform.eulerAngles;

        Vector3 dir = Vector3.zero;

        if (direction == Vector2.up)
        {
            targetPosition = rootTransform.position + Vector3.forward;
            //targetScale = rootTransform.localScale + rootTransform.InverseTransformDirection(Vector3.forward) * stretchAmount;
            rotationAxis = Vector3.right;

            dir = rootTransform.InverseTransformDirection(Vector3.forward);
            //targetRotation = rootTransform.eulerAngles + Vector3.right * 90;
        }
        else if (direction == Vector2.right)
        {
            targetPosition = rootTransform.position + Vector3.right;
            //targetScale = rootTransform.localScale + rootTransform.InverseTransformDirection(Vector3.right) * stretchAmount;
            rotationAxis = -Vector3.forward;

            dir = rootTransform.InverseTransformDirection(Vector3.right);
            //targetRotation = rootTransform.eulerAngles - Vector3.forward * 90;
        }
        else if (direction == Vector2.down)
        {
            targetPosition = rootTransform.position + Vector3.back;
            
            rotationAxis = -Vector3.right;

            dir = rootTransform.InverseTransformDirection(Vector3.back);

            //targetRotation = rootTransform.eulerAngles - Vector3.right * 90;
        }
        else
        {
            targetPosition = rootTransform.position + Vector3.left;
            //targetScale = rootTransform.localScale + rootTransform.InverseTransformDirection(Vector3.left) * stretchAmount;
            rotationAxis = Vector3.forward;

            dir = rootTransform.InverseTransformDirection(Vector3.left);
            //targetRotation = rootTransform.eulerAngles + Vector3.forward * 90;
        }

        //print("Time: " + Time.time + "\nDir: " + dir);

        if (dir.x + dir.y + dir.z < 0)//Vector3.Distance(Vector3.one, dir) > Vector3.Distance(Vector3.one, Vector3.up))
        {
            targetScale = rootTransform.localScale - dir * stretchAmount;
        }
        else
        {
            targetScale = rootTransform.localScale + dir * stretchAmount;
        }

        PlayerController.instance.rotationAfterAnimation.Rotate(rootTransform.InverseTransformDirection(rotationAxis), 90);
    }

    public void StartKnockback(Vector2 direction, int length)
    {
        playing = 2;
        curTime = knockbackTime;

        originalPosition = rootTransform.position;

        if (direction == Vector2.up)
        {
            targetPosition = rootTransform.position + Vector3.forward * length;
            //targetRotation = rootTransform.eulerAngles + Vector3.right * 90;
        }
        else if (direction == Vector2.right)
        {
            targetPosition = rootTransform.position + Vector3.right * length;
            //targetRotation = rootTransform.eulerAngles - Vector3.forward * 90;
        }
        else if (direction == Vector2.down)
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

    public void StartAttack(Entity target)
    {
        TrackHorizontalPosition.instance.StartAttack();
    }

    public void StartBlock()// Vector2 direction)
    {
        TrackHorizontalPosition.instance.StartBlock();
    }

    private void Update() {
        /*
        Vector2 movementAxis = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (movementAxis != Vector2.zero && !playing)
        {
            StartMove(movementAxis);
        }
        */

        if (playing == 1)
        {
            rootTransform.position = Vector3.Lerp(originalPosition, targetPosition, (moveTime - curTime) / moveTime); //new Vector3(); //Vector3.MoveTowards(rootTransform.position, targetPosition, (1 / moveTime) * Time.deltaTime);

            float timeSinceStart = Mathf.Clamp(moveTime - curTime, 0, moveTime);

            if (timeSinceStart < moveTime / 2)
            {
                rootTransform.localScale = Vector3.Lerp(originalScale, targetScale, (timeSinceStart) / (moveTime / 2));
                //moveTime -> moveTime / 2
            }
            else
            {
                //moveTime / 2 -> 0
                rootTransform.localScale = Vector3.Lerp(targetScale, originalScale, (timeSinceStart - moveTime / 2) / (moveTime / 2));
            }

            float newY = jumpHeight * -(timeSinceStart * (timeSinceStart - moveTime)) / (moveTime * moveTime / 4);

            rootTransform.position += Vector3.up * newY;
            //rootTransform.eulerAngles = Vector3.Lerp(originalRotation, targetRotation, moveTime - curTime);//new Vector3(); //Vector3.MoveTowards(rootTransform.eulerAngles, targetRotation, (90 / moveTime) * Time.deltaTime);
            //rootTransform.rotation *= Quaternion.AngleAxis(90 / moveTime * Time.deltaTime, rotationAxis);
            //rootTransform.Rotate(rotationAxis, 90 / moveTime * Time.deltaTime);
            rootTransform.Rotate(rootTransform.InverseTransformDirection(rotationAxis), 90 / moveTime * Time.deltaTime);

            if (curTime > 0)
            {
                curTime -= Time.deltaTime;
            }
            else
            {
                playing = 0;
                //transform.position = targetPosition;
                //rootTransform.localPosition = Vector3.zero;
                rootTransform.rotation = originalRotation;
                rootTransform.Rotate(rootTransform.InverseTransformDirection(rotationAxis), 90);
                //rootTransform.rotation *= Quaternion.AngleAxis(90, rotationAxis);
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
                rootTransform.position = targetPosition;
                //transform.position = targetPosition;
                //rootTransform.localPosition = Vector3.zero;
                //rootTransform.rotation *= Quaternion.AngleAxis(90, rotationAxis);
            }
        }
    }

}
