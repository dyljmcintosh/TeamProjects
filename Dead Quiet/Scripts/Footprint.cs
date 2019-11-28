using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Footprint : MonoBehaviour
{
    public float lifetime = 5;
    // Start is called before the first frame update
    void Start()
    {
        if (lifetime > 0)
        {
            Destroy(gameObject, lifetime);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
