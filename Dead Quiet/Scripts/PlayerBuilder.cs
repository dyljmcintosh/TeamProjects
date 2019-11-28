using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBuilder : MonoBehaviour
{

    GameController gameController;
    public int playerSpawnDistance = 10;
    // Start is called before the first frame update
    void Awake()
    {
        gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();   
    }
    
    public void SetPlayerStartPosition()
    {
        /*
        PlayerController[] playerArray = gameController.playerArray;
        CameraController[] cameraArray = gameController.cameraArray;
        // has to be a better way to convert an array to a List. THERE IS! See below!
        GameObject[] spawnArray = GameObject.FindGameObjectsWithTag("SpawnPoint");
        List<GameObject> spawnPoints = new List<GameObject>();
        spawnPoints.AddRange(spawnArray);
        */
        PlayerData[] players = gameController.players;
        
        List<GameObject> spawnPoints = new List<GameObject>();
        spawnPoints.AddRange(GameObject.FindGameObjectsWithTag("SpawnPoint"));

        int randomNumber;
        randomNumber = Random.Range(0, spawnPoints.Count);
        players[0].controller.gameObject.transform.position = spawnPoints[randomNumber].transform.position;
        players[0].camera.gameObject.transform.position = players[0].controller.gameObject.transform.position;
        spawnPoints.RemoveAt(randomNumber);

        bool spawning = false;
        do
        {
            randomNumber = Random.Range(0, spawnPoints.Count);

            //Debug.Log("Distance: " + Vector3.Distance(players[0].controller.gameObject.transform.position, spawnPoints[randomNumber].transform.position));

            if (Vector3.Distance(players[0].controller.gameObject.transform.position, spawnPoints[randomNumber].transform.position) > playerSpawnDistance)
            {

                players[1].controller.gameObject.transform.position = spawnPoints[randomNumber].transform.position;
                players[1].camera.gameObject.transform.position = players[1].controller.gameObject.transform.position;
                spawnPoints.RemoveAt(randomNumber);
                spawning = true;
            }
            
        } while (!spawning);


        
    }

}
