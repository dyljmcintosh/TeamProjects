using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuWin : Menu
{
    public MenuButtonTimed restartButton;

    public override void OpenMenu()
    {
        base.OpenMenu();

        restartButton.StartCoroutine("Countdown");
    }

    public override void CloseMenu()
    {
        restartButton.StopCoroutine("Countdown");

        base.CloseMenu();
    }
}
