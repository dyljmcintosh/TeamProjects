using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuQuit : Menu
{
    public PlayerController player;
    PlayerController.States oldState;

    protected override void Update()
    {
        if (!disableControl)
        {
            // PLAYER 1
            if (player.name == "Player 1")
            {
                // Directional input for selecting buttons
                if (Input.GetAxisRaw("Menu_Vertical_P1") != 0)
                {
                    if (!directionPressed)
                    {
                        if (Input.GetAxisRaw("Menu_Vertical_P1") < 0)
                        {
                            if (index > 0)
                                index--;
                            else
                                index = buttons.Length - 1;
                        }
                        if (Input.GetAxisRaw("Menu_Vertical_P1") > 0)
                        {
                            if (index < buttons.Length - 1)
                                index++;
                            else
                                index = 0;
                        }

                        PlaySound(buttonSelectSound);
                    }

                    directionPressed = true;
                }
                else
                {
                    directionPressed = false;
                }

                // Setting button states
                for (int i = 0; i < buttons.Length; i++)
                {
                    if (i == index)
                        buttons[i].ButtonSelect();
                    else
                        buttons[i].ButtonDeselect();
                }

                // Button input for pressing buttons
                if (Input.GetButtonDown("Menu_Submit_P1"))
                {
                    buttons[index].ButtonPress();

                    PlaySound(buttonPressSound);
                }
            }

            // PLAYER 2
            if (player.name == "Player 2")
            {
                // Directional input for selecting buttons
                if (Input.GetAxisRaw("Menu_Vertical_P2") != 0)
                {
                    if (!directionPressed)
                    {
                        if (Input.GetAxisRaw("Menu_Vertical_P2") < 0)
                        {
                            if (index > 0)
                                index--;
                            else
                                index = buttons.Length - 1;
                        }
                        if (Input.GetAxisRaw("Menu_Vertical_P2") > 0)
                        {
                            if (index < buttons.Length - 1)
                                index++;
                            else
                                index = 0;
                        }

                        PlaySound(buttonSelectSound);
                    }

                    directionPressed = true;
                }
                else
                {
                    directionPressed = false;
                }

                // Setting button states
                for (int i = 0; i < buttons.Length; i++)
                {
                    if (i == index)
                        buttons[i].ButtonSelect();
                    else
                        buttons[i].ButtonDeselect();
                }

                // Button input for pressing buttons
                if (Input.GetButtonDown("Menu_Submit_P2"))
                {
                    buttons[index].ButtonPress();

                    PlaySound(buttonPressSound);
                }
            }
        }
    }

    public override void OpenMenu()
    {
        if (player.state != PlayerController.States.Dead || player.state != PlayerController.States.Paused)
        {
            base.OpenMenu();

            oldState = player.state;
            player.state = PlayerController.States.Paused;
        }
    }

    public override void CloseMenu()
    {
        if (player.state != PlayerController.States.Dead)
        {
            base.CloseMenu();
            
            player.state = oldState;
        }
    }

    public void HardCloseMenu()
    {
        base.CloseMenu();
    }
}
