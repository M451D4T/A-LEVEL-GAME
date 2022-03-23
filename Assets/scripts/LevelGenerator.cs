using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

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
    public int iter_cutoff;
    public float percentToFill;
    public float chanceWalkerChangeDir;
    public float chanceWalkerDestroy;
    public float chanceWalkerSpawn;
    public int maxWalkers;

    enum dungeonTile{empty,floor,wall,nextFloor,testfail}
    dungeonTile[,] grid;
    
    struct RandomWalker 
    {
        public Vector2 dir;
		public Vector2 pos;
	}
    List<RandomWalker> walkers;


    void Awake() 
    //will run when the object it is attached to is loaded
    {
        prepGrid();
        generateFloors();
        placeWall();
        spawnExit();
        insertTiles();
        SpawnPlayer();
        AstarPath.active.Scan();
        Debug.Log(AstarPath.active);    
    }

        void prepGrid()
        //creates grid and sets all tiles to empty
        {
            grid = new dungeonTile[dungeonWidth,dungeonHeight];
            for(int x=0; x<dungeonWidth; x++)
            {
                for(int y=0; y<dungeonHeight; y++)
                {
                    grid[x,y]=dungeonTile.empty;
                }
            }
        //spawns first walker
		walkers = new List<RandomWalker>();
		RandomWalker walker = new RandomWalker();
		walker.dir = RandomDirection();
		Vector2 pos = new Vector2(Mathf.RoundToInt(dungeonWidth/ 2.0f), Mathf.RoundToInt(dungeonHeight/ 2.0f));
		walker.pos = pos;
		walkers.Add(walker);
    }

    void generateFloors()
    //generates floor tiles with walkers
    {
        int iter_floorgen = 0;
        do
        {
            // sets the current position of every walker to a floor tile
            foreach (RandomWalker walker in walkers)
            {
                grid[(int)walker.pos.x,(int)walker.pos.y] = dungeonTile.floor;
            }

            //chance to destroy Walker
			int numberChecks = walkers.Count;
			for (int i = 0; i < numberChecks; i++) 
            {
				if (Random.value < chanceWalkerDestroy && walkers.Count > 1)
                {
					walkers.RemoveAt(i);
					break;
				}
			}

            //chance to spawn new Walker
			numberChecks = walkers.Count;
			for (int i = 0; i < numberChecks; i++)
            {
				if (Random.value < chanceWalkerSpawn && walkers.Count < maxWalkers) 
                {
					RandomWalker walker = new RandomWalker();
					walker.dir = RandomDirection();
					walker.pos = walkers[i].pos;
					walkers.Add(walker);
				}
			}
            // chance for walker to change direction
			for (int i = 0; i < walkers.Count; i++) 
            {
				if (Random.value < chanceWalkerChangeDir)
                {
					RandomWalker walker = walkers[i];
					walker.dir = RandomDirection();
					walkers[i] = walker ;
				}
			}

        	//move Walkers
			for (int i = 0; i < walkers.Count; i++)
            {
				RandomWalker walker = walkers[i];
				walker.pos += walker.dir;
				walkers[i] = walker;				
			}

            //avoid border of grid
			for (int i =0; i < walkers.Count; i++)
            {
				RandomWalker walker = walkers[i];
				walker.pos.x = Mathf.Clamp(walker.pos.x, 1, dungeonWidth-3);
				walker.pos.y = Mathf.Clamp(walker.pos.y, 1, dungeonHeight-3);
				walkers[i] = walker;
            }
            //check to exit loop
            if (((float)floorCount() / (float)grid.Length) > percentToFill)
            {
                break;
            }
            iter_floorgen++;
        }
        while(iter_floorgen < iter_cutoff);
    }

    void placeWall()
    // changes empty spaces around floor to walls
    {
        for (int x = 0; x < dungeonWidth-1; x++)
        {
            for (int y = 0; y < dungeonHeight-1; y++)
            {
				if (grid[x,y] == dungeonTile.floor)
                {
					if (grid[x,y+1] == dungeonTile.empty)//tile to the above
                    {
						grid[x,y+1] = dungeonTile.wall;
					}

					if (grid[x,y-1] == dungeonTile.empty)//tile to the below
                    {
						grid[x,y-1] = dungeonTile.wall;
					}

					if (grid[x+1,y] == dungeonTile.empty)//tile to the right
                    {
						grid[x+1,y] = dungeonTile.wall;
					}
					
                    if (grid[x-1,y] == dungeonTile.empty)//tile to the left
                    {
						grid[x-1,y] = dungeonTile.wall;
					}

                    if (grid[x - 1, y - 1] == dungeonTile.empty)//tile to the bottom left
                    {
                        grid[x - 1, y - 1] = dungeonTile.wall;
                    }

                    if (grid[x - 1, y + 1] == dungeonTile.empty)//tile to the top left
                    {
                        grid[x - 1, y + 1] = dungeonTile.wall;
                    }

                    if (grid[x + 1, y + 1] == dungeonTile.empty)//tile to the top right
                    {
                        grid[x + 1, y + 1] = dungeonTile.wall;
                    }

                    if (grid[x + 1, y - 1] == dungeonTile.empty)//tile to the bottom right
                    {
                        grid[x + 1, y - 1] = dungeonTile.wall;
                    }
                }
            }
		}
	
    }

    


    void insertTiles()
    // places tiles on corresponding grid position
    {
        for(int y=0; y<dungeonWidth-1; y++)
        {
            for(int x=0; x<dungeonHeight-1; x++)
            {
                switch(grid[x,y])
                {
                    case dungeonTile.empty:
                        break;
                    case dungeonTile.floor:
                        spawn(x,y,floorTiles[Random.Range(0,floorTiles.Length)]);
                        break;
                    case dungeonTile.wall:
                        if (grid[x,y+1] == dungeonTile.empty )
                        {
                            spawn(x,y,wallTiles[0]);
                        }
                        else
                        {
                            spawn(x,y,wallTiles[1]);
                        }
                        break;
                    case dungeonTile.nextFloor:
                        spawn(x,y,exit);
                        break;
                    default:
                        Debug.Log("error at position("+new Vector2(x,y)+")");
                        break;
                }
            }
        }
    }

	Vector2 RandomDirection()
    // picks a random direction for walker
    {
        int choice = Mathf.FloorToInt(Random.value * 3.99f);
        switch (choice)
        {
            case 0:
                return Vector2.down;
            case 1:
                return Vector2.left;
            case 2:
                return Vector2.up;
            default:
                return Vector2.right;
        }
	}

    int floorCount()
    //counts floor tiles and returns
    {
        int count = 0;
        foreach(dungeonTile space in grid)
        {
            if(space == dungeonTile.floor)
            {
                count++;
            }
        }
        //Debug.Log("floor count = "+count);
        return count;

    }

    void spawn(float x, float y, GameObject toSpawn)
    // spawns the game object in the scene
    {
        Instantiate(toSpawn, new Vector3(x,y,0), Quaternion.identity);
    }


    void SpawnPlayer()
    //spawn the player
    {
        spawn(Mathf.RoundToInt(dungeonWidth / 2.0f), Mathf.RoundToInt(dungeonHeight / 2.0f), player);
    }

    void spawnExit() 
    {
        Vector2 playerPos = new Vector2(Mathf.RoundToInt(dungeonWidth/ 2.0f),Mathf.RoundToInt(dungeonHeight/ 2.0f));
        Vector2 exitPos = playerPos;
        float exitDistance = 0f;
        //find furthest floortile
		for (int x = 0; x < dungeonWidth - 1; x++)
        {
			for (int y = 0; y < dungeonHeight - 1; y++)
            {
				if (grid[x,y] == dungeonTile.floor)
                {
                    Vector2 nextPos = new Vector2(x, y);
                    float distance = Vector2.Distance(playerPos, nextPos);
                    if (distance > exitDistance) 
                    {
                        exitDistance = distance;
                        exitPos = nextPos;
                    }
                }
            }
        }
        //spawn exit
        grid[(int)exitPos.x,(int)exitPos.y] = dungeonTile.nextFloor;

    }








}