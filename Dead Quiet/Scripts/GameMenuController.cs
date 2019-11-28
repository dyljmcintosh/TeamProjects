using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMenuController : MonoBehaviour
{
    public GameObject gameMenu;
    public Menu winMenu;

    public GameObject[] winBanners;

    public MenuQuit player1Menu;
    public MenuQuit player2Menu;

    AudioSource audioSource;
    public AudioClip winMusic;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if(Input.GetButtonDown("Menu_Open_P1"))
        {
            player1Menu.Invoke("OpenMenu", 0.01f);
            player1Menu.PlaySound(player1Menu.buttonPressSound);
        }
        if (Input.GetButtonDown("Menu_Open_P2"))
        {
            player2Menu.Invoke("OpenMenu", 0.01f);
            player2Menu.PlaySound(player2Menu.buttonPressSound);
        }
    }

    public void ShowWinMenu(int winner)
    {
        gameMenu.SetActive(false);
        player1Menu.HardCloseMenu();
        player2Menu.HardCloseMenu();
        winMenu.OpenMenu();

        audioSource.clip = winMusic;
        audioSource.loop = false;
        audioSource.Play();

        for (int i = 0; i < winBanners.Length; i++)
        {
            if (winner == i)
                winBanners[i].SetActive(true);
            else
                winBanners[i].SetActive(false);
        }
    }
}
