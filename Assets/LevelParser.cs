using UnityEngine;
using System.Collections;
using System.IO;

public class LevelParser : MonoBehaviour {

	// Use this for initialization
	void Start () {
	    var fileReader = new StreamReader(File.OpenRead("Levels/testLevel.csv"));
        string line = fileReader.ReadLine();
        int tilesX, tilesY;
        while(true)
        {
            string[] tokens = line.Split(',');
            if (tokens[0].Equals("Tiles_X"))
            {
                tilesX = int.Parse(tokens[1]);
            }
            else if(tokens[0].Equals("Tiles_Y"))
            {
                tilesY = int.Parse(tokens[1]);
            }
            else if(tokens[0].Equals("MapData"))
            {
                break; //we begin map data section so stop reading parameters
            }
        }
            

        

	}
}
