using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Animator))]
public class MenuButton : MonoBehaviour
{
    public Animator animator;
    public Menu controller;

    protected Animator fadeOutAnimator;   // MUST BE ON THE CANVAS!

    // Inspector Methods Setting
    public enum ButtonMethods { ChangeScene, RestartScene, QuitGame, OpenMenu, CloseMenu };
    public ButtonMethods buttonMethods;

    public int buttonMethodIndexParameter;
    public Menu buttonMethodMenuParameter;

    // Component initialisation

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
        controller = GetComponentInParent<Menu>();
        fadeOutAnimator = transform.root.GetComponent<Animator>();
    }

    // Button states

    public void ButtonSelect()
    {
        animator.SetBool("Selected", true);
    }

    public void ButtonDeselect()
    {
        animator.SetBool("Selected", false);
        animator.SetBool("Pressed", false);
    }

    public void ButtonPress()
    {
        animator.SetBool("Pressed", true);

        switch (buttonMethods)
        {
            case ButtonMethods.ChangeScene:
                ChangeScene(buttonMethodIndexParameter);
                break;

            case ButtonMethods.RestartScene:
                RestartScene();
                break;

            case ButtonMethods.QuitGame:
                QuitGame();
                break;

            case ButtonMethods.OpenMenu:
                OpenMenu(buttonMethodMenuParameter);
                break;

            case ButtonMethods.CloseMenu:
                CloseMenu(buttonMethodMenuParameter);
                break;
        }
    }

    // Button activation functions

    public void ChangeScene(int index)
    {
        controller.disableControl = true;

        StartCoroutine(FadeToNewScene(index));

        fadeOutAnimator.SetTrigger("FadeOut");
    }

    protected IEnumerator FadeToNewScene(int index)
    {
        fadeOutAnimator.SetTrigger("FadeOut");

        yield return null;  // Skip the rest until the next frame, so the Animator has updated.

        yield return new WaitForSeconds(fadeOutAnimator.GetCurrentAnimatorStateInfo(0).length);

        SceneManager.LoadScene(index);
    }

    public void RestartScene()
    {
        ChangeScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        controller.disableControl = true;

        StartCoroutine(FadeToQuit());
    }

    protected IEnumerator FadeToQuit()
    {
        fadeOutAnimator.SetTrigger("FadeOut");

        yield return null;  // Skip the rest until the next frame, so the Animator has updated.

        yield return new WaitForSeconds(fadeOutAnimator.GetCurrentAnimatorStateInfo(0).length);

        Application.Quit();
    }

    public virtual void OpenMenu(Menu menu)
    {
        controller.CloseMenu();

        Invoke("ButtonDeselect", animator.GetCurrentAnimatorStateInfo(0).length);   // Deselects the button once its animation is complete so it doesn't lock the animation when returning to its menu.

        menu.gameObject.SetActive(true);

        menu.Invoke("OpenMenu", 0.01f); // This is to add a small delay so as to not to reopen a menu as soon as it's closed.
    }

    public virtual void CloseMenu(Menu menu)
    {
        menu.CloseMenu();
    }
}
