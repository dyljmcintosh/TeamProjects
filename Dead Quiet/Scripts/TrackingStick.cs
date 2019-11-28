using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackingStick : MonoBehaviour
{
    public GameObject brokenStick;

    // When the player gets close enough to the stick destory it
    // and replace with glowing broken stick
    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Instantiate(brokenStick, transform.position, Quaternion.identity);

            Destroy(gameObject);
        }
    }
}
