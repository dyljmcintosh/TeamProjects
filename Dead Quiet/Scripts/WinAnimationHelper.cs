using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinAnimationHelper : MonoBehaviour
{
    public Footsteps[] feet;

    public void WinAnimationFootsteps()
    {
        foreach(Footsteps f in feet)
        {
            Vector3 position = f.transform.position;
            position.y = 0;

            Vector3 eulers = f.transform.eulerAngles;
            eulers.x = 0;
            eulers.z = 0;

            Instantiate(f.footprintEffect, position, Quaternion.Euler(eulers));
        }
    }
}
