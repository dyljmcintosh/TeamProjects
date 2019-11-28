using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Killswitch : MonoBehaviour
{
    // This script will force a game restart if the buttons combo is pressed

    void Update()
    {
        if(Input.GetButton("Fire1_All") && Input.GetButton("Fire2_All") && Input.GetButton("Menu_Submit_All") && Input.GetButton("Menu_Cancel_All") && (Input.GetAxisRaw("Menu_Horizontal_All") <= -1))
            SceneManager.LoadScene(0);
    }
}
