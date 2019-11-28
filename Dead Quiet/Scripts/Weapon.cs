using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Projectile
{
    public int currentAmmo = 1;

    public GameObject pickupEffect;

    protected override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);

        PlayerController player = collision.transform.GetComponent<PlayerController>();

        if (player)
        {
            if (!player.hasWeapon)
            {
                player.hasWeapon = true;
                player.currentAmmo = currentAmmo;

                Instantiate(pickupEffect, transform.position, transform.rotation);

                Destroy(gameObject);
            }
        }
        else
        {
            Instantiate(impactEffect, transform.position, transform.rotation);
        }
    }
}
