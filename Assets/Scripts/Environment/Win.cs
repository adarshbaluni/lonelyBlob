using UnityEngine;
using System.Collections;

public class Win : MonoBehaviour {
	public Transform creditsPose;
	public GameObject winPanel;
	public static bool isWon;
	protected string currentLevel;
	protected int levelIndex;
	// Use this for initialization
	void Start () {
		isWon = false;
        winPanel = UIManager.s_winPanel;
		currentLevel = Application.loadedLevelName;
	}
	
	// Update is called once per frame
	void Update () {
		PauseGame (isWon);
	}

	void PauseGame(bool state){
		winPanel.SetActive (state);
	}

	void OnTriggerEnter2D(Collider2D col){
		Debug.Log ("Entered Trigger");
		if (col.gameObject.tag == "Player") {
			isWon=true;
			if(CharacterControl.totalTime < 20){
			UnlockLevels(3);
			}
			else if(CharacterControl.totalTime < 25){
				UnlockLevels(2);
			}
			else if(CharacterControl.totalTime < 35){

				UnlockLevels(1);
			}
			CharacterControl.totalTime = 0f;
		}
	}

	protected void  UnlockLevels (int stars){
		//set the playerprefs value of next level to 1 to unlock
		//for(int i = 0; i < LockLevel.worlds; i++){
			for(int j = 1; j <= LockLevel.levels; j++){               
				if(currentLevel == "Level"+j.ToString()){
					//worldIndex  = (i+1);
					levelIndex  = (j+1);
					PlayerPrefs.SetInt("level"+levelIndex.ToString(),1);
				//check if the current stars value is less than the new value
				if(PlayerPrefs.GetInt("level"+j.ToString()+"stars")< stars)
					//overwrite the stars value with the new value obtained
					PlayerPrefs.SetInt("level"+j.ToString()+"stars",stars);
				}
			}
		//}
		//load the World1 level 
		//Application.LoadLevel("World1");
	}


}
