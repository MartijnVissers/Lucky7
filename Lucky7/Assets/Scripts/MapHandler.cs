using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MapHandler : MonoBehaviour
{
    [SerializeField] private int Width, Length;
    [SerializeField] MapItem roadPreFab;
    [SerializeField] MapItem buildingPreFabA, buildingPreFabB, buildingPreFabC;
    [SerializeField] MapItem truckDepotPrefab;

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

        LoadPreMadeMap(); //this will instantiate a premade map.
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*  This function creates a simple map for testing.
     *  the map is 9x9 and not random.
     */
    void LoadPreMadeMap()
    {
        Width = 9;
        Length = 9;

        GameMap = new MapItem[Length, Width]; //test map is 9 by 9 for testing reasons
        GameMap[4, 4] = Instantiate(truckDepotPrefab, new Vector3(4, 0, 4), Quaternion.identity); //place the depot in the middle

        GameMap[3, 3] = Instantiate(roadPreFab, new Vector3(3, 0, 3), Quaternion.identity); //Now surround the depot with road. this should enable you to drive into the depot.
        GameMap[3, 4] = Instantiate(roadPreFab, new Vector3(3, 0, 4), Quaternion.identity);
        GameMap[3, 5] = Instantiate(roadPreFab, new Vector3(3, 0, 5), Quaternion.identity);

        GameMap[4, 3] = Instantiate(roadPreFab, new Vector3(4, 0, 3), Quaternion.identity);
        GameMap[4, 4] = Instantiate(roadPreFab, new Vector3(4, 0, 4), Quaternion.identity);
        GameMap[4, 5] = Instantiate(roadPreFab, new Vector3(4, 0, 5), Quaternion.identity);

        GameMap[5, 3] = Instantiate(roadPreFab, new Vector3(5, 0, 3), Quaternion.identity);
        GameMap[5, 4] = Instantiate(roadPreFab, new Vector3(5, 0, 4), Quaternion.identity);
        GameMap[5, 5] = Instantiate(roadPreFab, new Vector3(5, 0, 5), Quaternion.identity);

        for(int i = 1; i < 8; i++ ) //instantiate all the borders with roads.
        {
            GameMap[i, 0] = Instantiate(roadPreFab, new Vector3(i, 0, 0), Quaternion.identity);
            GameMap[i, 0] = Instantiate(roadPreFab, new Vector3(i, 0, 8), Quaternion.identity);

            GameMap[0, i] = Instantiate(roadPreFab, new Vector3(0, 0, i), Quaternion.identity);
            GameMap[0, i] = Instantiate(roadPreFab, new Vector3(8, 0, i), Quaternion.identity);
        }
        GameMap[0, 0] = Instantiate(roadPreFab, new Vector3(0, 0, 0), Quaternion.identity); //instantiate all corners with roads.
        GameMap[0, 8] = Instantiate(roadPreFab, new Vector3(0, 0, 8), Quaternion.identity);
        GameMap[8, 0] = Instantiate(roadPreFab, new Vector3(8, 0, 0), Quaternion.identity);
        GameMap[8, 8] = Instantiate(roadPreFab, new Vector3(8, 0, 8), Quaternion.identity);

        GameMap[4, 1] = Instantiate(roadPreFab, new Vector3(4, 0, 1), Quaternion.identity); //add a single accesspoint to the depot. ( a single point is enough for now )
        GameMap[4, 2] = Instantiate(roadPreFab, new Vector3(4, 0, 2), Quaternion.identity);

        GameMap[1, 1] = Instantiate(buildingPreFabA, new Vector3(1, 0, 1), Quaternion.identity); //add buildings next to each corner
        GameMap[1, 7] = Instantiate(buildingPreFabA, new Vector3(1, 0, 7), Quaternion.identity);
        GameMap[7, 1] = Instantiate(buildingPreFabA, new Vector3(7, 0, 1), Quaternion.identity);
        GameMap[7, 7] = Instantiate(buildingPreFabA, new Vector3(7, 0, 7), Quaternion.identity);
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
            MapItem tmp = Instantiate(buildingPreFabA, new Vector3(x, 1, y), Quaternion.identity);
            tmp.GetComponent<Building>().SetDumpLocation(r);
            GameMap[x, y] = tmp;
        }
    }

    /*  This will find a road adjacent ( or right on, dont @ me )
     *  to the GameItem at x, y.
     *  returns 0 when no road could be found. 
     */
    Road FindAdjacentRoad( int x, int y )
    {
        for( int i = x-1; i < x+1; i++ )
        {
            for( int j = y-1; j < y+1; y++ )
            {
                if( i >= 0 && i < Length && j >= 0 && j < Width )
                {
                    Road r = GameMap[i, j].GetComponent<Road>();
                    if( r != null )
                    {
                        return r;
                    }
                }
            }
        }
        return null;
    }
}
