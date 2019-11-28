using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputController : MonoBehaviour
{
    public enum Controllers { All, Controller1, Controller2 };
    public Controllers controller;

    string AppendName(string name)
    {
        string appendedName = name;

        switch (controller)
        {
            case Controllers.All:
                appendedName += "_All";
                break;
            case Controllers.Controller1:
                appendedName += "_P1";
                break;
            case Controllers.Controller2:
                appendedName += "_P2";
                break;
        }

        return appendedName;
    }

    public float GetAxisRaw(string name)
    {
        return Input.GetAxisRaw(AppendName(name));
    }

    public bool GetButtonDown(string name)
    {
        return Input.GetButtonDown(AppendName(name));
    }
}
