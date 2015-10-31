using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

	public GameObject mainMenu;
	public GameObject controls;
	public GameObject levelSelect;
	public GameObject controlButton;
	private Animator animPlay;
	private Animator animControl;
	private Animator animQuit;

	void start(){
	
		controls.gameObject.SetActive (false);
		levelSelect.gameObject.SetActive (false);
		animPlay = controlButton.GetComponent<Animator> ();
		animPlay.enabled=true;
		animPlay.Play("ControlButtonAnimation");
	
	}


	public void StartGame(){
		levelSelect.gameObject.SetActive (false);
		LoadingScreen.show ();
		Application.LoadLevel ("LonelyBlob");
		
	}

	public void Back(){
	      
		levelSelect.gameObject.SetActive (false);
		controls.gameObject.SetActive (false);
		mainMenu.gameObject.SetActive (true);

	
	}

	public void LevelSelect(){
		
		mainMenu.gameObject.SetActive (false);
		levelSelect.gameObject.SetActive (true);
	}


	public void Controls(){

		mainMenu.gameObject.SetActive (false);
		controls.gameObject.SetActive (true);
	}
	
	public void Quit(){
		
		Application.Quit();
		
	}

}
