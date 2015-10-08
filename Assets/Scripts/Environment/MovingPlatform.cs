using UnityEngine;
using System.Collections;

public class MovingPlatform : MonoBehaviour {

	public bool movingHorizontally = false;
	public bool movingVertically = false;
	public float speed = 15.0f;
	public float amount = 40.0f; 

	private Vector2 initialLoc;
	private bool atMax = false;
	private bool atMin = false;


	// Use this for initialization
	void Start () {
	
		// Take initial location as middle point
		initialLoc = new Vector2 (transform.position.x, transform.position.y);


	}
	
	// Update is called once per frame
	void Update () {

		// If moving horizontally
		if (movingHorizontally) {

			// Object goes right
			GetComponent<Rigidbody2D> ().velocity = new Vector2 (speed, GetComponent<Rigidbody2D> ().velocity.y);

			// If the distance between current location and initial location greater than amount, goes left
			if( (transform.position.x - initialLoc.x) > amount && !atMax){
				speed *= -1;
				atMax = true;
				atMin = false;
			}

			// Goes right again if the object hit the left bound
			if( (initialLoc.x - transform.position.x) > amount && !atMin){
				speed *= -1;
				atMax = false;
				atMin = true;
			}


		}

		// If moving vertically
		if (movingVertically) {

			// Object goes up
			GetComponent<Rigidbody2D> ().velocity = new Vector2 (GetComponent<Rigidbody2D> ().velocity.x,speed );

			// If the distance between current location and initial location greater than amount, goes down
			if( (transform.position.y - initialLoc.y) > amount && !atMax){
				speed *= -1;
				atMax = true;
				atMin = false;
			}
			// Goes up again if the object hit the bottom bound
			if( (initialLoc.y - transform.position.y) > amount && !atMin){
				speed *= -1;
				atMax = false;
				atMin = true;
			}


		}
	
	}
}
