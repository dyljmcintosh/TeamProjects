using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Footsteps : MonoBehaviour
{
    public GameObject footprintEffect;

    public float rayLength = 0.1f;

    bool footprintCast = false;

    public LayerMask layerMask;

    PlayerController playerController;

    private void Awake()
    {
        playerController = transform.root.GetComponent<PlayerController>();
    }

    void Update()
    {
        if (playerController.state == PlayerController.States.Day)
        {
            RaycastHit hit;

            //Debug.DrawRay(transform.position, -transform.up * rayLength);

            if (Physics.Raycast(transform.position, -transform.up, out hit, rayLength, layerMask))
            {
                if (!footprintCast)
                {
                    Vector3 position = transform.position;
                    position.y = 0;

                    Vector3 eulers = transform.eulerAngles;
                    eulers.x = 0;
                    eulers.z = 0;

                    Instantiate(footprintEffect, position, Quaternion.Euler(eulers));

                    footprintCast = true;
                }
            }
            else
            {
                footprintCast = false;
            }
        }
    }

    //void OnCollisionEnter(Collision collision)
    //{
    //    Instantiate(footprintEffect, transform.position, transform.rotation);
    //}

    /*
    public GameObject footprintEffectLeft;
    public GameObject footprintEffectRight;

    public Transform footLeft;
    public Transform footRight;

    void SpawnFootstepEffect(string foot)
    {
        if (foot == "left")
            Instantiate(footprintEffectLeft, footLeft.position, footLeft.rotation);
        if (foot == "right")
            Instantiate(footprintEffectRight, footRight.position, footRight.rotation);
    }
    */
}
