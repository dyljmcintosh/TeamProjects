using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/////////////////////////////////////
//                                 //
//  This implementation serves as  //
//      as a way to visualise      //
//   2D arrays in the inspector    //
//                                 //
/////////////////////////////////////

public class MultiTile : MonoBehaviour
{
    public MultiTileSecondDimension[] rows;
}

[System.Serializable]
public class MultiTileSecondDimension
{
    public Tile[] tiles;
}
