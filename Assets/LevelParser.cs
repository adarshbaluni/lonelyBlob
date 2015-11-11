using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;

public enum MapElementType
{
    BLOCK = 1, 
    PLATFORM = 2, 
    BAT = 3, 
    EMPTY = 4, 
    CHARACTER = 5, 
    BLOB_REGEN = 6, 
    GOAL = 7, 
    ANGRY_POWERUP = 8, 
    BREAKABLE_WALL = 10, 
    MOVING_PLATF = 11, 
    ARROW_TRAP = 9, 
    SPIKES_NORTH = 12, 
    SPIKES_WEST = 13, 
    SPIKES_EAST = 14, 
    SPIKES_SOUTH = 15, 
    PLAYER_DETECTOR = 16
};

public struct MapElement
{
    public GameObject prefab;
    public MapElementType type;
    public int tileWidth; //amount of tiles that this object occupies in X direction
    public int tileHeight;
    public Quaternion rotation;
    public Vector2 customOffset;

    public MapElement(GameObject Prefab, MapElementType Type, Quaternion rot, Vector2 CustomOffset)
    { 
        prefab = Prefab;
        type = Type;
        tileWidth = -1; //invalid value by default
        tileHeight = -1;
        rotation = rot;
        customOffset = CustomOffset;
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
            case "BAT":
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
            case "ANGRY_POWERUP":
                return MapElementType.ANGRY_POWERUP;
            default:
                Debug.Assert(false, "Unknown map element "+typeString);
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
    public GameObject pfAngry;
    
    static public int tilesX, tilesY;
    static public float tileSize;
  
    public Dictionary<MapElementType, MapElement> mapElemDict;
    public Dictionary<int, MapElementType> symbolToElemTypeDict; //map file defines which symbols correspond to which map elements
    static public Vector2 MapBounds;


    void Start()
    {

		//string level = MainMenu.levels.ToString ();
        mapElemDict = new Dictionary<MapElementType, MapElement>();
        symbolToElemTypeDict = new Dictionary<int, MapElementType>();

        mapElemDict[MapElementType.BLOCK] = new MapElement(pfBlock, MapElementType.BLOCK, Quaternion.identity,Vector2.zero);
        mapElemDict[MapElementType.BAT] = new MapElement(pfBat, MapElementType.BAT, Quaternion.identity, Vector2.zero);
        mapElemDict[MapElementType.PLATFORM] = new MapElement(pfPlatform, MapElementType.PLATFORM, Quaternion.identity, Vector2.zero);
        mapElemDict[MapElementType.CHARACTER] = new MapElement(pfCharacter, MapElementType.CHARACTER, Quaternion.identity, Vector2.zero);
        mapElemDict[MapElementType.GOAL] = new MapElement(pfGoal, MapElementType.GOAL, Quaternion.identity, Vector2.zero);
        mapElemDict[MapElementType.ANGRY_POWERUP] = new MapElement(pfAngry, MapElementType.ANGRY_POWERUP, Quaternion.identity, Vector2.zero);
        mapElemDict[MapElementType.SPIKES_SOUTH] = new MapElement(pfSpikes, MapElementType.SPIKES_SOUTH, Quaternion.Euler(0, 0, 180), new Vector2(0, 5));
        mapElemDict[MapElementType.SPIKES_NORTH] = new MapElement(pfSpikes, MapElementType.SPIKES_NORTH, Quaternion.identity, new Vector2(0,-5));
        mapElemDict[MapElementType.SPIKES_WEST] = new MapElement(pfSpikes, MapElementType.SPIKES_WEST, Quaternion.Euler(0, 0, -90), new Vector2(-5, 0));
        mapElemDict[MapElementType.SPIKES_EAST] = new MapElement(pfSpikes, MapElementType.SPIKES_EAST, Quaternion.Euler(0, 0, 90), new Vector2(5, 0));
        mapElemDict[MapElementType.BLOB_REGEN] = new MapElement(pfBlobRegen, MapElementType.BLOB_REGEN, Quaternion.identity, Vector2.zero);
        mapElemDict[MapElementType.BREAKABLE_WALL] = new MapElement(pfBreakableWall, MapElementType.BREAKABLE_WALL, Quaternion.identity, Vector2.zero);
        mapElemDict[MapElementType.ARROW_TRAP] = new MapElement(pfArrowTrap, MapElementType.ARROW_TRAP, Quaternion.identity, Vector2.zero);
        mapElemDict[MapElementType.MOVING_PLATF] = new MapElement(pfMovingPlatform, MapElementType.MOVING_PLATF, Quaternion.identity, Vector2.zero);
        mapElemDict[MapElementType.PLAYER_DETECTOR] = new MapElement(pfPlayerDetector, MapElementType.PLAYER_DETECTOR, Quaternion.identity, Vector2.zero);

        //TODO: Add prefabs for other elements

        //MapElementType[,] map = LoadMap(Application.dataPath + "/testLevel.csv");
        //#if defined(UNITY_EDITOR)
        //string path;
        //if (Application.platform == RuntimePlatform.WindowsEditor)
        //{
        //    path = Application.dataPath + "/StreamingAssets";
        //}
        //else if(Application.platform == RuntimePlatform.Android)
        //{
        //    path = "jar:file://" + Application.dataPath + "!/assets";

        //}


        //StartCoroutine(crLoadMap("/Level_5_Test.csv"));
        StartCoroutine(crLoadMap("/Edgar_Level_1.csv"));

        //#endif

       
    }

    IEnumerator crLoadMap(string filename)
    {
        string filePath = Application.streamingAssetsPath;
        
#if UNITY_ANDROID && !UNITY_EDITOR
        if (filePath.Contains("://"))
        {
            WWW www = new WWW(filePath);
            yield return www;
            filePath = www.text;
        }
        else
            filePath = System.IO.File.ReadAllText(filePath);
#endif 
        yield return null;
        //MapElementType[,] map = LoadMap(path+"/"+level+".csv");
        MapElementType[,] map = LoadMap(filePath+filename);

        //MapElementType[,] map = LoadMap("Assets/Levels/Level_5_Test.csv");
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
                        CreateMapObject(map, i, j, width, height, mapElem.prefab, mapElem.rotation,mapElem.customOffset);
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

    void CreateMapObject(MapElementType[,] map, uint row, uint col, uint width, uint height, GameObject prefab, Quaternion rotation, Vector3 CustomOffset)
    {
        Vector2 objSize = new Vector2(width * tileSize, height * tileSize); //target size for the object
        //Quaternion.FromToRotation(Vector3.right,)
        //Quaternion.EulerRotation();

        GameObject mapObj = (GameObject)Instantiate(prefab, new Vector3(-col * tileSize - objSize.x / 2.0f + CustomOffset.x, row * tileSize + objSize.y / 2.0f + CustomOffset.y, 0), rotation);
        Renderer rend = mapObj.GetComponent<Renderer>();

        /*
        //Make a standard size for the object to match with the map tiling
        Vector3 normalizedScale = mapObj.transform.localScale;
        normalizedScale.x *= objSize.x / rend.bounds.size.x;
        normalizedScale.y *= objSize.y / rend.bounds.size.y;
        mapObj.transform.localScale = normalizedScale;
        */

        //Make a standard size for the object to match with the map tiling but preserve its aspect ratio
        Vector3 normalizedScale = mapObj.transform.localScale;

        float ratio = (rend.bounds.size.y > rend.bounds.size.x) ? objSize.y / rend.bounds.size.y : objSize.x / rend.bounds.size.x;
        normalizedScale.x *= ratio; //Increased scale a little bit to avoid gaps between tiles
        normalizedScale.y *= ratio;

        //rend.bounds.extents.Set(rend.bounds.extents.x * 2.0f, rend.bounds.extents.y * 2.0f, 0);

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
                //Debug.Log("X: " + coordX + "Y: " + coordY);
                Debug.Assert(!symbol.Equals(""), "Found an empty space in map file, replace it with a valid symbol !");
                    
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

            string symbolName = tokens[0];
            if (symbolName.Equals("END"))
            {
                break;
            }

            int symbol = int.Parse(tokens[1]);
            
            
            MapElementType symbolType = MapElement.ParseSymbolType(symbolName);
            symbolToElemTypeDict[symbol] = symbolType;  //need this to be able to create map elements identified by their symbols in the map file
            
            if(symbolName.Equals("EMPTY"))
            {
                continue;
            }

            MapElement mapElem;
            if(!mapElemDict.TryGetValue(symbolType, out mapElem))
            {
                Debug.Assert(false, "Couldnt find symbol in dictionary !");
            }
            mapElem.tileWidth = int.Parse(tokens[2]);
            mapElem.tileHeight = int.Parse(tokens[3]);

            mapElemDict[symbolType] = mapElem;
        }
    }
}
