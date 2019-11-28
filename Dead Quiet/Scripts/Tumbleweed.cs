using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(SphereCollider))]
public class Tumbleweed : MonoBehaviour
{
    Rigidbody rigidbody;

    public float velocityMin = 1;
    public float velocityMax = 2;
    float velocity;
    Vector3 oldVelocity;

    public GameObject bounceEffect;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    void Start()
    {
        velocity = Random.Range(velocityMin, velocityMax);
        
        rigidbody.AddForce(transform.forward * velocity, ForceMode.Impulse);
    }

    void Update()
    {
        transform.rotation = Quaternion.LookRotation(rigidbody.velocity, Vector3.up);

        oldVelocity = rigidbody.velocity;
    }

    void LateUpdate()
    {
        // Enforce constsant velocity
        if (rigidbody.velocity.magnitude != velocity)
            rigidbody.velocity = transform.forward * velocity;
    }

    public void Bounce()
    {
        Instantiate(bounceEffect, new Vector3(transform.position.x, 0, transform.position.z), transform.rotation);
    }

    void OnCollisionEnter(Collision collision)
    {
        Vector3 direction = Vector3.Reflect(oldVelocity, collision.contacts[0].normal);

        rigidbody.velocity = direction * velocity;
    }
}
