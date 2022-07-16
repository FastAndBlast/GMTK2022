using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public Transform rootTransform;

    public float timeLength = 0.25f;

    // Jump height of 0.2 makes it look like its rolling
    public float jumpHeight = 0.2f;

    public float stretchAmount = 0.3f;

    private bool playing;

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
    }

    public void StartAnimation(Vector2 direction)
    {
        //transform.position += Vector3.one;
        playing = true;
        curTime = timeLength;

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
    }

    public void StopAnimation()
    {
        //playing = false;
    }

    private void Update() {
        Vector2 movementAxis = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (movementAxis != Vector2.zero && !playing)
        {
            StartAnimation(movementAxis);
        }

        if (playing)
        {
            
            

            rootTransform.position = Vector3.Lerp(originalPosition, targetPosition, (timeLength - curTime) / timeLength); //new Vector3(); //Vector3.MoveTowards(rootTransform.position, targetPosition, (1 / timeLength) * Time.deltaTime);

            float timeSinceStart = Mathf.Clamp(timeLength - curTime, 0, timeLength);

            if (timeSinceStart < timeLength / 2)
            {
                rootTransform.localScale = Vector3.Lerp(originalScale, targetScale, (timeSinceStart) / (timeLength / 2));
                //timeLength -> timeLength / 2
            }
            else
            {
                //timeLength / 2 -> 0
                rootTransform.localScale = Vector3.Lerp(targetScale, originalScale, (timeSinceStart - timeLength / 2) / (timeLength / 2));
            }

            float newY = jumpHeight * -(timeSinceStart * (timeSinceStart - timeLength)) / (timeLength * timeLength / 4);

            rootTransform.position += Vector3.up * newY;
            //rootTransform.eulerAngles = Vector3.Lerp(originalRotation, targetRotation, timeLength - curTime);//new Vector3(); //Vector3.MoveTowards(rootTransform.eulerAngles, targetRotation, (90 / timeLength) * Time.deltaTime);
            //rootTransform.rotation *= Quaternion.AngleAxis(90 / timeLength * Time.deltaTime, rotationAxis);
            //rootTransform.Rotate(rotationAxis, 90 / timeLength * Time.deltaTime);
            rootTransform.Rotate(rootTransform.InverseTransformDirection(rotationAxis), 90 / timeLength * Time.deltaTime);

            if (curTime > 0)
            {
                curTime -= Time.deltaTime;
            }
            else
            {
                playing = false;
                rootTransform.rotation = originalRotation;
                rootTransform.Rotate(rootTransform.InverseTransformDirection(rotationAxis), 90);
                //rootTransform.rotation *= Quaternion.AngleAxis(90, rotationAxis);
            }
        }
    }

}
