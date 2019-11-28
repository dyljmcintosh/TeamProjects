using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : DestructibleObject
{
    Rigidbody rigidbody;    // This error refers to obsolete members that have been removed in the next version of Unity.
    Collider collider;      // This error refers to obsolete members that have been removed in the next version of Unity.
    PlayerInputController playerInput;
    Animator animator;

    public enum States { Paused, Day, Night, Dead, Win };
    public States state = States.Day;     // Make this private after testing.

    public int playerNumber;

    public float moveSpeed = 5;

    // Smooth Movement
    Vector2 velocity;
    Vector2 currentMoveVelocity;
    public float smoothMoveTime = 0.1f;
    //float dampInput;
    //float currentInputDamp;
    public float hurtKnockback = 2;

    //Rotation
    float targetAngle;
    float currentTurnVelocity;
    public float smoothTurnTime = 0.05f;

    // Shooting
    public int maxAmmo = 1;
    [HideInInspector]
    public int currentAmmo = 1;
    [HideInInspector]
    public bool hasWeapon = true;
    bool aiming = false;

    public Transform firePoint;         // Should we use a separate point for throwing weapons?
    public GameObject bulletProjectile;
    public GameObject weaponProjectile; // I don't think this name is descriptive enough. Refers to the thrown gun prefab.

    public GameObject gunObject;        // Another non-descriptive name... Refers to the gun the player model is holding.

    // Campfire
    bool campfireBuilt = false;
    public Transform campfire;
    public float campfireSpawnPosition = 2;

    // Effects
    public GameObject gunshotEffect;
    public GameObject gunMisfireEffect;
    public GameObject gunThrowEffect;
    public GameObject soundwaveEffect;
    public GameObject hurtEffect;
    public GameObject deathEffect;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        collider = GetComponentInChildren<Collider>();  // We really need to discuss the best way to handle colliders.
        playerInput = GetComponent<PlayerInputController>();
        animator = GetComponent<Animator>();
    }

    protected override void Start()
    {
        base.Start();

        currentAmmo = maxAmmo;
    }

    void Update()
    {
        // Day time state. Used for regular gameplay.
        if (state == States.Day)
        {
            animator.SetBool("End", false);
            animator.SetBool("Dead", false);

            transform.position = new Vector3(transform.position.x, 0, transform.position.z);    // THIS IS A QUICK AND DIRTY WAY TO WORK AROUND THE SPAWN POINT POSITION PROBLEMS

            animator.SetBool("Sleeping", false);

            // Collect Movement Input
            Vector2 moveInput = new Vector2(playerInput.GetAxisRaw("Horizontal_LeftStick"), playerInput.GetAxisRaw("Vertical_LeftStick"));

            // Calculate Velocity
            if(!aiming)
                velocity = Vector2.SmoothDamp(velocity, moveInput * moveSpeed, ref currentMoveVelocity, smoothMoveTime);
            else
                velocity = Vector2.SmoothDamp(velocity, Vector2.zero, ref currentMoveVelocity, smoothMoveTime);

            // Collect Aiming Input
            Vector2 aimInput = new Vector2(playerInput.GetAxisRaw("Horizontal_RightStick"), playerInput.GetAxisRaw("Vertical_RightStick"));

            // Caculate Rotation
            if (aimInput.magnitude != 0)
            {
                targetAngle = Mathf.SmoothDampAngle(targetAngle, Mathf.Atan2(aimInput.x, aimInput.y) * Mathf.Rad2Deg, ref currentTurnVelocity, smoothTurnTime);

                aiming = true;
            }
            else if (aimInput.magnitude == 0 && moveInput.magnitude != 0)
            {
                targetAngle = Mathf.SmoothDampAngle(targetAngle, Mathf.Atan2(moveInput.x, moveInput.y) * Mathf.Rad2Deg, ref currentTurnVelocity, smoothTurnTime);

                aiming = false;
            }
            else
            {
                targetAngle = transform.eulerAngles.y;

                aiming = false;
            }

            transform.eulerAngles = Vector3.up * targetAngle;

            if (hasWeapon)
            {
                gunObject.SetActive(true);

                // Fire Weapon
                if (aiming && currentAmmo > 0 && playerInput.GetButtonDown("Fire1"))
                {
                    //Projectile projectileInstance = Instantiate(bulletProjectile, firePoint.position, firePoint.rotation).GetComponent<Projectile>();
                    Projectile projectileInstance = Instantiate(bulletProjectile, firePoint.position, transform.rotation).GetComponent<Projectile>();
                    projectileInstance.Fire(collider);
                    currentAmmo--;

                    Instantiate(gunshotEffect, firePoint.position, firePoint.rotation);

                    animator.SetTrigger("Shoot");
                }

                // Firing without ammo
                if (aiming && currentAmmo <= 0 && playerInput.GetButtonDown("Fire1"))
                {
                    Instantiate(gunMisfireEffect, firePoint.position, firePoint.rotation);
                }

                // Throw Weapon
                if (aiming  && playerInput.GetButtonDown("Fire2"))
                {
                    // Moved throwing logic to its own function to be called by animation event.

                    animator.SetTrigger("Throw");
                }
            }
            else
            {
                gunObject.SetActive(false);
            }

            // Update Animations
            //animator.SetFloat("MoveX", moveInput.x);
            //animator.SetFloat("MoveY", moveInput.y);
            //animator.SetFloat("ForwardVector", moveInput.magnitude);
            //dampInput = Mathf.SmoothDamp(dampInput, moveInput.magnitude, ref currentInputDamp, smoothMoveTime);
            //animator.SetFloat("ForwardVector", dampInput);
            animator.SetFloat("ForwardVector", velocity.magnitude / moveSpeed);
            animator.SetBool("Aiming", aiming);
            animator.SetBool("HasWeapon", hasWeapon);
        }

        // Night time state. Used for Camping.
        if(state == States.Night)
        {
            velocity = Vector2.zero;

            if(!campfireBuilt)
                BuildCampfire();

            animator.SetBool("Sleeping", true);
        }

        // Dead state. Used when player is dead, plays animations, informs GameController of player death, etc.
        if(state == States.Dead)
        {
            velocity = Vector2.SmoothDamp(velocity, Vector2.zero, ref currentMoveVelocity, smoothMoveTime);
            //velocity = Vector2.zero;

            animator.SetBool("Dead", true);
        }

        // Paused State. Stop the player.
        if(state == States.Paused)
        {
            velocity = Vector2.SmoothDamp(velocity, Vector2.zero, ref currentMoveVelocity, smoothMoveTime);
        }

        // Win State. Dance!
        if (state == States.Win)
        {
            velocity = Vector2.SmoothDamp(velocity, Vector2.zero, ref currentMoveVelocity, smoothMoveTime);

            animator.SetBool("End", true);
        }
    }
    
    void FixedUpdate()
    {
        // Move the Rigidbody
        rigidbody.velocity = new Vector3(velocity.x, 0, velocity.y);    // This appears to solve the sliding issues with collisions.
    }

    public void ChangeState(States state)
    {
        // Reset relevant values.
        campfireBuilt = false;

        // Change the state.
        this.state = state;
    }

    public void ThrowWeapon()
    {
        Weapon weaponInstance = Instantiate(weaponProjectile, firePoint.position, transform.rotation).GetComponent<Weapon>();
        weaponInstance.Fire(collider);

        weaponInstance.currentAmmo = currentAmmo;
        currentAmmo = 0;
        hasWeapon = false;

        Instantiate(gunThrowEffect, firePoint.position, firePoint.rotation);
    }

    void BuildCampfire()
    {
        Vector3 startingRotation = Vector3.zero;
        startingRotation.y = Random.Range(0, 360);

        Instantiate(campfire, transform.position + transform.forward * campfireSpawnPosition, Quaternion.Euler(startingRotation));
        
        campfireBuilt = true;
    }

    void Hurt()
    {
        Vector3 knockback = transform.forward * -hurtKnockback;

        //velocity = knockback;
        velocity *= velocity.magnitude * -hurtKnockback;

        //rigidbody.AddForce(knockback, ForceMode.Impulse);

        Instantiate(hurtEffect, transform.position, transform.rotation);

        animator.SetTrigger("Hurt");
    }

    protected override void Die()
    {
        Instantiate(deathEffect, transform.position, transform.rotation);

        state = States.Dead;
    }

    public void TrackSound(Vector3 location)
    {
        Transform t = Instantiate(soundwaveEffect, transform.position, Quaternion.identity).transform;
        t.LookAt(location);

        //t.gameObject.layer = (playerNumber == 1) ? LayerMask.NameToLayer("Camera2Effect") : LayerMask.NameToLayer("Camera1Effect");

        foreach (Transform c in t)
        {
            c.gameObject.layer = (playerNumber == 1) ? LayerMask.NameToLayer("Camera1Effect") : LayerMask.NameToLayer("Camera2Effect");
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Dangerous") && state == States.Day)
        {
            Hurt();
        }
    }
}

[System.Serializable]
public class PlayerData
{
    public PlayerController controller;
    public CameraController camera;
}
