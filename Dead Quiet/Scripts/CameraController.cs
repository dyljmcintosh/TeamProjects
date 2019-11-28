using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Transform cameraObject;
    public Transform target;
    Vector3 targetPosition;
    Camera cameraScript;

    public enum States { Normal, Win };
    public States state = States.Normal;

    public float cameraDistance = 15;
    public float cameraAngle = 60;
    public float cameraHeight = 0;

    public float smoothTime = 0.5f;
    Vector3 currentMoveVelocity;
    float currentZoomVelocity;
    float currentRotationVelocity;
    float currentHeightVelocity;

    public float winDistance = 10;
    public float winAngle = 30;
    public float winHeight = 1.2f;
    public float winOrbit = 0.5f;
    public float rectSmoothTime = 0.5f;
    Vector2 currentRectPositionVelocity;
    float currectRectHeightVelocity;

    //public GameObject screenSeparator;

    void Awake()
    {
        cameraObject = transform.GetChild(0);
        cameraScript = cameraObject.GetComponent<Camera>();
    }

    void FixedUpdate()  // Camera movement would typically be done via LateUpdate() however this create a jerky effect when combined with rigidbodies which need to use FixedUpdate()
    {
        if (state == States.Normal)
        {
            // Camera Rig Position
            if (target)
                targetPosition = target.position;   // This should stop the game from crashing when a player dies.

            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref currentMoveVelocity, smoothTime);

            // Camera Distance
            float targetDistance = Mathf.SmoothDamp(cameraObject.localPosition.z, -cameraDistance, ref currentZoomVelocity, smoothTime);
            float targetHeight = Mathf.SmoothDamp(cameraObject.localPosition.y, cameraHeight, ref currentHeightVelocity, smoothTime);
            cameraObject.localPosition = new Vector3(cameraObject.localPosition.x, targetHeight, targetDistance);

            // Camera Angle
            float targetAngle = Mathf.SmoothDampAngle(transform.eulerAngles.x, cameraAngle, ref currentRotationVelocity, smoothTime);
            transform.rotation = Quaternion.Euler(targetAngle, transform.eulerAngles.y, transform.eulerAngles.z);
        }
        if(state == States.Win)
        {
            // Camera Rig Position
            if (target)
                targetPosition = target.position;

            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref currentMoveVelocity, smoothTime);

            // Camera Distance
            float targetDistance = Mathf.SmoothDamp(cameraObject.localPosition.z, -winDistance, ref currentZoomVelocity, smoothTime);
            float targetHeight = Mathf.SmoothDamp(cameraObject.localPosition.y, winHeight, ref currentHeightVelocity, smoothTime);
            cameraObject.localPosition = new Vector3(cameraObject.localPosition.x, targetHeight, targetDistance);

            // Camera Angle
            float targetAngle = Mathf.SmoothDampAngle(transform.eulerAngles.x, winAngle, ref currentRotationVelocity, smoothTime);
            transform.rotation = Quaternion.Euler(targetAngle, transform.eulerAngles.y - winOrbit, transform.eulerAngles.z);

            // Camera Rect
            cameraScript.depth = 0;
            Rect targetRect = cameraScript.rect;
            targetRect.position = Vector2.SmoothDamp(cameraScript.rect.position, Vector2.zero, ref currentRectPositionVelocity, rectSmoothTime);
            targetRect.height = Mathf.SmoothDamp(cameraScript.rect.height, 1, ref currectRectHeightVelocity, rectSmoothTime);
            cameraScript.rect = targetRect;

            //screenSeparator.SetActive(false);
        }
    }
}
