using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public int edgeLoopWidth = 2;
    public int multiTileAmount = 3;
    public int gridSizeX = 5, gridSizeY = 5;
    public int tileAmount;
    public int tileSize = 10;
    public List<Tile> tileList;
    public List<MultiTile> multiTileList;
    public List<Tile> noGoTile;
    public List<Tile> pathTileList;
    public Tile tempPath;
    public Tile[,] tileMap;
    public int bufferSize = 1;
    Vector2 spawnPos = new Vector2(0, 0);

    Queue<int[]> tileQueue = new Queue<int[]>();

    int tileCount;
    void Start()
    {
        gridSizeX += (edgeLoopWidth * 2);
        gridSizeY += (edgeLoopWidth * 2);
        tileMap = new Tile[gridSizeX, gridSizeY];

    }

    public void StartMapGenerator()
    {
        for (int y = 0; y < gridSizeY; y++)
        {
            for (int x = 0; x < gridSizeX; x++)
            {
                // Builds the fence around the play area.
                if (x == edgeLoopWidth - 1 && y != edgeLoopWidth - 1 && y > edgeLoopWidth - 1 && y < gridSizeY - edgeLoopWidth)
                    tileMap[x, y] = noGoTile[Random.Range(0, 4)];
                if (x != edgeLoopWidth - 1 && y == edgeLoopWidth - 1 && x > edgeLoopWidth - 1 && x < gridSizeX - edgeLoopWidth)
                    tileMap[x, y] = noGoTile[Random.Range(0, 4)];
                if (x != gridSizeX - edgeLoopWidth && y == gridSizeY - edgeLoopWidth && x > edgeLoopWidth - 1 && x < gridSizeX - edgeLoopWidth)
                    tileMap[x, y] = noGoTile[Random.Range(0, 4)];
                if (x == gridSizeX - edgeLoopWidth && y != gridSizeY - edgeLoopWidth && y > edgeLoopWidth - 1 && y < gridSizeY - edgeLoopWidth)
                    tileMap[x, y] = noGoTile[Random.Range(0, 4)];

                // Populates the outside area with tiles.
                if (tileMap[x, y] == null)
                {
                    if (x < edgeLoopWidth)
                        tileMap[x, y] = noGoTile[Random.Range(5, 8)];
                    if (y < edgeLoopWidth)
                        tileMap[x, y] = noGoTile[Random.Range(5, 8)];
                    if (x >= gridSizeX - edgeLoopWidth)
                        tileMap[x, y] = noGoTile[Random.Range(5, 8)];
                    if (y >= gridSizeY - edgeLoopWidth)
                        tileMap[x, y] = noGoTile[Random.Range(5, 8)];
                }
            }
        }
        GenerateMultiTiles();
    }
    public void GenerateMultiTiles()
    {
        int tileCounter = 0;
        List<MultiTile> mtList = new List<MultiTile>(multiTileList);
        List<Vector2> multiTilePos = new List<Vector2>();
        List<MultiTile> chosenTiles = new List<MultiTile>();
        MultiTile multiTile;
        int randomX = 0;
        int randomY = 0;
        int randomNumber = 0;
        do
        {
            randomX = Random.Range(edgeLoopWidth, gridSizeX - (edgeLoopWidth * 2));
            randomY = Random.Range(edgeLoopWidth+1, gridSizeY - (edgeLoopWidth * 2));
            //Debug.Log("===== X: " + randomX + " Y: " + randomY + "=====");

            randomNumber = Random.Range(0, mtList.Count);
            //Debug.Log(randomNumber + " " + mtList.Count);
            multiTile = mtList[randomNumber];
            bool spawn = true;

            if ((randomX + multiTile.rows.Length) <= gridSizeX - edgeLoopWidth && (randomY + multiTile.rows.Length) <= gridSizeY - edgeLoopWidth)
            {
                if (randomX - bufferSize >= 0 && randomY - bufferSize >= 0)
                {

                    for (int y = 0; y < (multiTile.rows.Length + bufferSize* edgeLoopWidth); y++)
                    {
                        if (!spawn)
                            break;
                        //Debug.Log(multiTile.rows.Length);
                        for (int x = 0; x < (multiTile.rows.Length + bufferSize* edgeLoopWidth); x++)
                        {
                           
                            if (tileMap[(randomX-bufferSize) + x, (randomY-bufferSize) + y] != null)
                            {

                                if (tileMap[(randomX - bufferSize) + x, (randomY - bufferSize) + y].GetComponent<MultiTile>() == true)
                                {
                                    //Debug.Log("Can't Spawn!");
                                    spawn = false;
                                    break;
                                }
                            }

                        }
                        

                    }

                    if (spawn)
                    {

                        //Debug.Log("SPAWNING>>>" + randomX + " " + randomY);
                        tileCounter++;
                        multiTilePos.Add(new Vector2(randomX, randomY));
                        chosenTiles.Add(multiTile);
                        tileMap[randomX, randomY] = multiTile.rows[0].tiles[0];
                        GetMultiTile(randomX, randomY, multiTile);
                        mtList.Remove(mtList[randomNumber]);

                    }

                }
            }
            
        } while (tileCounter < multiTileAmount);
        GeneratePath(multiTilePos, chosenTiles);
        GenerateMap();
        SpawnTiles();
    }

    public void GeneratePath(List<Vector2> tilePos, List<MultiTile> tiles)
    {
        int randomNumber;
        List<Vector2> multiTilePos = new List<Vector2>(tilePos);
        List<MultiTile> chosenTleList = new List<MultiTile>(tiles);
        //finding the midpoint
        float x = 0, y = 0;
        int averageX, averageY;
        foreach (Vector2 mt in multiTilePos)
        {
            x += mt.x;
            y += mt.y;
        }
        averageX = Mathf.RoundToInt(x / multiTilePos.Count);
        averageY = Mathf.RoundToInt(y / multiTilePos.Count);

        //generating the path to midpoint.
        int distance = 0;
        for (int i = 0; i < multiTilePos.Count; i++)
        {
            randomNumber = Random.Range(1,100);
            
             
            x = multiTilePos[i].x;
            y = multiTilePos[i].y-1;

            for (int j = -1; j < chosenTleList[i].rows.Length+1; j++)
            {
                for (int l = 0; l < chosenTleList[i].rows[0].tiles.Length+2; l++)
                {
                    if(tileMap[Mathf.RoundToInt(x + j), Mathf.RoundToInt(y + l)] == null)
                    tileMap[Mathf.RoundToInt(x + j), Mathf.RoundToInt(y+l)] = tempPath;
                }
            }
           


            distance = Mathf.Abs(Mathf.RoundToInt(x - averageX));
            if (distance == 1)
            {
                randomNumber = 51;
            }
            if (randomNumber < 50)
            {
                //going along the X axis first
                

                for (int pathX = 0; pathX < distance; pathX++)
                {
                   
                        if (x > averageX)
                        {
                            if(tileMap[Mathf.RoundToInt(averageX + pathX), Mathf.RoundToInt(y)] == null)
                                tileMap[Mathf.RoundToInt(averageX + pathX), Mathf.RoundToInt(y)] = tempPath;
                        }
                        else
                        {
                            if (tileMap[Mathf.RoundToInt(x + pathX), Mathf.RoundToInt(y)] == null)
                                tileMap[Mathf.RoundToInt(x + pathX), Mathf.RoundToInt(y)] = tempPath;

                        }
                    
                }
                distance = Mathf.Abs(Mathf.RoundToInt(y - averageY));
                for (int pathY = 0; pathY <= distance; pathY++)
                {
                    if (y > averageY)
                    {
                        if (tileMap[Mathf.RoundToInt(averageX), Mathf.RoundToInt(averageY + pathY)] == null)
                            tileMap[Mathf.RoundToInt(averageX), Mathf.RoundToInt(averageY + pathY)] = tempPath;
                    }
                    else
                    {
                        if (tileMap[Mathf.RoundToInt(averageX), Mathf.RoundToInt(y + pathY)] == null)
                            tileMap[Mathf.RoundToInt(averageX), Mathf.RoundToInt(y + pathY)] = tempPath;

                    }
                }



            }
            else
            {
                
                
                //going along the Y axis
                distance = Mathf.Abs(Mathf.RoundToInt(y - averageY));
                for (int pathY = 0; pathY <= distance; pathY++)
                {
                    if (y > averageY)
                    {
                        if (tileMap[Mathf.RoundToInt(x), Mathf.RoundToInt(averageY + pathY)] == null)
                            tileMap[Mathf.RoundToInt(x), Mathf.RoundToInt(averageY + pathY)] = tempPath;
                    }
                    else
                    {
                        if (tileMap[Mathf.RoundToInt(x), Mathf.RoundToInt(y + pathY)] == null)
                            tileMap[Mathf.RoundToInt(x), Mathf.RoundToInt(y + pathY)] = tempPath;

                    }
                }

                distance = Mathf.Abs(Mathf.RoundToInt(x - averageX));
                for (int pathX = 0; pathX < distance; pathX++)
                {

                    if (x > averageX)
                    {
                        if (tileMap[Mathf.RoundToInt(averageX + pathX), Mathf.RoundToInt(averageY)] == null)
                            tileMap[Mathf.RoundToInt(averageX + pathX), Mathf.RoundToInt(averageY)] = tempPath;
                    }
                    else
                    {
                        if (tileMap[Mathf.RoundToInt(x + pathX), Mathf.RoundToInt(averageY)] == null)
                            tileMap[Mathf.RoundToInt(x + pathX), Mathf.RoundToInt(averageY)] = tempPath;

                    }

                }

            }
            

        }


        BuildPath();

    }

    void BuildPath() {
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                if(tileMap[x,y] != null)
                    if (tileMap[x, y].pathTile)
                    {
                        tileMap[x,y] = GetPathTile(x, y);
                    }

            }
        }


    }


    int[] GetConnections(int x, int y)
    {
        int[] connections = new int[4];
        for (int i = 0; i < connections.Length; i++)
        {
            connections[i] = 0;
        }

        if (tileMap[x, y + 1] != null)
        {
            if (tileMap[x, y + 1].pathTile || tileMap[x, y + 1].southExit)
                connections[0] = 1;
        }

        if (tileMap[x + 1, y] != null)
        {
            if (tileMap[x + 1, y].pathTile || tileMap[x + 1, y].westExit)
                connections[1] = 1;

        }

        if (tileMap[x, y - 1] != null)
        {
            if (tileMap[x, y - 1].pathTile || tileMap[x, y - 1].northExit)
                connections[2] = 1;

        }

        if (tileMap[x - 1, y] != null)
        {
            if (tileMap[x - 1, y].pathTile || tileMap[x - 1, y].eastExit)
                connections[3] = 1;

        }

        return connections;
    }

    Tile GetPathTile(int x, int y)
    {
        int[] connections = new int[4];
        connections = GetConnections(x,y);

        Tile outputTile = null;
        List<Tile> possibleTileList = new List<Tile>();
        //Debug.Log(connections[0] + ", " + connections[1] + ", " + connections[2] + ", " + connections[3]);

        foreach (Tile tile in pathTileList)
        {
            if ((connections[0] != 1 && tile.northExit) || (connections[0] != 0 && !tile.northExit))
            {
                continue;
            }
            if ((connections[1] != 1 && tile.eastExit) || (connections[1] != 0 && !tile.eastExit))
            {
                continue;
            }
            if ((connections[2] != 1 && tile.southExit) || (connections[2] != 0 && !tile.southExit))
            {
                continue;
            }
            if ((connections[3] != 1 && tile.westExit) || (connections[3] != 0 && !tile.westExit))
            {
                continue;
            }
            possibleTileList.Add(tile);
        }

        if (possibleTileList.Count == 1)
            outputTile = possibleTileList[0];
        else
            Debug.LogError("MORE THAN ONE PATH RETURNED!!     " +  possibleTileList.Count);
        if (possibleTileList.Count == 0)
            Debug.LogError("NO PATH RETURNED!!");

        return outputTile;
    }

    void GetMultiTile(int x, int y, MultiTile currentTile)
    {

        
        foreach (MultiTileSecondDimension multi in currentTile.rows)
        {
            //Debug.Log(multi.tiles.Length);
            for (int x1 = 0; x1 < multi.tiles.Length; x1++)
            {
                    if (tileMap[x + x1, y] == null)
                        tileMap[x + x1, y] = multi.tiles[x1];
            }
            y++;
        }


    }

    void GenerateMap()
    {
        int HayBaleTile = 10;
        int SkullTile = 0;
        int tileAmount = 100;
        int randomNumber;
        Queue<int> previousNumbers = new Queue<int>();
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                if (tileMap[x, y] == null)
                {
                    do
                    {
                        randomNumber = Random.Range(0, tileList.Count);

                        
                        if (randomNumber == HayBaleTile && !previousNumbers.Contains(HayBaleTile))
                        {
                            previousNumbers.Enqueue(randomNumber);
                            tileMap[x, y] = tileList[randomNumber];
                            //tileCount++;
                            
                        } else if (randomNumber == SkullTile && !previousNumbers.Contains(SkullTile)) { 
                            
                            previousNumbers.Enqueue(randomNumber);
                            tileMap[x, y] = tileList[randomNumber];
                            //tileCount++;
                          
                        } else if(randomNumber != HayBaleTile && randomNumber != SkullTile) {
                            previousNumbers.Enqueue(randomNumber);
                            tileMap[x, y] = tileList[randomNumber];
                        }

                    } while (tileMap[x, y] == null);

                  if(previousNumbers.Count == tileAmount)
                    {
                        previousNumbers.Dequeue();
                    }

                }
            }
        }

    }

    void SpawnTiles()
    {
        int[] rotation = new int[4] { 0, 90, 180, 270 };
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                

                if (tileMap[x, y] != null && tileMap[x, y].deadZone)
                {
                    if (x == edgeLoopWidth - 1 && y != edgeLoopWidth - 1 && y > edgeLoopWidth - 1 && y < gridSizeY - (edgeLoopWidth - 1))
                        Instantiate(tileMap[x, y].gameObject, new Vector3(x * tileSize, 0, y * tileSize), Quaternion.Euler(90, 0, 0));
                    if (x != edgeLoopWidth - 1 && y == edgeLoopWidth - 1 && x > edgeLoopWidth - 1 && x < gridSizeX - (edgeLoopWidth - 1))
                        Instantiate(tileMap[x, y].gameObject, new Vector3(x * tileSize, 0, y * tileSize), Quaternion.Euler(90, 270, 0));
                    if (x != gridSizeX - edgeLoopWidth && y == gridSizeY - edgeLoopWidth && x > edgeLoopWidth - 1 && x < gridSizeX - edgeLoopWidth)
                        Instantiate(tileMap[x, y].gameObject, new Vector3(x * tileSize, 0, y * tileSize), Quaternion.Euler(90, 90, 0));
                    if (x == gridSizeX - edgeLoopWidth && y != gridSizeY - edgeLoopWidth && y > edgeLoopWidth - 1 && y < gridSizeY - edgeLoopWidth)
                        Instantiate(tileMap[x, y].gameObject, new Vector3(x * tileSize, 0, y * tileSize), Quaternion.Euler(90, 180, 0));
                }
                if (tileMap[x, y] != null && tileList.Contains(tileMap[x,y]))
                    Instantiate(tileMap[x, y].gameObject, new Vector3(x * tileSize, 0, y * tileSize), Quaternion.Euler(90, rotation[Random.Range(0,4)], 0));

                if (tileMap[x, y] != null && !tileMap[x, y].deadZone && !tileList.Contains(tileMap[x, y]))
                    Instantiate(tileMap[x, y].gameObject, new Vector3(x * tileSize, 0, y * tileSize), Quaternion.Euler(90, 0, 0));

            }
        }

    }


}
