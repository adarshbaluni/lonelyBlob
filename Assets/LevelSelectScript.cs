using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LevelSelectScript : MonoBehaviour {
	
	//private int worldIndex;   
	private int levelIndex;   
	//variable which holds the stars obtained   
	private int stars=0;
	
	void  Start (){
		//loop thorugh all the worlds
		//for(int i = 1; i <= LockLevel.worlds; i++){
			//if(Application.loadedLevelName == "World"+i){
				//worldIndex = i;
				CheckLockedLevels(); 
			}
		//}
	//}
	
	//Level to load on button click. Will be used for Level button click event 
	public void Selectlevel(string worldLevel){
		Application.LoadLevel("Level"+worldLevel); //load the level
	}
	
	//uncomment the below code if you have a main menu scene to navigate to it on clicking escape when in World1 scene
	/*public void  Update (){
  if (Input.GetKeyDown(KeyCode.Escape) ){
   Application.LoadLevel("MainMenu");
  }   
 }*/
	
	//function to check for the levels locked
	void  CheckLockedLevels (){
		//loop through the levels of a particular world
		for(int j = 1; j <= LockLevel.levels; j++){
			//get the number of stars obtained for that particular level
			//used to enable the image which should be displayed in the World1 scene beside the individual levels
			stars = PlayerPrefs.GetInt("level"+j.ToString()+"stars");
			levelIndex = (j+1);
			//enable the respective image based on the stars variable value
			GameObject.Find(j+"star"+stars).GetComponent<Image>().enabled = true;
			Debug.Log(j+"star"+stars);
			if((PlayerPrefs.GetInt("level"+levelIndex.ToString()))==1){
				GameObject.Find("LockedLevel"+(j+1)).active = false;
				Debug.Log ("Unlocked");
			}
		}
	}
}
