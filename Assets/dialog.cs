using UnityEngine;
using UnityEngine.UI; 
using System.Collections;

public class dialog : MonoBehaviour
{
	public GameObject messagePanel;
	public Text message;
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
		
		if (Input.GetButtonDown ("Cancel")) {
			//Debug.Log(Application.loadedLevel);
			//messagePanel.SetActive(true);
			//message.text="i am here";

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
