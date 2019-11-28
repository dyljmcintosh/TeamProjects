using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleObject : MonoBehaviour
{
    public int maxHealth = 1;
    protected int currentHealth;

    protected virtual void Start()
    {
        currentHealth = maxHealth;
    }

    public virtual void Hit(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        Destroy(gameObject);
    }
}
