using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource), typeof(Animator))]
public class Menu : MonoBehaviour
{
    public Menu parentMenu;
    public Menu[] childMenus;

    AudioSource audioSource;
    [HideInInspector]
    public Animator animator;   // Can't initialise if readonly, is this the best workaround?

    public MenuButton[] buttons;
    public int startingIndex = 0;
    protected int index;
    protected bool directionPressed = false;
    
    //[HideInInspector]
    public bool disableControl = false;

    // TO DO: CLOSE SUB-MENUS WITH CANCEL BUTTON. This may require reworking menus to allow for a hierarchy.

    public AudioClip buttonSelectSound;
    public AudioClip buttonPressSound;

    protected virtual void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
    }

    protected virtual void Start()
    {
        if (childMenus.Length > 0)
            SetParentsInChildren();

        index = Mathf.Clamp(startingIndex, 0, buttons.Length - 1);
    }

    protected virtual void Update()
    {
        if (!disableControl)
        {
            // Directional input for selecting buttons
            if (Input.GetAxisRaw("Menu_Horizontal_All") != 0)
            {
                if (!directionPressed)
                {
                    if (Input.GetAxisRaw("Menu_Horizontal_All") < 0)
                    {
                        if (index > 0)
                            index--;
                        else
                            index = buttons.Length - 1;
                    }
                    if (Input.GetAxisRaw("Menu_Horizontal_All") > 0)
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
            if (Input.GetButtonDown("Menu_Submit_All"))
            {
                buttons[index].ButtonPress();
                
                PlaySound(buttonPressSound);
            }
        }
    }

    public virtual void OpenMenu()
    {
        disableControl = false;

        if (animator)
            animator.SetBool("Open", true);

        index = Mathf.Clamp(startingIndex, 0, buttons.Length - 1);
    }

    public virtual void CloseMenu()
    {
            disableControl = true;

            if (animator)
                animator.SetBool("Open", false);
    }

    public void PlaySound(AudioClip sound)
    {
        audioSource.clip = sound;
        audioSource.Play();
    }

    protected void SetParentsInChildren()
    {
        for (int i = 0; i < childMenus.Length; i++)
        {
            childMenus[i].parentMenu = this;
        }
    }
}
