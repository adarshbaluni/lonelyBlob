using UnityEngine;
using UnityEngine.UI; 
using System.Collections;

public class UIManager : MonoBehaviour
{
	public GameObject pausePanel;
	public  static bool isPaused;
	public AudioSource jumpMusic;
	private Animator anim;
	// Use this for initialization
	void Start ()
	{
		isPaused = false;
		anim = pausePanel.GetComponent<Animator> ();
		anim.enabled = false;
		//pausePanel.SetActive (false);
	}
	// Update is called once per frame
	void Update ()
	{
		if (isPaused) {
			PauseGame (true);
		}
		else {
			UnPauseGame (false);
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

		Win.isWon = false;
		Application.LoadLevel ("LonelyBlob");
		
	}

	public void Quit(){
		Win.isWon = false;
		Application.LoadLevel ("MainMenu");
		
	}
	
	void PauseGame(bool state){
		if (state) {

			anim.enabled=true;
			anim.Play("pauseanimation");
			Time.timeScale = 0.0f;
		} 
		//else {
		//	Time.timeScale = 1.0f;
			
		//}
		//pausePanel.SetActive (state);
	}

	void UnPauseGame(bool state){
	
		anim.Play("pauseanimation0");  
			Time.timeScale = 1.0f;
			

	
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
	
        GameObject.Find("Character").GetComponent<Rigidbody2D>().AddForce(Vector2.up * 5000);


    }
	
}
