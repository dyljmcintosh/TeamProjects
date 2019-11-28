using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapHelper : MonoBehaviour
{
    // I didn't want to mess with your structure.

    void Start()
    {
        GetComponent<MapGenerator>().StartMapGenerator();
    }
}
