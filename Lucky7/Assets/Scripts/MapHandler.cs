using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MapHandler : MonoBehaviour
{
    [SerializeField] private int Width, Length;
    [SerializeField] MapItem roadStraight, roadCorner, roadTshape, roadCrossRoad;
    enum RoadOrientation
    {
        LeftToRight = 90, UpToDown = 0,
        TopFacing = 180, RightFacing = 90, DownFacing = 0, LeftFacing = 270,
        UpperLeftCorner = 90, UpperRightCorner = 0, LowerRightCorner = 270, LowerLeftCorner = 180,
        CrossRoad = 0
    };
    //[SerializeField] private Text points;
    [SerializeField] List<MapItem> housePrefabs;
    [SerializeField] MapItem parkPrefab;
    [SerializeField] MapItem truckDepotPrefab;
    [SerializeField] List<Trash> ThrashPrefabs;
    [SerializeField] GarbageTruck GarbageTruck;
    [SerializeField] Text pointstext;
    [SerializeField] MapItem InvisableWall;
    private int Score;

    // some buildings should be able to spawn thrash on the street next to it.

    private MapItem[,] GameMap;
    //A MapItem is either a Building, truckdepot or Road.


    // Start is called before the first frame update
    void Start()
    {
        if (Width < 10)
        {
            Width = 10;
        }
        if (Length < 10)
        {
            Length = 10;
        }
        GameMap = new MapItem[Width, Length];
        Score = 0;
        pointstext.text = "Points: " + Score.ToString();
        //unless defined otherwise, we now have an 10 by 10 map.
        //it still has to be generated though.

        //LoadTestingMap(); //this will instantiate a premade testing map.
        LoadGameMap(); //this will instantiate a premade playing map.
        GenerateBorders();
        PlaceTruck(10, 12);
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

        for (int i = 1; i < 8; i++) //instantiate all the borders with roads.
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
        Width = 21;
        Length = 21;
        GameMap[10, 10] = Instantiate(truckDepotPrefab, new Vector3(10, 0, 10), Quaternion.identity); //place the depot center piece

        // 0 Row Roads
        PlaceRoad(0, 0, roadCorner, RoadOrientation.UpperLeftCorner);
        PlaceRoad(1, 0, roadStraight, RoadOrientation.LeftToRight);
        PlaceRoad(2, 0, roadStraight, RoadOrientation.LeftToRight);
        PlaceRoad(3, 0, roadStraight, RoadOrientation.LeftToRight);
        PlaceRoad(4, 0, roadTshape, RoadOrientation.DownFacing);
        PlaceRoad(5, 0, roadStraight, RoadOrientation.LeftToRight);
        PlaceRoad(6, 0, roadStraight, RoadOrientation.LeftToRight);
        PlaceRoad(7, 0, roadStraight, RoadOrientation.LeftToRight);
        PlaceRoad(8, 0, roadStraight, RoadOrientation.LeftToRight);
        PlaceRoad(9, 0, roadStraight, RoadOrientation.LeftToRight);
        PlaceRoad(10, 0, roadTshape, RoadOrientation.DownFacing);
        PlaceRoad(11, 0, roadStraight, RoadOrientation.LeftToRight);
        PlaceRoad(12, 0, roadStraight, RoadOrientation.LeftToRight);
        PlaceRoad(13, 0, roadStraight, RoadOrientation.LeftToRight);
        PlaceRoad(14, 0, roadStraight, RoadOrientation.LeftToRight);
        PlaceRoad(15, 0, roadStraight, RoadOrientation.LeftToRight);
        PlaceRoad(16, 0, roadTshape, RoadOrientation.DownFacing);
        PlaceRoad(17, 0, roadStraight, RoadOrientation.LeftToRight);
        PlaceRoad(18, 0, roadStraight, RoadOrientation.LeftToRight);
        PlaceRoad(19, 0, roadStraight, RoadOrientation.LeftToRight);
        PlaceRoad(20, 0, roadCorner, RoadOrientation.UpperRightCorner);

        // 1/2/3/4 Row Roads
        PlaceRoad(0, 1, roadStraight, RoadOrientation.UpToDown);
        PlaceRoad(0, 2, roadStraight, RoadOrientation.UpToDown);
        PlaceRoad(0, 3, roadStraight, RoadOrientation.UpToDown);
        PlaceRoad(0, 4, roadStraight, RoadOrientation.UpToDown);
        PlaceRoad(4, 1, roadStraight, RoadOrientation.UpToDown);
        PlaceRoad(4, 2, roadStraight, RoadOrientation.UpToDown);
        PlaceRoad(4, 3, roadStraight, RoadOrientation.UpToDown);
        PlaceRoad(4, 4, roadStraight, RoadOrientation.UpToDown);
        PlaceRoad(10, 1, roadStraight, RoadOrientation.UpToDown);
        PlaceRoad(10, 2, roadStraight, RoadOrientation.UpToDown);
        PlaceRoad(10, 3, roadStraight, RoadOrientation.UpToDown);
        PlaceRoad(10, 4, roadStraight, RoadOrientation.UpToDown);
        PlaceRoad(16, 1, roadStraight, RoadOrientation.UpToDown);
        PlaceRoad(16, 2, roadStraight, RoadOrientation.UpToDown);
        PlaceRoad(16, 3, roadStraight, RoadOrientation.UpToDown);
        PlaceRoad(16, 4, roadStraight, RoadOrientation.UpToDown);
        PlaceRoad(20, 1, roadStraight, RoadOrientation.UpToDown);
        PlaceRoad(20, 2, roadStraight, RoadOrientation.UpToDown);
        PlaceRoad(20, 3, roadStraight, RoadOrientation.UpToDown);
        PlaceRoad(20, 4, roadStraight, RoadOrientation.UpToDown);

        // 5 Row Roads
        PlaceRoad(0, 5, roadTshape, RoadOrientation.RightFacing);
        PlaceRoad(1, 5, roadStraight, RoadOrientation.LeftToRight);
        PlaceRoad(2, 5, roadStraight, RoadOrientation.LeftToRight);
        PlaceRoad(3, 5, roadStraight, RoadOrientation.LeftToRight);
        PlaceRoad(4, 5, roadTshape, RoadOrientation.TopFacing);
        PlaceRoad(5, 5, roadStraight, RoadOrientation.LeftToRight);
        PlaceRoad(6, 5, roadTshape, RoadOrientation.DownFacing);
        PlaceRoad(7, 5, roadStraight, RoadOrientation.LeftToRight);
        PlaceRoad(8, 5, roadStraight, RoadOrientation.LeftToRight);
        PlaceRoad(9, 5, roadStraight, RoadOrientation.LeftToRight);
        PlaceRoad(10, 5, roadCrossRoad, RoadOrientation.CrossRoad);
        PlaceRoad(11, 5, roadStraight, RoadOrientation.LeftToRight);
        PlaceRoad(12, 5, roadStraight, RoadOrientation.LeftToRight);
        PlaceRoad(13, 5, roadStraight, RoadOrientation.LeftToRight);
        PlaceRoad(14, 5, roadTshape, RoadOrientation.DownFacing);
        PlaceRoad(15, 5, roadStraight, RoadOrientation.LeftToRight);
        PlaceRoad(16, 5, roadTshape, RoadOrientation.TopFacing);
        PlaceRoad(17, 5, roadStraight, RoadOrientation.LeftToRight);
        PlaceRoad(18, 5, roadStraight, RoadOrientation.LeftToRight);
        PlaceRoad(19, 5, roadStraight, RoadOrientation.LeftToRight);
        PlaceRoad(20, 5, roadTshape, RoadOrientation.LeftFacing);

        // 6/7/8/9
        PlaceRoad(0, 6, roadStraight, RoadOrientation.UpToDown);
        PlaceRoad(0, 7, roadStraight, RoadOrientation.UpToDown);
        PlaceRoad(0, 8, roadStraight, RoadOrientation.UpToDown);
        PlaceRoad(0, 9, roadStraight, RoadOrientation.UpToDown);
        PlaceRoad(6, 6, roadStraight, RoadOrientation.UpToDown);
        PlaceRoad(6, 7, roadStraight, RoadOrientation.UpToDown);
        PlaceRoad(6, 8, roadStraight, RoadOrientation.UpToDown);
        PlaceRoad(6, 9, roadStraight, RoadOrientation.UpToDown);
        PlaceRoad(10, 6, roadStraight, RoadOrientation.UpToDown);
        PlaceRoad(10, 7, roadStraight, RoadOrientation.UpToDown);
        PlaceRoad(10, 8, roadStraight, RoadOrientation.UpToDown);
        PlaceRoad(10, 9, roadStraight, RoadOrientation.UpToDown);
        PlaceRoad(14, 6, roadStraight, RoadOrientation.UpToDown);
        PlaceRoad(14, 7, roadStraight, RoadOrientation.UpToDown);
        PlaceRoad(14, 8, roadStraight, RoadOrientation.UpToDown);
        PlaceRoad(14, 9, roadStraight, RoadOrientation.UpToDown);
        PlaceRoad(20, 6, roadStraight, RoadOrientation.UpToDown);
        PlaceRoad(20, 7, roadStraight, RoadOrientation.UpToDown);
        PlaceRoad(20, 8, roadStraight, RoadOrientation.UpToDown);
        PlaceRoad(20, 9, roadStraight, RoadOrientation.UpToDown);

        // 10 Row Roads
        PlaceRoad(0, 10, roadTshape, RoadOrientation.RightFacing);
        PlaceRoad(1, 10, roadStraight, RoadOrientation.LeftToRight);
        PlaceRoad(2, 10, roadStraight, RoadOrientation.LeftToRight);
        PlaceRoad(3, 10, roadStraight, RoadOrientation.LeftToRight);
        PlaceRoad(4, 10, roadStraight, RoadOrientation.LeftToRight);
        PlaceRoad(5, 10, roadStraight, RoadOrientation.LeftToRight);
        PlaceRoad(6, 10, roadCrossRoad, RoadOrientation.CrossRoad);
        PlaceRoad(7, 10, roadStraight, RoadOrientation.LeftToRight);
        PlaceRoad(8, 10, roadStraight, RoadOrientation.LeftToRight);
        PlaceRoad(9, 10, roadStraight, RoadOrientation.LeftToRight);
        PlaceRoad(10, 10, roadCrossRoad, RoadOrientation.CrossRoad);
        PlaceRoad(11, 10, roadStraight, RoadOrientation.LeftToRight);
        PlaceRoad(12, 10, roadStraight, RoadOrientation.LeftToRight);
        PlaceRoad(13, 10, roadStraight, RoadOrientation.LeftToRight);
        PlaceRoad(14, 10, roadCrossRoad, RoadOrientation.CrossRoad);
        PlaceRoad(15, 10, roadStraight, RoadOrientation.LeftToRight);
        PlaceRoad(16, 10, roadStraight, RoadOrientation.LeftToRight);
        PlaceRoad(17, 10, roadStraight, RoadOrientation.LeftToRight);
        PlaceRoad(18, 10, roadStraight, RoadOrientation.LeftToRight);
        PlaceRoad(19, 10, roadStraight, RoadOrientation.LeftToRight);
        PlaceRoad(20, 10, roadTshape, RoadOrientation.LeftFacing);

        // 11/12/13/14
        PlaceRoad(0, 11, roadStraight, RoadOrientation.UpToDown);
        PlaceRoad(0, 12, roadStraight, RoadOrientation.UpToDown);
        PlaceRoad(0, 13, roadStraight, RoadOrientation.UpToDown);
        PlaceRoad(0, 14, roadStraight, RoadOrientation.UpToDown);
        PlaceRoad(6, 11, roadStraight, RoadOrientation.UpToDown);
        PlaceRoad(6, 12, roadStraight, RoadOrientation.UpToDown);
        PlaceRoad(6, 13, roadStraight, RoadOrientation.UpToDown);
        PlaceRoad(6, 14, roadStraight, RoadOrientation.UpToDown);
        PlaceRoad(10, 11, roadStraight, RoadOrientation.UpToDown);
        PlaceRoad(10, 12, roadStraight, RoadOrientation.UpToDown);
        PlaceRoad(10, 13, roadStraight, RoadOrientation.UpToDown);
        PlaceRoad(10, 14, roadStraight, RoadOrientation.UpToDown);
        PlaceRoad(14, 11, roadStraight, RoadOrientation.UpToDown);
        PlaceRoad(14, 12, roadStraight, RoadOrientation.UpToDown);
        PlaceRoad(14, 13, roadStraight, RoadOrientation.UpToDown);
        PlaceRoad(14, 14, roadStraight, RoadOrientation.UpToDown);
        PlaceRoad(20, 11, roadStraight, RoadOrientation.UpToDown);
        PlaceRoad(20, 12, roadStraight, RoadOrientation.UpToDown);
        PlaceRoad(20, 13, roadStraight, RoadOrientation.UpToDown);
        PlaceRoad(20, 14, roadStraight, RoadOrientation.UpToDown);

        // 15 Row Roads
        PlaceRoad(0, 15, roadTshape, RoadOrientation.RightFacing);
        PlaceRoad(1, 15, roadStraight, RoadOrientation.LeftToRight);
        PlaceRoad(2, 15, roadStraight, RoadOrientation.LeftToRight);
        PlaceRoad(3, 15, roadStraight, RoadOrientation.LeftToRight);
        PlaceRoad(4, 15, roadTshape, RoadOrientation.DownFacing);
        PlaceRoad(5, 15, roadStraight, RoadOrientation.LeftToRight);
        PlaceRoad(6, 15, roadTshape, RoadOrientation.TopFacing);
        PlaceRoad(7, 15, roadStraight, RoadOrientation.LeftToRight);
        PlaceRoad(8, 15, roadStraight, RoadOrientation.LeftToRight);
        PlaceRoad(9, 15, roadStraight, RoadOrientation.LeftToRight);
        PlaceRoad(10, 15, roadCrossRoad, RoadOrientation.CrossRoad);
        PlaceRoad(11, 15, roadStraight, RoadOrientation.LeftToRight);
        PlaceRoad(12, 15, roadStraight, RoadOrientation.LeftToRight);
        PlaceRoad(13, 15, roadStraight, RoadOrientation.LeftToRight);
        PlaceRoad(14, 15, roadTshape, RoadOrientation.TopFacing);
        PlaceRoad(15, 15, roadStraight, RoadOrientation.LeftToRight);
        PlaceRoad(16, 15, roadTshape, RoadOrientation.DownFacing);
        PlaceRoad(17, 15, roadStraight, RoadOrientation.LeftToRight);
        PlaceRoad(18, 15, roadStraight, RoadOrientation.LeftToRight);
        PlaceRoad(19, 15, roadStraight, RoadOrientation.LeftToRight);
        PlaceRoad(20, 15, roadTshape, RoadOrientation.LeftFacing);

        // 16/17/18/19 Row Roads
        PlaceRoad(0, 16, roadStraight, RoadOrientation.UpToDown);
        PlaceRoad(0, 17, roadStraight, RoadOrientation.UpToDown);
        PlaceRoad(0, 18, roadStraight, RoadOrientation.UpToDown);
        PlaceRoad(0, 19, roadStraight, RoadOrientation.UpToDown);
        PlaceRoad(4, 16, roadStraight, RoadOrientation.UpToDown);
        PlaceRoad(4, 17, roadStraight, RoadOrientation.UpToDown);
        PlaceRoad(4, 18, roadStraight, RoadOrientation.UpToDown);
        PlaceRoad(4, 19, roadStraight, RoadOrientation.UpToDown);
        PlaceRoad(10, 16, roadStraight, RoadOrientation.UpToDown);
        PlaceRoad(10, 17, roadStraight, RoadOrientation.UpToDown);
        PlaceRoad(10, 18, roadStraight, RoadOrientation.UpToDown);
        PlaceRoad(10, 19, roadStraight, RoadOrientation.UpToDown);
        PlaceRoad(16, 16, roadStraight, RoadOrientation.UpToDown);
        PlaceRoad(16, 17, roadStraight, RoadOrientation.UpToDown);
        PlaceRoad(16, 18, roadStraight, RoadOrientation.UpToDown);
        PlaceRoad(16, 19, roadStraight, RoadOrientation.UpToDown);
        PlaceRoad(20, 16, roadStraight, RoadOrientation.UpToDown);
        PlaceRoad(20, 17, roadStraight, RoadOrientation.UpToDown);
        PlaceRoad(20, 18, roadStraight, RoadOrientation.UpToDown);
        PlaceRoad(20, 19, roadStraight, RoadOrientation.UpToDown);

        // 20 Row Roads
        PlaceRoad(0, 20, roadCorner, RoadOrientation.LowerLeftCorner);
        PlaceRoad(1, 20, roadStraight, RoadOrientation.LeftToRight);
        PlaceRoad(2, 20, roadStraight, RoadOrientation.LeftToRight);
        PlaceRoad(3, 20, roadStraight, RoadOrientation.LeftToRight);
        PlaceRoad(4, 20, roadTshape, RoadOrientation.TopFacing);
        PlaceRoad(5, 20, roadStraight, RoadOrientation.LeftToRight);
        PlaceRoad(6, 20, roadStraight, RoadOrientation.LeftToRight);
        PlaceRoad(7, 20, roadStraight, RoadOrientation.LeftToRight);
        PlaceRoad(8, 20, roadStraight, RoadOrientation.LeftToRight);
        PlaceRoad(9, 20, roadStraight, RoadOrientation.LeftToRight);
        PlaceRoad(10, 20, roadTshape, RoadOrientation.TopFacing);
        PlaceRoad(11, 20, roadStraight, RoadOrientation.LeftToRight);
        PlaceRoad(12, 20, roadStraight, RoadOrientation.LeftToRight);
        PlaceRoad(13, 20, roadStraight, RoadOrientation.LeftToRight);
        PlaceRoad(14, 20, roadStraight, RoadOrientation.LeftToRight);
        PlaceRoad(15, 20, roadStraight, RoadOrientation.LeftToRight);
        PlaceRoad(16, 20, roadTshape, RoadOrientation.TopFacing);
        PlaceRoad(17, 20, roadStraight, RoadOrientation.LeftToRight);
        PlaceRoad(18, 20, roadStraight, RoadOrientation.LeftToRight);
        PlaceRoad(19, 20, roadStraight, RoadOrientation.LeftToRight);
        PlaceRoad(20, 20, roadCorner, RoadOrientation.LowerRightCorner);

        // 1 Row Buildings
        PlaceBuilding(1, 1);
        PlaceBuilding(2, 1);
        PlaceBuilding(5, 1);
        PlaceBuilding(6, 1);
        PlaceBuilding(9, 1);
        PlaceBuilding(11, 1);
        PlaceBuilding(14, 1);
        PlaceBuilding(15, 1);
        PlaceBuilding(18, 1);
        PlaceBuilding(19, 1);

        // 2 Row Buildings
        PlaceBuilding(1, 2);
        PlaceBuilding(19, 2);

        // 3 Row Buildings
        PlaceBuilding(3, 3);
        PlaceBuilding(17, 3);

        // 4 Row Buildings
        PlaceBuilding(2, 4);
        PlaceBuilding(3, 4);
        PlaceBuilding(6, 4);
        PlaceBuilding(9, 4);
        PlaceBuilding(11, 4);
        PlaceBuilding(14, 4);
        PlaceBuilding(17, 4);
        PlaceBuilding(18, 4);

        // 6 Row Buildings
        PlaceBuilding(4, 6);
        PlaceBuilding(8, 6);
        PlaceBuilding(12, 6);
        PlaceBuilding(16, 6);

        // 7 Row Buildings
        PlaceBuilding(1, 7);
        PlaceBuilding(7, 7);
        PlaceBuilding(13, 7);
        PlaceBuilding(19, 7);

        // 8 Row Buildings
        PlaceBuilding(5, 8);
        PlaceBuilding(15, 8);

        // 9 Row Buildings
        PlaceBuilding(3, 9);
        PlaceBuilding(17, 9);

        // 11 Row Buildings;
        PlaceBuilding(3, 11);
        PlaceBuilding(17, 11);

        // 12 Row Buildings
        PlaceBuilding(5, 12);
        PlaceBuilding(15, 12);

        // 13 Row Buildings
        PlaceBuilding(1, 13);
        PlaceBuilding(7, 13);
        PlaceBuilding(13, 13);
        PlaceBuilding(19, 13);

        // 14 Row Buildings
        PlaceBuilding(4, 14);
        PlaceBuilding(8, 14);
        PlaceBuilding(12, 14);
        PlaceBuilding(16, 14);

        // 16 Row Buildings
        PlaceBuilding(2, 16);
        PlaceBuilding(3, 16);
        PlaceBuilding(6, 16);
        PlaceBuilding(9, 16);
        PlaceBuilding(11, 16);
        PlaceBuilding(14, 16);
        PlaceBuilding(17, 16);
        PlaceBuilding(18, 16);

        // 17 Row Buildings
        PlaceBuilding(3, 17);
        PlaceBuilding(17, 17);

        // 18 Row Buildings
        PlaceBuilding(1, 18);
        PlaceBuilding(19, 18);

        // 19 Row Buildings
        PlaceBuilding(1, 19);
        PlaceBuilding(2, 19);
        PlaceBuilding(5, 19);
        PlaceBuilding(6, 19);
        PlaceBuilding(9, 19);
        PlaceBuilding(11, 19);
        PlaceBuilding(14, 19);
        PlaceBuilding(15, 19);
        PlaceBuilding(18, 19);
        PlaceBuilding(19, 19);

        for (int x = 0; x < Length; x++)
            for (int y = 0; y < Width; y++)
                if (GameMap[x, y] == null)
                    PlacePark(x, y);
    }

    /*  This will place a building on GameMap[x,y]
     *  Aditionally, the building will be allocated a road to dump trash on.
     *  If not placed next to a road, it will not be placed.
     */
    void GenerateBorders()
    {
        for (int i = -1; i < 22; i++)
        {
            Instantiate(InvisableWall, new Vector3(i, 0, -1), Quaternion.identity);
            Instantiate(InvisableWall, new Vector3(i, 0, 21), Quaternion.identity);
        }
        for (int j = -1; j < 22; j++)
        {
            Instantiate(InvisableWall, new Vector3(-1, 0, j), Quaternion.identity);
            Instantiate(InvisableWall, new Vector3(21, 0, j), Quaternion.identity);
        }
    }

    void PlaceBuilding(int x, int y)
    {
        Road r = FindAdjacentRoad(x, y);
        if (r != null)
        {
            if (housePrefabs.Count > 0)
            {
                float result = Random.Range(0, housePrefabs.Count);
                MapItem tmp = Instantiate(housePrefabs[Mathf.RoundToInt(result)], new Vector3(x, 0, y), Quaternion.identity);
                tmp.GetComponent<Building>().SetDumpLocation(r);
                GameMap[x, y] = tmp;
            }
        }
    }

    void PlacePark(int x, int y)
    {
        int nxtRotation = Random.Range(1, 4);
        int angle = 0;
        switch (nxtRotation)
        {
            case 1:
                angle = 0;
                break;
            case 2:
                angle = 180;
                break;
            case 3:
                angle = 270;
                break;
            case 4:
                angle = 360;
                break;
            default:
                break;
        }
        MapItem tmp = Instantiate(parkPrefab, new Vector3(x, 0, y), Quaternion.identity);
        tmp.transform.Rotate(new Vector3(0, angle, 0));
        GameMap[x, y] = tmp;
    }

    /*  This will find a road adjacent ( or right on, dont @ me )
     *  to the GameItem at x, y.
     *  returns 0 when no road could be found. 
     */
    Road FindAdjacentRoad(int x, int y)
    {
        for (int i = x - 1; i <= x + 1; i++)
        {
            for (int j = y - 1; j <= y + 1; j++)
            {
                if (i >= 0 && i < Length && j >= 0 && j < Width)
                {
                    if (GameMap[i, j] != null)
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

    void PlaceRoad(int x, int y, MapItem roadType, RoadOrientation orientation)
    {
        //, new Quaternion(0, rotation, 0, 0)
        MapItem tmp = Instantiate(roadType, new Vector3(x, 0, y), Quaternion.identity);
        tmp.transform.Rotate(new Vector3(0, (int)orientation, 0));
        tmp.GetComponent<Road>().trashPrefabs = ThrashPrefabs;
        GameMap[x, y] = tmp;
    }

    void PlaceRoad(int x, int y)
    {
        MapItem tmp = Instantiate(roadStraight, new Vector3(x, 0, y), Quaternion.identity);
        tmp.GetComponent<Road>().trashPrefabs = ThrashPrefabs;
        GameMap[x, y] = tmp;
    }

    void PlaceTruck(int x, int y)
    {
        Instantiate(GarbageTruck, new Vector3(x, 0, y), Quaternion.identity);
    }

    public void AddScore(int amount)
    {
        Score += amount;
        pointstext.text = "Points: " + Score.ToString();
    }

    public void RemoveScore()
    {
        Score -= 100;
        pointstext.text = "Points: " + Score.ToString();
    }
}
