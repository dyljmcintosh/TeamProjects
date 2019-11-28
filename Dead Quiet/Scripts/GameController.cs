using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public enum States { Start, Play, End };
    public States gameState = States.Start;

    DayNightCycle gameTime;
    
    public PlayerData[] players = new PlayerData[2];

    int winningPlayer = 0;  // 0 = No winner (draw?), 1 = Player 1 wins, 2 = Player 2 wins.

    GameMenuController gameMenu;
    PlayerBuilder playerBuilder;
    EnvironmentBuilder environmentBuilder;
    MapGenerator mapGenerator;

    bool winMenuOpened = false; // Quick and dirty solution.

    void Awake()
    {
        gameTime = GameObject.Find("Sun and Moon").GetComponent<DayNightCycle>();
        gameMenu = GameObject.Find("Canvas").GetComponent<GameMenuController>();
        mapGenerator = GetComponent<MapGenerator>();
        playerBuilder = GetComponent<PlayerBuilder>();
        environmentBuilder = GetComponent<EnvironmentBuilder>();
    }

    void Start()
    {
        // Tehcnically the "true" start state. The start state below is more to make the game wait until all starting requirements are met before play begins.
        // start the generation of the map.
        mapGenerator.StartMapGenerator();
        // Set starting position for the players.
        playerBuilder.SetPlayerStartPosition();
        // Spawn dynamic environment objects.
        environmentBuilder.CreateEnvironmentalObjects();
    }

    void Update()
    {
        if(gameState == States.Start)
        {
            // Wait for starting animations to play (fade in, animations, etc.), pause/disable anything that should wait.

            // Stop the day/night cycle.
            gameTime.enabled = false;

            // Make sure players remain paused.
            for (int i = 0; i < players.Length; i++)
            {
                if(players[i].controller.state != PlayerController.States.Paused)
                    players[i].controller.ChangeState(PlayerController.States.Paused);
            }
            
            // Determine the condition to transition to the play state. For now just start the game in play state.
            if (true)
            {
                for (int i = 0; i < players.Length; i++)
                {
                    if (players[i].controller.state != PlayerController.States.Day)
                        players[i].controller.ChangeState(PlayerController.States.Day);
                }

                gameState = States.Play;
            }
        }
        if (gameState == States.Play)
        {
            // The actual game loop.

            // Start the day/night cycle.
            gameTime.enabled = true;

            for (int i = 0; i < players.Length; i++)
            {
                // Check for player death, and if found transition to game end state.
                if (players[i].controller.state == PlayerController.States.Dead)
                {
                    gameState = States.End;

                    // There must be a cleaner way to do this...
                    if (i == 0)
                    {
                        winningPlayer = 2;
                        players[1].camera.state = CameraController.States.Win;
                    }
                    if (i == 1)
                    {
                        winningPlayer = 1;
                        players[0].camera.state = CameraController.States.Win;
                    }

                    return;
                }

                // Check game time, and set player state accordingly.
                if (gameTime.currentTime >= 6 && gameTime.currentTime <= 18)
                {
                    if (players[i].controller.state != PlayerController.States.Day && players[i].controller.state != PlayerController.States.Paused)
                        players[i].controller.ChangeState(PlayerController.States.Day);
                }
                else
                {
                    if (players[i].controller.state != PlayerController.States.Night && players[i].controller.state != PlayerController.States.Paused)
                        players[i].controller.ChangeState(PlayerController.States.Night);
                }
            }
        }
        if(gameState == States.End)
        {
            if (!winMenuOpened)
            {
                for (int i = 0; i < players.Length; i++)
                {
                    players[i].controller.state = PlayerController.States.Win;
                }

                gameMenu.ShowWinMenu(winningPlayer);

                winMenuOpened = true;
            }
        }
    }
}
