using UnityEngine;
using UnityEngine.UI; 
using System.Collections;

public class MessageText : MonoBehaviour
{
	public GameObject messagePanel;
	public TextMesh message;
	public bool isMessage;

	void Start ()
	{
		isMessage = false;

	}
	// Update is called once per frame
	void Update ()
	{

		if(isMessage){

			ShowMessage(true);
		}

		else{

			ShowMessage(false);

		}
		
		//if (Input.GetButtonDown ("Submit")) {
			//Debug.Log(Application.loadedLevel);
			//messagePanel.SetActive(true);
			//message.text="i am here";
		if (Input.GetKeyDown(KeyCode.Tab)) {
			Show();
		}
		
		
	}

	void Show(){

		if(isMessage)
			isMessage = false;
		else
			isMessage = true;
	}

	void ShowMessage(bool state){
		messagePanel.SetActive(state);
		if(state)
		message.text="i am here";
	}


	
}
