using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneReloader : MonoBehaviour
{
    protected Animator fadeOutAnimator;   // MUST BE ON THE CANVAS!

    public float restartTime = 48;

    void Awake()
    {
        fadeOutAnimator = transform.root.GetComponent<Animator>();
    }

    void Start()
    {
        Invoke("RestartScene", restartTime);
    }

    void RestartScene()
    {
        StartCoroutine(FadeToNewScene(0));

        fadeOutAnimator.SetTrigger("FadeOut");
    }

    protected IEnumerator FadeToNewScene(int index)
    {
        fadeOutAnimator.SetTrigger("FadeOut");

        yield return null;  // Skip the rest until the next frame, so the Animator has updated.

        yield return new WaitForSeconds(fadeOutAnimator.GetCurrentAnimatorStateInfo(0).length);

        SceneManager.LoadScene(index);
    }
}
