using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;

public class LevelParser : MonoBehaviour {
    public enum MapElement { BLOCK = 1, PLATFORM, ENEMY, EMPTY };

    public GameObject pfBlock;
    public GameObject pfPlatform;
    public GameObject pfEnemy;
    public int tilesX, tilesY;
    public float tileSize;
    public Dictionary<MapElement, GameObject> mapElemPrefabDict;
    

    /*
	// Use this for initialization
	void Start () {
	    var fileReader = new StreamReader(File.OpenRead("Assets/Levels/testLevel.csv"));
        string line;
        int tilesX = 0, tilesY = 0;
        float tileSize = 0;
        while(true)
        {
            line = fileReader.ReadLine();
            print("hi");
            string[] tokens = line.Split(',');
            if (tokens[0].Equals("Tiles_X"))
            {
                tilesX = int.Parse(tokens[1]);
            }
            else if(tokens[0].Equals("Tiles_Y"))
            {
                tilesY = int.Parse(tokens[1]);
            }
            else if (tokens[0].Equals("Tile size"))
            {
                tileSize = float.Parse(tokens[1]);
            }
            else if(tokens[0].Equals("MapData"))
            {
                break; //we begin map data section so stop reading parameters
            }
            print("ho");
        }

        MapElement mapElem;
        int coordX = 0; //start on top left corner of map 
        int coordY = tilesY - 1;
        tileSize = 13.8f;
        while(coordY >= 0)
        {
            line = fileReader.ReadLine();
            string[] tokens = line.Split(',');
            foreach(var token in tokens)
            {
                mapElem = (MapElement)int.Parse(token);
                switch(mapElem)
                {
                    case MapElement.BLOCK:
                        Instantiate(pfBlock, new Vector3(-coordX * tileSize, coordY * tileSize, 0), Quaternion.identity); //x axis is inverted, so mirror the coordinate
                        break;
                    case MapElement.PLATFORM:
                        Instantiate(pfPlatform, new Vector3(-coordX * tileSize, coordY * tileSize, 0), Quaternion.identity);
                        break;
                    case MapElement.ENEMY:
                        Instantiate(pfEnemy, new Vector3(-coordX * tileSize, coordY * tileSize, 0), Quaternion.identity);
                        break;
                    case MapElement.EMPTY:
                        //Dont do nothing since this is an empty spot
                        break;

                }
                ++coordX;
            }
            coordX = 0; //Row finished
            --coordY; 
        }

        

	}
     * */

    void Start()
    {
        mapElemPrefabDict[MapElement.BLOCK] = pfBlock;
        mapElemPrefabDict[MapElement.ENEMY] = pfEnemy;
        mapElemPrefabDict[MapElement.PLATFORM] = pfPlatform;
        MapElement[,] map = LoadMap("Assets/Levels/testLevel.csv");
        BuildMap(map);
    }

    void BuildMap(MapElement[,] map)
    {
        //TODO: Make this values parameters of the CSV map file
        uint blockWidth = 2;
        uint blockHeight = 2;
        uint enemyWidth = 1;
        uint enemyHeight = 1;
        uint platformWidth = 1;
        uint platformHeight = 1;

        uint width = 0;
        uint height = 0;
        for (uint i = 0; i < tilesX; ++i)
        {
            for (uint j = 0; j < tilesY; ++j)
            {
                if(map[i,j] != MapElement.EMPTY)
                {
                    switch(map[i,j])
                    {
                        case MapElement.BLOCK:
                            width = blockWidth;
                            height = blockHeight;
                            break;
                        case MapElement.ENEMY:
                            width = enemyWidth;
                            height = enemyHeight;
                            break;
                        case MapElement.PLATFORM:
                            width = platformWidth;
                            height = platformHeight;
                            break;
                        default:
                            print("Error: Unknown map object on parsing");
                            break;
                    }
                    if(IsCompleteObject(map,i,j,width,height))
                    {
                        CreateMapObject(map, i, j, width, height,mapElemPrefabDict[map[i,j]]);
                    }
                }
            }
        }
    }

    bool IsCompleteObject(MapElement[,] map, uint row,uint col,uint width, uint height)
    {
        if(row + height >= tilesY || col + width >= tilesX) 
            return false; //out of map bounds, so object cannot be completed

        for (uint i = row; i < row + height; ++i)
        {
            for (uint j = col+1; i < col + width; ++j) //first element is checked by default so skip it
            {
                if(map[i,j] != map[row,col])
                {
                    return false; //found a tile that is not part of the object
                }
            }
        }
        return true;
    }

    void CreateMapObject(MapElement[,] map, uint row, uint col, uint width, uint height, GameObject prefab)
    {
        Instantiate(prefab, new Vector3(row * tileSize, col * tileSize, 0), Quaternion.identity);
        for(uint i=row; i < row + height;++i)
        {
            for(uint j=col; i < col + width;++j)
            {
                map[i,j] = MapElement.EMPTY; //We have already created this element so remove from map objects
            }
        }
    }

    MapElement[,] LoadMap(string filename)
    {
        var fileReader = new StreamReader(File.OpenRead(filename));
        string line;
        tilesX = 0;
        tilesY = 0;
        tileSize = 0;
        while (true)
        {
            line = fileReader.ReadLine();
            string[] tokens = line.Split(',');
            if (tokens[0].Equals("Tiles_X"))
            {
                tilesX = int.Parse(tokens[1]);
            }
            else if (tokens[0].Equals("Tiles_Y"))
            {
                tilesY = int.Parse(tokens[1]);
            }
            else if (tokens[0].Equals("Tile size"))
            {
                tileSize = float.Parse(tokens[1]);
            }
            else if (tokens[0].Equals("MapData"))
            {
                break; //we begin map data section so stop reading parameters
            }
        }

        //Read load map as a matrix
        MapElement[,] map = new MapElement[tilesX, tilesY];
        int coordX = 0; //start on top left corner of map 
        int coordY = tilesY - 1;
        tileSize = 13.8f;
        while (coordY >= 0)
        {
            line = fileReader.ReadLine();
            string[] tokens = line.Split(',');
            foreach (var token in tokens)
            {
                map[coordX, coordY] = (MapElement)int.Parse(token);
                ++coordX;
            }
            coordX = 0; //Row finished
            --coordY;
        }

        return map;
    }
}
