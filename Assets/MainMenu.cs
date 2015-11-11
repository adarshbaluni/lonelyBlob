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
	public static int levels = 0;

	void start(){
	
		controls.gameObject.SetActive (false);
		levelSelect.gameObject.SetActive (false);
		animPlay = controlButton.GetComponent<Animator> ();
		animPlay.enabled=true;
		animPlay.Play("ControlButtonAnimation");
	
	}


	public void StartGame(int a){
		levelSelect.gameObject.SetActive (false);
		//levels = a;
		LoadingScreen.show ();
		Application.LoadLevel ("Level"+a.ToString());
		
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
