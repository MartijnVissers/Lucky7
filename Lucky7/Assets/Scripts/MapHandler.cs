using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MapHandler : MonoBehaviour
{
    [SerializeField] private int Width, Length;
    [SerializeField] MapItem roadStraight, roadCorner, roadTshape, roadCrossRoad;
    enum RoadOrientation {  LeftToRight = 0, UpToDown = 90,
                            TopFacing = 0, RightFacing = 90, DownFacing = 180, LeftFacing = 270,
                            UpperLeftCorner = 0, UpperRightCorner = 90, LowerRightCorner = 180, LowerLeftCorner = 270,
                            CrossRoad = 0 };
    [SerializeField] List<MapItem> housePrefabs;
    [SerializeField] MapItem truckDepotPrefab;
    [SerializeField] List<Trash> ThrashPrefabs;
    [SerializeField] GarbageTruck GarbageTruck;

    // some buildings should be able to spawn thrash on the street next to it.

    private MapItem[,] GameMap;
    //A MapItem is either a Building, truckdepot or Road.


    // Start is called before the first frame update
    void Start()
    {
        if( Width < 10 )
        {
            Width = 10;
        }
        if( Length < 10 )
        {
            Length = 10;
        }
        GameMap = new MapItem[Width, Length];
        //unless defined otherwise, we now have an 10 by 10 map.
        //it still has to be generated though.

        //LoadTestingMap(); //this will instantiate a premade testing map.
        LoadTestingMap(); //this will instantiate a premade playing map.
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*  This function creates a simple map for testing.
     *  the map is 9x9 and not random.
     */
    void LoadTestingMap()
    {
        Width = 9;
        Length = 9;

        GameMap = new MapItem[Length, Width]; //test map is 9 by 9 for testing reasons
        GameMap[4, 4] = Instantiate(truckDepotPrefab, new Vector3(4, 0, 4), Quaternion.identity); //place the depot in the middle

        PlaceRoad(3, 3);//Now surround the depot with road. this should enable you to drive into the depot.
        PlaceRoad(3, 4);
        PlaceRoad(3, 5);

        PlaceRoad(4, 3);
        PlaceRoad(4, 4);
        PlaceRoad(4, 5);

        PlaceRoad(5, 3);
        PlaceRoad(5, 4);
        PlaceRoad(5, 5);

        for(int i = 1; i < 8; i++ ) //instantiate all the borders with roads.
        {
            PlaceRoad(i, 0);
            PlaceRoad(i, 8);

            PlaceRoad(0, i);
            PlaceRoad(8, i);
        }

        PlaceRoad(0, 0);//instantiate all corners with roads.
        PlaceRoad(0, 8);
        PlaceRoad(8, 0);
        PlaceRoad(8, 8);

        PlaceRoad(4, 1);//add a single accesspoint to the depot. ( a single point is enough for now )
        PlaceRoad(4, 2);
        PlaceTruck(4, 1);

        PlaceBuilding(1, 1);//add buildings next to each corner
        PlaceBuilding(1, 7);
        PlaceBuilding(7, 1);
        PlaceBuilding(7, 7);
    }



    /*  this will load a 21x21 map. 
     *  this map is meant for playing.
     *  this map is not random.
     */
    void LoadGameMap()
    {
        GameMap = new MapItem[21, 21]; //generate an empty 21x21 sized map

        GameMap[10, 10] = Instantiate(truckDepotPrefab, new Vector3(10, 0, 10), Quaternion.identity); //place the depot center piece
        PlaceRoad(0, 0, roadCorner, RoadOrientation.UpperLeftCorner);

    }

    /*  This will place a building on GameMap[x,y]
     *  Aditionally, the building will be allocated a road to dump trash on.
     *  If not placed next to a road, it will not be placed.
     */
    void PlaceBuilding( int x, int y )
    {
        Road r = FindAdjacentRoad(x, y);
        if( r != null )
        {
            if (housePrefabs.Count > 0 )
            {
                float result = Random.Range(0, housePrefabs.Count);
                MapItem tmp = Instantiate(housePrefabs[Mathf.RoundToInt(result)], new Vector3(x, 0, y), Quaternion.identity);
                tmp.GetComponent<Building>().SetDumpLocation(r);
                GameMap[x, y] = tmp;
            }
        }
    }

    /*  This will find a road adjacent ( or right on, dont @ me )
     *  to the GameItem at x, y.
     *  returns 0 when no road could be found. 
     */
    Road FindAdjacentRoad( int x, int y )
    {
        for( int i = x-1; i <= x+1; i++ )
        {
            for( int j = y-1; j <= y+1; j++ )
            {
                if( i >= 0 && i < Length && j >= 0 && j < Width )
                {
                    if( GameMap[i,j] != null )
                    {
                        Road r = GameMap[i, j].GetComponent<Road>();
                        if (r != null)
                        {
                            return r;
                        }
                    }
                }
            }
        }
        return null;
    }

    void PlaceRoad( int x, int y, MapItem roadType, RoadOrientation orientation )
    {
        MapItem tmp = Instantiate(roadType, new Vector3(x, 0, y), new Quaternion(0, (int)orientation, 0, 0));
        tmp.GetComponent<Road>().trashPrefabs = ThrashPrefabs;
        GameMap[x, y] = tmp;
    }

    void PlaceRoad( int x, int y )
    {
        MapItem tmp = Instantiate(roadStraight, new Vector3(x, 0, y), Quaternion.identity );
        tmp.GetComponent<Road>().trashPrefabs = ThrashPrefabs;
        GameMap[x, y] = tmp;
    }

    void PlaceTruck( int x, int y )
    {
        Instantiate(GarbageTruck, new Vector3(x, 0, y), Quaternion.identity);
    }
}
