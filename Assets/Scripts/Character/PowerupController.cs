using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/* This script controls the powerups the character has */

public class PowerupController : MonoBehaviour {
	public enum Powerup { normal, ghost, sticky };

	public int numGhost = 3;
	public int numSticky = 3;
	public Rigidbody2D ghostBdy;
	public float stickyTime = 10;

	public Powerup powerType = Powerup.normal;
	public Dictionary<Powerup,int> powers;
	float stickyTimer; //Timer that counts how much time has the character been sticky
	public bool isSticky = false;

	// Use this for initialization
	void Start () {
		powers = new Dictionary<Powerup,int>();
		powers.Add(Powerup.normal, 1);
		powers.Add(Powerup.ghost,numGhost);
		powers.Add(Powerup.sticky,numSticky);
	}
	
	// Update is called once per frame
	void Update () {
		if (isSticky) {
			//Increase timer of sticky status
			stickyTimer += Time.deltaTime;
		}
		if (stickyTimer > stickyTime) {
			Debug.Log("Finished Sticky");
			stickyTimer = 0.0f;
			isSticky = false;
			Rigidbody2D playerBdy = (Rigidbody2D)GameObject.Find("Character").GetComponent("Rigidbody2D");
			playerBdy.isKinematic = false;
		}

		if (Input.GetKeyDown (KeyCode.E)) {
			//Normal powerup
			powerType = Powerup.normal;
		} else if (Input.GetKeyDown (KeyCode.R)) {
			//Ghost powerup
			powerType = Powerup.ghost;
		} else if (Input.GetKeyDown (KeyCode.T)) {
			//Sticky powerup
			powerType = Powerup.sticky;
		}
	}

	public void createGhost(){
		//Get Mouse world position
		Camera cam = GameObject.Find ("Camera").GetComponent<Camera>();
		Vector3 mouseWrldPos = cam.ScreenToWorldPoint(Input.mousePosition);
		mouseWrldPos.z = 0; //Eliminate Z component because its 2D
		//Create Ghost same size of player and without gravity
		Rigidbody2D ghostBdyInst = (Rigidbody2D)Instantiate (ghostBdy, mouseWrldPos, Quaternion.identity);
		ghostBdyInst.isKinematic = true;
		//ghostBdyInst.transform.localScale = transform.localScale;
	}

	public void addStickiness(){
		Debug.Log ("I am sticky !");
		stickyTimer = 0.0f;
		//TODO: Change character sprite to sticky character
		isSticky = true;
	}

	public bool hasPowerup(){
		Debug.Log ("Powertype" + powerType);
		return (int)powers[powerType] > 0;
	}

	public void addPowerup(int type){
		if (type == 0) { //Ghost
			powers [Powerup.ghost] = powers [Powerup.ghost] + 1;
		//	GameObject.Find ("HUDAppleCount").GetComponent<TextMesh>().text = powers [Powerup.ghost].ToString();
		}
       	else{ //Sticky
			powers [Powerup.ghost] = powers [Powerup.sticky] + 1;
			//GameObject.Find ("HUDBananaCount").GetComponent<TextMesh>().text = powers [Powerup.sticky].ToString();
		}

	}

	/*Removes one of the active powerup*/
	public void removePowerup(){
		if (powerType != Powerup.normal) {
			powers [powerType] = (int)powers [powerType] - 1;
		/*	if(powerType == Powerup.ghost)
				GameObject.Find ("HUDAppleCount").GetComponent<TextMesh> ().text = powers [powerType].ToString ();
			else
				GameObject.Find ("HUDBananaCount").GetComponent<TextMesh> ().text = powers [powerType].ToString ();*/
		}
	}
}
