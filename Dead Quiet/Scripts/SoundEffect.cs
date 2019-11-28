using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffect : MonoBehaviour
{
    public LayerMask layerMask;

    public float soundRange = 5;

    AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, soundRange, layerMask);

        if (colliders.Length == 0)
        {
            audioSource.Stop();
        }
        else
        {
            foreach (Collider c in colliders)
            {
                if (Vector3.Distance(transform.position, c.transform.position) > 3)
                {
                    c.transform.GetComponent<PlayerController>().TrackSound(transform.position);
                }
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, soundRange);
    }
}
