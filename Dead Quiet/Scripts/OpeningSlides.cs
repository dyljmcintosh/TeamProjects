using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpeningSlides : MonoBehaviour
{
    public GameObject slidesObject;

    public Menu mainMenu;
    Animator animator;

    bool disabled = false;

    public AudioSource musicSource;
    public AudioSource sfxSource;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        slidesObject.SetActive(true);
        mainMenu.disableControl = true;
    }

    void Update()
    {
        if (!disabled)
        {
            if (Input.anyKey)
            {
                animator.SetTrigger("Skip");

                EnableMainMenu();
            }
        }
    }

    public void StartMusic()
    {
        musicSource.Play();
    }

    public void EnableMainMenu()
    {
        mainMenu.Invoke("OpenMenu", 0.01f);
        disabled = true;
    }

    public void PlaySound()
    {
        sfxSource.Play();
    }
}
