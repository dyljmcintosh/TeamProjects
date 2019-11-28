using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuCameraController : MonoBehaviour
{
    public Vector3 mapSize;
    public Camera[] cameras;

    public float cameraDistance = 50;
    public float cameraSpeed = 100;

    // Cross Fade
    public Animator renderTextureAnimator;
    public Camera currentCamera;

    void Start()
    {
        SetPosition(cameras[0].transform);

        currentCamera = cameras[0];
    }

    void Update()
    {
        foreach (Camera c in cameras)
            c.transform.Translate(transform.up * cameraSpeed * Time.deltaTime);

        if ((currentCamera.transform.position.x > 0 && currentCamera.transform.position.x > mapSize.x / 2) || (currentCamera.transform.position.x < 0 && currentCamera.transform.position.x < -mapSize.x / 2) || 
            (currentCamera.transform.position.z > 0 && currentCamera.transform.position.z > mapSize.z / 2) || (currentCamera.transform.position.z < 0 && currentCamera.transform.position.z < -mapSize.z / 2))
        {
            bool fade;

            if (currentCamera == cameras[0])
            {
                currentCamera = cameras[1];

                fade = true;
            }
            else
            {
                currentCamera = cameras[0];

                fade = false;
            }

            SetPosition(currentCamera.transform);
            SetRotation(currentCamera.transform);

            renderTextureAnimator.SetBool("CrossFade", fade);
        }
    }
    
    void SetPosition(Transform cam)
    {
        bool xy = (Random.value > 0.5f) ? true : false;
        float direction = (Random.value > 0.5f) ? 1 : -1;

        if (xy)
            cam.position = new Vector3(mapSize.x / 2 * direction, cameraDistance, Random.Range(-mapSize.z / 2, mapSize.z / 2));
        else
            cam.position = new Vector3(Random.Range(-mapSize.x / 2, mapSize.x / 2), cameraDistance, mapSize.z / 2 * direction);
    }

    void SetRotation(Transform cam)
    {
        Vector3 newRotation = new Vector3(90, cam.eulerAngles.y + Random.Range(-45, 45), 0);

        cam.rotation = Quaternion.Euler(newRotation);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(Vector3.up * transform.position.y, mapSize);
    }
}
