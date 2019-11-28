using System.Collections;
using System.Collections.Generic;
using UnityEngine;

////////////////////////////////////////////////////////////
//                                                        //
// THIS CLASS SHOULD BE EXTENDED TO INCLUDE A "GRAVEYARD" //
//       SYSTEM TO AVOID UNNECESSARY INSTANTIATIONS       //
//                                                        //
////////////////////////////////////////////////////////////

public class Effect : MonoBehaviour
{
    List<ParticleSystem> particles = new List<ParticleSystem>();

    AudioSource audioSource;
    public AudioClip[] clips;

    public LayerMask layerMask;
    public float soundRange = 5;

    public Transform followTarget;

    void Awake()
    {
        particles.AddRange(transform.GetComponents<ParticleSystem>());
        particles.AddRange(transform.GetComponentsInChildren<ParticleSystem>());

        audioSource = GetComponent<AudioSource>();

        if (audioSource)
            audioSource.playOnAwake = false;
    }

    void Start()
    {
        if (audioSource)
        {
            audioSource.clip = clips[Random.Range(0, clips.Length)];

            // Check if any valid objects are in range, and if so, play the sound and do tracking effects.
            Collider[] colliders = Physics.OverlapSphere(transform.position, soundRange, layerMask);

            if(colliders.Length > 0)
                audioSource.Play();

            foreach (Collider c in colliders)
            {
                if (Vector3.Distance(transform.position, c.transform.position) > 3)
                {
                    c.transform.GetComponent<PlayerController>().TrackSound(transform.position);
                }
            }
        }

        // Check if Particle Systems are looping and throw a warning if any are.
        foreach (ParticleSystem p in particles)
            if(p.main.loop)
                Debug.LogWarning("The Effect script is not designed to be used with looping Particle Systems.", p);

        float lifetime = GetLifetime();
        
        if (lifetime > 0)
        {
            Destroy(gameObject, lifetime);
        }
        else
        {
            Debug.LogWarning(name + " has no Particle System or Audio Source attached. Are you sure this class is needed for this object?", gameObject);
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (followTarget != null)
            transform.position = followTarget.position;
    }

    // Checks all audio sources and particle systems and calcultes the longest one and returns that value.
    float GetLifetime()
    {
        float result = 0;

        foreach (ParticleSystem p in particles)
            if (p.main.duration > result)
                result = p.main.duration;

        if (audioSource)
            if (audioSource.clip.length > result)
                result = audioSource.clip.length;

        return result;
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, soundRange);
    }
}
