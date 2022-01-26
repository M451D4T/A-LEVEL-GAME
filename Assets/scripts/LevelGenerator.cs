using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    //gameObjects
    public GameObject[] floorTiles;
    public GameObject[] wallTiles;
    public GameObject[] enemy;
    public GameObject exit;
    public GameObject player;
    public GameObject playerCam;
    
    //variables
    public int dungeonWidth;
    public int dungeonHeight;

    enum dungeonTile{empty,floor,wall,testfail}
    [SerializeField]
    dungeonTile[,] grid;


    void Awake() 
    //will run when the object it is attached to is loaded
    {
        prepGrid();
        insertTiles();
    }

    void prepGrid()
    //creates grid and sets all tiles to empty
    {
        grid = new dungeonTile[dungeonWidth,dungeonHeight];
        for(int x=0; x<dungeonWidth-1; x++)
        {
            for(int y=0; y<dungeonHeight-1; y++)
            {
                grid[x,y]=dungeonTile.empty`1;
            }
        }
    }

    void insertTiles()
    //places tiles on corresponding grid position
    {
        for(int x=0; x<dungeonWidth-1; x++)
        {
            for(int y=0; y<dungeonHeight-1; y++)
            {
                switch(grid[x,y])
                {
                    case dungeonTile.empty:
                        break;
                    case dungeonTile.floor:
                        spawn(x,y,floorTiles[Random.Range(0,floorTiles.Length)]);
                        break;
                    case dungeonTile.wall:
                        spawn(x,y,wallTiles[Random.Range(0,wallTiles.Length)]);
                        break;
                    default:
                        Debug.Log("error at position("+new Vector2(x,y)+")");
                        break;
                }
            }
        }
    }

    void spawn(float x, float y, GameObject toSpawn)
    //spawns the game object in the scene
    {
        Instantiate(toSpawn, new Vector3(x,y,0), Quaternion.identity);
    }


}