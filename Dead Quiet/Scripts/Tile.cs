using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public float tileSize = 20; // Temp? Not sure if it's better handled here or the mapgen.

    // Exits
    public bool northExit;
    public bool eastExit;
    public bool southExit;
    public bool westExit;

    public bool pathTile;

    // We might need to figure out if the tile is the deadZone around the map.
    public bool deadZone;



    //// Does this tile require specific neighbour tiles?
    //public bool forceNorthNeighbour;
    //public bool forceEastNeighbour;
    //public bool forceSouthNeighbour;
    //public bool forceWestNeighbour;

    //// Neighbour tiles
    //public Tile northNeighbour;
    //public Tile eastNeighbour;
    //public Tile southNeighbour;
    //public Tile westNeighbour;

    //void OnDrawGizmos()
    //{
    //    Gizmos.DrawWireCube(transform.position + transform.up * (tileSize / 2), Vector3.one * tileSize);
    //}
}
