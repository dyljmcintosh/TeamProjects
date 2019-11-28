using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdOverhead : MonoBehaviour
{
    public float speedMin = 2;
    public float speedMax = 5;
    float speed;

    [HideInInspector]
    public Vector3 playAreaStart;
    [HideInInspector]
    public Vector3 playAreaEnd;

    public float mapEdgeBuffer = 1;

    // Edge wrapping
    bool inPlayArea = true;

    // Material Animation
    public Texture[] textures;
    public float animationMultiplier = 1;

    void Start()
    {
        speed = Random.Range(speedMin, speedMax);

        MapGenerator mapGen = GameObject.FindWithTag("GameController").GetComponent<MapGenerator>();
        playAreaStart = new Vector3(-mapEdgeBuffer, 0, -mapEdgeBuffer);
        playAreaEnd = new Vector3(mapGen.gridSizeX * mapGen.tileSize + mapEdgeBuffer, 0, mapGen.gridSizeY * mapGen.tileSize + mapEdgeBuffer);

        StartCoroutine(Animate());
    }

    void Update()
    {
        // Outside Check
        if (transform.position.x < playAreaStart.x && inPlayArea)
        {
            transform.position = new Vector3(playAreaEnd.x, transform.position.y, transform.position.z);

            inPlayArea = false;
        }
        if (transform.position.x > playAreaEnd.x && inPlayArea)
        {
            transform.position = new Vector3(playAreaStart.x, transform.position.y, transform.position.z);

            inPlayArea = false;
        }
        if (transform.position.z < playAreaStart.z && inPlayArea)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, playAreaEnd.z);

            inPlayArea = false;
        }
        if (transform.position.z > playAreaEnd.z && inPlayArea)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, playAreaStart.z);

            inPlayArea = false;
        }

        // Inside Check
        if (transform.position.x >= playAreaStart.x && transform.position.x <= playAreaEnd.x && transform.position.z >= playAreaStart.z && transform.position.z <= playAreaEnd.z)
        {
            inPlayArea = true;
        }

        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    IEnumerator Animate()
    {
        Projector projector = GetComponentInChildren<Projector>();

        int count = 0;

        while (true)
        {
            //projector.material.mainTexture = textures[count];
            projector.material.SetTexture("_ShadowTex", textures[count]);
            count++;
            
            if (count >= textures.Length)
                count = 0;

            yield return new WaitForSeconds(0.0167f * animationMultiplier);
            //yield return new WaitForSeconds(0.5f);
        }
        
        //yield return null;
    }
}
