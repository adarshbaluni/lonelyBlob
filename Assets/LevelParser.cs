using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;

public enum MapElementType { BLOCK = 1, PLATFORM, BAT, EMPTY, CHARACTER, GOAL, SPIKES_SOUTH, SPIKES_NORTH, SPIKES_WEST, SPIKES_EAST, BLOB_REGEN,
	BREAKABLE_WALL,ARROW_TRAP,MOVING_PLATF,PLAYER_DETECTOR};

public struct MapElement
{
    public GameObject prefab;
    public MapElementType type;
    public int tileWidth; //amount of tiles that this object occupies in X direction
    public int tileHeight;

    public MapElement(GameObject Prefab, MapElementType Type)
    { 
        prefab = Prefab;
        type = Type;
        tileWidth = -1; //invalid value by default
        tileHeight = -1;
    }

    public static MapElementType ParseSymbolType(string typeString)
    {
        switch(typeString)
        {
            case "EMPTY":
                return MapElementType.EMPTY;
            case "BLOCK":
                return MapElementType.BLOCK;
            case "PLATFORM":
                return MapElementType.PLATFORM;
            case "ENEMY":
				return MapElementType.BAT;
            case "CHARACTER":
                return MapElementType.CHARACTER;
            case "GOAL":
                return MapElementType.GOAL;
            case "SPIKES_SOUTH":
                return MapElementType.SPIKES_SOUTH;
            case "SPIKES_NORTH":
                return MapElementType.SPIKES_NORTH;
            case "SPIKES_WEST":
                return MapElementType.SPIKES_WEST;
            case "SPIKES_EAST":
                return MapElementType.SPIKES_EAST;
            case "BLOB_REGEN":
                return MapElementType.BLOB_REGEN;
			case "BREAKABLE_WALL":
				return MapElementType.BREAKABLE_WALL;
			case "ARROW_TRAP":
				return MapElementType.ARROW_TRAP;
			case "MOVING_PLATF":
				return MapElementType.MOVING_PLATF;
			case "PLAYER_DETECTOR":
				return MapElementType.PLAYER_DETECTOR;
            default:
                Debug.Assert(false, "Unknown map element");
                break;
        }
        return MapElementType.EMPTY; //this should never happen
    }
}

public class LevelParser : MonoBehaviour {
    

    public GameObject pfBlock;
    public GameObject pfPlatform;
    public GameObject pfBat;
    public GameObject pfCharacter;
    public GameObject pfGoal;
    public GameObject pfSpikes;
    public GameObject pfBlobRegen;
	public GameObject pfBreakableWall;
	public GameObject pfArrowTrap;
	public GameObject pfMovingPlatform;
	public GameObject pfPlayerDetector;
    
    static public int tilesX, tilesY;
    static public float tileSize;
  
    public Dictionary<MapElementType, MapElement> mapElemDict;
    public Dictionary<int, MapElementType> symbolToElemTypeDict; //map file defines which symbols correspond to which map elements
    static public Vector2 MapBounds; 



    void Start()
    {
        mapElemDict = new Dictionary<MapElementType, MapElement>();
        symbolToElemTypeDict = new Dictionary<int, MapElementType>();

        mapElemDict[MapElementType.BLOCK] = new MapElement(pfBlock, MapElementType.BLOCK);
		mapElemDict[MapElementType.BAT] = new MapElement(pfBat, MapElementType.BAT);
        mapElemDict[MapElementType.PLATFORM] = new MapElement(pfPlatform, MapElementType.PLATFORM);
        mapElemDict[MapElementType.CHARACTER] = new MapElement(pfCharacter, MapElementType.CHARACTER);
        mapElemDict[MapElementType.GOAL] = new MapElement(pfGoal, MapElementType.GOAL);
        mapElemDict[MapElementType.SPIKES_SOUTH] = new MapElement(pfSpikes, MapElementType.SPIKES_SOUTH);
        mapElemDict[MapElementType.SPIKES_NORTH] = new MapElement(pfSpikes, MapElementType.SPIKES_NORTH);
        mapElemDict[MapElementType.SPIKES_WEST] = new MapElement(pfSpikes, MapElementType.SPIKES_WEST);
        mapElemDict[MapElementType.SPIKES_EAST] = new MapElement(pfSpikes, MapElementType.SPIKES_EAST);
        mapElemDict[MapElementType.BLOB_REGEN] = new MapElement(pfBlobRegen, MapElementType.BLOB_REGEN);

		mapElemDict[MapElementType.BREAKABLE_WALL] = new MapElement(pfBreakableWall, MapElementType.BREAKABLE_WALL);
		mapElemDict[MapElementType.ARROW_TRAP] = new MapElement(pfArrowTrap, MapElementType.ARROW_TRAP);
		mapElemDict[MapElementType.MOVING_PLATF] = new MapElement(pfMovingPlatform, MapElementType.MOVING_PLATF);
		mapElemDict[MapElementType.PLAYER_DETECTOR] = new MapElement(pfPlayerDetector, MapElementType.PLAYER_DETECTOR);

        //TODO: Add prefabs for other elements

        //MapElementType[,] map = LoadMap(Application.dataPath + "/testLevel.csv");
        MapElementType[,] map = LoadMap("Assets/Levels/testLevel.csv");
        BuildMap(map);
    }

    void BuildMap(MapElementType[,] map)
    {
        uint width = 0;
        uint height = 0;
        for (uint i = 0; i < tilesY; ++i)
        {
            for (uint j = 0; j < tilesX; ++j)
            {
                if(map[i,j] != MapElementType.EMPTY)
                {
                    width = (uint)mapElemDict[map[i, j]].tileWidth;
                    height = (uint)mapElemDict[map[i, j]].tileHeight;
                    if(IsCompleteObject(map,i,j,width,height))
                    {
                        MapElement mapElem = mapElemDict[map[i, j]];
                        CreateMapObject(map, i, j, width, height, mapElem.prefab);
                    }
                }
            }
        }
    }

    bool IsCompleteObject(MapElementType[,] map, uint row,uint col,uint width, uint height)
    {
        if(row + height > tilesY || col + width > tilesX) 
            return false; //out of map bounds, so object cannot be completed

        for (uint i = row; i < row + height; ++i)
        {
            for (uint j = col; j < col + width; ++j)
            {
                if(map[i,j] != map[row,col])
                {
                    return false; //found a tile that is not part of the object
                }
            }
        }
        return true;
    }

    void CreateMapObject(MapElementType[,] map, uint row, uint col, uint width, uint height, GameObject prefab)
    {
        Vector2 objSize = new Vector2(width * tileSize, height * tileSize); //target size for the object
        GameObject mapObj = (GameObject)Instantiate(prefab, new Vector3(-col * tileSize - objSize.x/2.0f, row * tileSize + objSize.y/2.0f, 0), Quaternion.identity);
        Renderer rend = mapObj.GetComponent<Renderer>();

        //Make a standard size for the object to match with the map tiling
        Vector3 normalizedScale = mapObj.transform.localScale;
        normalizedScale.x *= objSize.x / rend.bounds.size.x;
        normalizedScale.y *= objSize.y / rend.bounds.size.y;
        mapObj.transform.localScale = normalizedScale;

        for(uint i=row; i < row + height;++i)
        {
            for(uint j=col; j < col + width;++j)
            {
                map[i,j] = MapElementType.EMPTY; //We have already created this element so remove from map objects
            }
        }
    }

    MapElementType[,] LoadMap(string filename)
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
            if (tokens[0].Equals("Tiles X"))
            {
                tilesX = int.Parse(tokens[1]);
            }
            else if (tokens[0].Equals("Tiles Y"))
            {
                tilesY = int.Parse(tokens[1]);
            }
            else if (tokens[0].Equals("Tile size"))
            {
                tileSize = float.Parse(tokens[1]);
            }
            else if (tokens[0].Equals("Map Definitions"))
            {
                ParseMapElementDefs(fileReader);
            }
            else if (tokens[0].Equals("Map Data"))
            {
                break; //we begin map data section so stop reading parameters
            }
        }

        Debug.Assert(tilesX > 0 && tilesY > 0 && tileSize > 0); //check that map parameters have been properly set

        //Read load map as a matrix
        MapElementType[,] map = new MapElementType[tilesY, tilesX];
        int coordX = 0; //start on top left corner of map 
        int coordY = tilesY - 1;
        //tileSize = 13.8f;
        //tileSize = 28;
        MapBounds = new Vector2(-tilesX * tileSize, tilesY * tileSize);

        while (coordY >= 0)
        {
            line = fileReader.ReadLine();
            string[] symbols = line.Split(',');
            foreach (var symbol in symbols)
            {
                Debug.Log("X: " + coordX + "Y: " + coordY);
                map[coordY, coordX] = symbolToElemTypeDict[int.Parse(symbol)];
                ++coordX;
            }
            coordX = 0; //Row finished
            --coordY;
        }

        return map;
    }

    void ParseMapElementDefs(StreamReader file)
    {
        string line;
        while(true)
        {
            line = file.ReadLine();
            string[] tokens = line.Split(',');

            int symbol = int.Parse(tokens[1]);
            string symbolName = tokens[0];
            
            MapElementType symbolType = MapElement.ParseSymbolType(symbolName);
            symbolToElemTypeDict[symbol] = symbolType;  //need this to be able to create map elements identified by their symbols in the map file
            
            if (symbolName.Equals("EMPTY")) //assume empty is always the last element definition and it does not have width nor height
            {
                break;
            }

            MapElement mapElem;
            mapElemDict.TryGetValue(symbolType, out mapElem);
            mapElem.tileWidth = int.Parse(tokens[2]);
            mapElem.tileHeight = int.Parse(tokens[3]);
            mapElemDict[symbolType] = mapElem;
        }
    }
}
