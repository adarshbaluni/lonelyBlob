using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour {

	public Rigidbody2D gravBox;
	public float massModifier = 1.1f;
	public float minMass = 0.3f;
	public int gunPlayerDistance = 30;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        
		//Compute character and mouse positions
		Vector2 shootRay, gunPos;
		Vector2 playerPos = GameObject.Find ("Character").transform.position;
		Camera cam = GameObject.Find("Camera").GetComponent<Camera>();
		Vector2 mousePos = cam.ScreenToWorldPoint (Input.mousePosition);

		//Update Gun position to be around the character
		shootRay = mousePos - playerPos;
		shootRay.Normalize ();
		shootRay *= gunPlayerDistance;
		//playerPos = transform.position;
		gunPos = playerPos + shootRay;
		transform.position = gunPos;

        /*
		if (Input.GetMouseButtonDown (0)) { //Left click so attack

			PowerupController powerMgr = (PowerupController)GetComponent("PowerupController");
			if(powerMgr.isSticky){
				//If player is sticky then release from any attached object
				Rigidbody2D playerBdy = (Rigidbody2D)GameObject.Find("Character").GetComponent("Rigidbody2D");
				playerBdy.isKinematic = false;
			}

			if(powerMgr.hasPowerup()){
				if(powerMgr.powerType == PowerupController.Powerup.normal){
					shoot (gunPos,shootRay,false);
				}
				else if(powerMgr.powerType == PowerupController.Powerup.ghost){
					//TODO: Add ghost function
					powerMgr.removePowerup();
					powerMgr.createGhost();
					Debug.Log ("Remain Ghosts: "+powerMgr.powers[powerMgr.powerType]);
				}
				else if(powerMgr.powerType == PowerupController.Powerup.sticky){
					//TODO: Add Sticky function
					powerMgr.removePowerup();
					powerMgr.addStickiness();
					Debug.Log ("Remain Sticky: "+powerMgr.powers[powerMgr.powerType]);
				}

			}

		}

		else if (Input.GetMouseButtonDown (1)) {
			shoot (gunPos,shootRay,true);
		}
        */
	}

	void shoot(Vector3 gunPos,Vector2 shootRay,bool kinematic){
		RaycastHit2D hit = new RaycastHit2D ();
		if (hit = Physics2D.Raycast (gunPos, shootRay)) {
			//Debug.Log (hit.collider.name);
			if (hit.collider.tag == "Cube") {
				GameObject.Destroy(hit.collider.gameObject);
				ChangePlayerSize(true);
			}
			else{
				Rigidbody2D playerBody = (Rigidbody2D)GameObject.Find ("Character").GetComponent("Rigidbody2D");
				if(playerBody.mass > minMass){
					//If its mass is high enough, it can create boxes, otherwise he won't be able too
					Rigidbody2D gravBoxInst = (Rigidbody2D)Instantiate (gravBox, hit.point, Quaternion.identity);
					gravBoxInst.isKinematic = kinematic;
					//gravBoxInst.transform.localScale = GameObject.Find ("Character").transform.localScale;
					ChangePlayerSize(false);
				}
			}
		}

	}
	
	void ChangePlayerSize(bool bigger){
		Vector3 playerScale = GameObject.Find ("Character").transform.localScale;
		if (bigger) {
			//Make it bigger

			//Increase player size
			GameObject.Find ("Character").transform.localScale = new Vector3(playerScale.x*massModifier,playerScale.y*massModifier,playerScale.z*massModifier);
			
			//Reduce mass and size
			Rigidbody2D playerBody = (Rigidbody2D)GameObject.Find ("Character").GetComponent("Rigidbody2D");
			playerBody.mass = playerBody.mass*massModifier;
		} else {
			Rigidbody2D playerBody = (Rigidbody2D)GameObject.Find ("Character").GetComponent("Rigidbody2D");

			//Make it smaller
			//Debug.Log ("Scale: "+GameObject.Find ("Character").transform.localScale);
			GameObject.Find ("Character").transform.localScale = new Vector3(playerScale.x/massModifier,playerScale.y/massModifier,playerScale.z/massModifier);
			//Debug.Log ("New Scale: "+GameObject.Find ("Character").transform.localScale);

			//Change its mass to make it  more susceptible to extern fofrces and jump higher
			playerBody.mass = playerBody.mass/massModifier;
			//Debug.Log ("New Mass: "+playerBody.mass);
		}
	}
}
