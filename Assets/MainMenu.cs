using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

	public GameObject mainMenu;
	public GameObject controls;


	void start(){
	
		controls.gameObject.SetActive (false);
	
	
	}


	public void StartGame(){
		mainMenu.gameObject.SetActive (false);
		LoadingScreen.show ();
		Application.LoadLevel ("LonelyBlob");
		
	}

	public void Back(){
	       
		controls.gameObject.SetActive (false);
		mainMenu.gameObject.SetActive (true);

	
	}

	public void Controls(){

		mainMenu.gameObject.SetActive (false);
		controls.gameObject.SetActive (true);
	}
	
	public void Quit(){
		
		Application.Quit();
		
	}

}
