using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentBuilder : MonoBehaviour
{
    public float mapEdgeBuffer = 1;

    // Tumbleweed
    public int tumbleweedCount = 2;
    public GameObject tumbleweed;

    // Birds
    public int birdCount = 5;
    public GameObject bird;

    public void CreateEnvironmentalObjects()
    {
        MapGenerator mapGen = GameObject.FindWithTag("GameController").GetComponent<MapGenerator>();
        Vector3 playAreaStart = new Vector3(mapGen.tileSize * (mapGen.edgeLoopWidth - 0.5f + mapEdgeBuffer), 0, mapGen.tileSize * (mapGen.edgeLoopWidth - 0.5f + mapEdgeBuffer));
        Vector3 playAreaEnd = new Vector3((mapGen.gridSizeX - mapGen.edgeLoopWidth - 0.5f - mapEdgeBuffer) * mapGen.tileSize, 0, (mapGen.gridSizeY - mapGen.edgeLoopWidth - 0.5f - mapEdgeBuffer) * mapGen.tileSize);
        
        // Spawn Tumbleweed
        for (int i = 0; i < tumbleweedCount; i++)
        {
            bool spawned = false;
            Vector3 spawnPos;
            
            while (!spawned)
            {
                spawnPos = RandomPosition(playAreaStart, playAreaEnd);

                if (Physics.OverlapSphere(spawnPos, 1).Length <= 1)
                {
                    Instantiate(tumbleweed, spawnPos, RandomRotation());
                    spawned = true;
                }
                else
                {
                    Debug.Log("Tumbleweed failed to spawn. Retrying...");
                }
            }
        }

        // Spawn Birds
        for (int i = 0; i < birdCount; i++)
        {
            Instantiate(bird, RandomPosition(playAreaStart, playAreaEnd, 0), RandomRotation());
        }
    }

    Vector3 RandomPosition(Vector3 startPos, Vector3 endPos, float height=1)
    {
        return new Vector3(Random.Range(startPos.x, endPos.x), height, Random.Range(startPos.z, endPos.z));
    }

    Quaternion RandomRotation()
    {
        return Quaternion.Euler(0, Random.Range(0, 360), 0);
    }

    /*
    void OnDrawGizmos()
    {
        MapGenerator mapGen = GameObject.FindWithTag("GameController").GetComponent<MapGenerator>();
        Vector3 playAreaStart = new Vector3(mapGen.tileSize * (mapGen.edgeLoopWidth - 0.5f + mapEdgeBuffer), 0, mapGen.tileSize * (mapGen.edgeLoopWidth - 0.5f + mapEdgeBuffer));
        Vector3 playAreaEnd = new Vector3((mapGen.gridSizeX - mapGen.edgeLoopWidth - 0.5f - mapEdgeBuffer) * mapGen.tileSize, 0, (mapGen.gridSizeY - mapGen.edgeLoopWidth - 0.5f - mapEdgeBuffer) * mapGen.tileSize);

        Gizmos.DrawSphere(playAreaStart, 0.1f);
        Gizmos.DrawSphere(playAreaEnd, 0.1f);
    }
    */
}
