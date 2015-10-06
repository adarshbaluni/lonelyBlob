using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

	public GameObject mainMenu;

	public void StartGame(){
		
		Application.LoadLevel ("LonelyBlob");
		
	}
	
	public void Quit(){
		
		Application.Quit();
		
	}

}
