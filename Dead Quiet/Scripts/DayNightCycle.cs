using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    public float dayLengthInSeconds = 300;
    public float nightLengthInSeconds = 20;
    public float totalLengthInSeconds   // Technically wrong naming convention for a propery, but keeps in line with above fields. Should we change it?
    {
        get
        {
            return dayLengthInSeconds + nightLengthInSeconds;
        }
    }

    public float currentTime { get; private set; }
    public int dayCount { get; private set; }

    public float dayIncrementTime = 6;
    bool daySetToday = false;

    // Audio Triggers
    public bool playSounds = true;
    public GameObject dawnSound;
    public GameObject duskSound;
    bool dawnSoundTriggered = false;
    bool duskSoundTriggered = false;
    Vector2 mapSize;

    void Start()
    {
        currentTime = 6;
        dayCount = 0;

        MapGenerator mapGen = GameObject.FindWithTag("GameController").GetComponent<MapGenerator>();
        mapSize = new Vector2(mapGen.gridSizeX * mapGen.tileSize, mapGen.gridSizeY * mapGen.tileSize);
    }

    void Update()
    {
        float rotation;
        if (transform.eulerAngles.x > 0 && transform.eulerAngles.x < 180)
        {
            rotation = 180f / (dayLengthInSeconds * 60f);

            if (!dawnSoundTriggered && dayCount > 1 && playSounds)
            {
                dawnSoundTriggered = true;
                duskSoundTriggered = false;

                Vector3 soundPosition = Random.value > 0.5f ? new Vector3(Random.Range(-mapSize.x, mapSize.x), 0, Random.value > 0.5f ? -mapSize.y : mapSize.y) : new Vector3(Random.value > 0.5f ? -mapSize.x : mapSize.x, 0, Random.Range(-mapSize.y, mapSize.y));

                Instantiate(dawnSound, soundPosition, Quaternion.identity);
            }
        }
        else
        {
            rotation = 180f / (nightLengthInSeconds * 60f);

            if (!duskSoundTriggered && dayCount > 0 && playSounds)
            {
                dawnSoundTriggered = false;
                duskSoundTriggered = true;

                Vector3 soundPosition = Random.value > 0.5f ? new Vector3(Random.Range(-mapSize.x, mapSize.x), 0, Random.value > 0.5f ? -mapSize.y : mapSize.y) : new Vector3(Random.value > 0.5f ? -mapSize.x : mapSize.x, 0, Random.Range(-mapSize.y, mapSize.y));

                Instantiate(duskSound, soundPosition, Quaternion.identity);
            }
        }

        transform.Rotate(rotation, 0 , 0);

        currentTime += rotation / 15;

        if (currentTime >= 24)
        {
            currentTime = 0;
            daySetToday = false;
        }

        if(currentTime >= dayIncrementTime && !daySetToday)
        {
            dayCount++;
            daySetToday = true;
        }
    }
}
