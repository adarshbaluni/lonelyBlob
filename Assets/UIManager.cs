using UnityEngine;
using UnityEngine.UI; 
using System.Collections;

public class UIManager : MonoBehaviour
{
	public GameObject pausePanel;
	public bool isPaused;
	public AudioSource jumpMusic;
	// Use this for initialization
	void Start ()
	{
		isPaused = false;
	}
	// Update is called once per frame
	void Update ()
	{
		if (isPaused) {
			PauseGame (true);
		}
		else {
			PauseGame (false);
		}
		
		if (Input.GetKey ("r")) {
			Debug.Log(Application.loadedLevel);
		}
		
		
		if (Input.GetButtonDown ("Cancel")) {
			SwitchPause();
		}

		Debug.Log (Application.levelCount);
	}
	
	public void Restart(){
		
		Application.LoadLevel ("LonelyBlob");
		
	}

	public void Quit(){
		
		Application.LoadLevel ("MainMenu");
		
	}
	
	void PauseGame(bool state){
		if (state) {
			
			Time.timeScale = 0.0f;
		} 
		else {
			Time.timeScale = 1.0f;
			
		}
		pausePanel.SetActive (state);
	}
	
	public void SwitchPause(){
		
		if (isPaused) {
			isPaused = false;
		}
		else {
			isPaused = true;
		}
		
	}

    public void OnJumpButtonClicked()
    {

		jumpMusic.volume = 1.0f;
	
        GameObject.Find("Character").GetComponent<Rigidbody2D>().AddForce(Vector2.up * 4000);


    }
	
}
