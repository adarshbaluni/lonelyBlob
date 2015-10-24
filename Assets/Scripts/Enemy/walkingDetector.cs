using UnityEngine;
using System.Collections;

public class walkingDetector : MonoBehaviour {

	private bool playerDetected = false;
	
	public float speed = 10.0f;
	
	private Vector3 initialLoc;
	private Vector2 playerLocation;
	private Transform player;



	// Use this for initialization
	void Start () {
	
		initialLoc = new Vector3 (transform.position.x, transform.position.y,0);

	}

	void OnTriggerEnter2D(Collider2D col){

		// If the player enter the collider box
		if (col.gameObject.tag == "Player") {

			// record the location of the player
			playerLocation = new Vector2( col.gameObject.transform.position.x,col.gameObject.transform.position.y);
			player = col.gameObject.transform;

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
	

	// Update is called once per frame
	void Update () {

		// player is detected
		if (playerDetected) {

			// Enemy start moving towards player
			transform.position = Vector2.MoveTowards (transform.position, player.position, speed * Time.deltaTime);

		} else { // player is not detected

			// Enemy moves back to its original location
			transform.position = Vector2.MoveTowards (transform.position, initialLoc, speed * Time.deltaTime);

		}
	
	}
}
