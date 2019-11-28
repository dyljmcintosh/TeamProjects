using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuButtonTimed : MenuButton
{
    public int timer = 5;
    protected int currentTime;

    public Text timerText;

    protected virtual void Start()
    {
        currentTime = timer;
    }

    protected virtual void Update()
    {
        if (currentTime < 0)
        {
            ChangeScene(0); // Should be updated to assign a function similar to base class.
        }
    }

    protected IEnumerator Countdown()
    {
        currentTime = timer;

        while (timer > 0)
        {
            timerText.text = currentTime.ToString();
            currentTime--;

            yield return new WaitForSeconds(1);
        }
    }
}
