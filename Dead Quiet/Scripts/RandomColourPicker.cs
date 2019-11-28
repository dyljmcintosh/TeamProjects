using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomColourPicker : MonoBehaviour
{
    public Material[] materialList;
    public Renderer rend;
    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();
        rend.material = materialList[Random.Range(0, materialList.Length)];


    }
}
