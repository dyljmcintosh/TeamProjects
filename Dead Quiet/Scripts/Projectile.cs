using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    public float projectileForce;
    public float projectileTorque;

    public int damage = 1;
    bool doesDamage = true;

    public float lifetime = 5;

    public bool destroyOnHit = true;

    public Effect impactEffect;

    public void Fire(Collider parent)
    {
        //Physics.IgnoreCollision(GetComponentInChildren<Collider>(), parent);  // We really need to discuss the best way to handle colliders.

        Rigidbody rigidbody = GetComponent<Rigidbody>();
        rigidbody.AddForce(transform.forward * projectileForce, ForceMode.Impulse);
        rigidbody.AddTorque(transform.right * projectileTorque, ForceMode.Impulse);

        if(lifetime > 0)
            Destroy(gameObject, lifetime);
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        if (doesDamage)
        {
            DestructibleObject destructibleObject = collision.transform.GetComponent<DestructibleObject>();

            if (destructibleObject)
            {
                destructibleObject.Hit(damage);
            }
        }

        if (destroyOnHit)
            Die();

        doesDamage = false; // Once the projectile has hit something (ideally the ground) it will no longer be lethal, allowing subclasses (the weapon) to be picked up.
    }

    protected virtual void Die()
    {
        Instantiate(impactEffect, transform.position, transform.rotation);

        Destroy(gameObject);
    }
}
