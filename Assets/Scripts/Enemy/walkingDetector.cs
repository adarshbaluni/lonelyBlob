using UnityEngine;
using System.Collections;

public class walkingDetector : MonoBehaviour {

	private bool playerDetected = false;
	
	public float speed = 10.0f;
	
	private Vector3 initialLoc;
	private Vector2 playerLocation;
	private Transform player;
	private bool initialLocGet = false;
	private bool backToInitialLoc = false;

	private bool playerEnterAtRightSide;



	// Use this for initialization
	void Start () {
	
		//initialLoc = new Vector3 (transform.position.x, transform.position.y,0);

	}

	void OnTriggerEnter2D(Collider2D col){

		// If the player enter the collider box
		if (col.gameObject.tag == "Player") {

			// record the location of the player
			playerLocation = new Vector2( col.gameObject.transform.position.x,col.gameObject.transform.position.y);
			player = col.gameObject.transform;

			playerEnterAtRightSide =  (initialLoc.x - playerLocation.x) > 0? true:false;

			// player is detected
			playerDetected = true;
		}

	}

	void OnTriggerExit2D(Collider2D col) {

		// If the player left the collider box
		if (col.gameObject.tag == "Player") {

			// player is not detected
			playerDetected = false;


		}

	}

	void OnCollisionEnter2D(Collision2D coll) {

		if (coll.gameObject.tag == "Ground" & initialLocGet == false) {
			initialLoc = new Vector3 (transform.position.x, transform.position.y,0);
			initialLocGet = true;
		}

	}
	

	// Update is called once per frame
	void Update () {

		if (Mathf.Abs(initialLoc.x - transform.position.x) < 0.5) {

			GetComponent<Animator>().Play("playerDetector_idle");

		}


		// player is detected
		if (playerDetected) {

			if(playerEnterAtRightSide){

				GetComponent<Animator>().Play("playerDetector_move");

				// Enemy start moving towards player
				transform.position = Vector2.MoveTowards (transform.position, player.position, speed * Time.deltaTime);
			
			}else{

				GetComponent<Animator>().Play("playerDetector_move");

				// Filp the sprite
				transform.localRotation = Quaternion.Euler(0, 180, 0);

				// Enemy start moving towards player
				transform.position = Vector2.MoveTowards (transform.position, player.position, speed * Time.deltaTime);

			}



		} else { // player is not detected

			if(playerEnterAtRightSide){

				//GetComponent<Animator>().Play("playerDetector_move");

				// Filp the sprite
				transform.localRotation = Quaternion.Euler(0, 180, 0);

				// Enemy moves back to its original location
				transform.position = Vector2.MoveTowards (transform.position, initialLoc, speed * Time.deltaTime);

			}else{

				//GetComponent<Animator>().Play("playerDetector_move");

				// Filp the sprite
				transform.localRotation = Quaternion.Euler(0, 0, 0);
				
				// Enemy moves back to its original location
				transform.position = Vector2.MoveTowards (transform.position, initialLoc, speed * Time.deltaTime);
			}

		}
	
	}
}
