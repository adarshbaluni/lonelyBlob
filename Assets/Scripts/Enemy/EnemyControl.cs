using UnityEngine;
using System.Collections;

public class EnemyControl : MonoBehaviour {

	public float speed = 50.0f;
	public bool goingRight = false;
	public bool colliding = false;

	void Start(){

		if (goingRight) {

			speed *= -1;
		}


	}


	void OnCollisionEnter2D(Collision2D col){

		if (col.gameObject.name == "FallingSpike") {
			Destroy(gameObject);
		}

		if (colliding == false) {
			colliding = true;
		}
		
	}


	// Update is called once per frame
	void Update () {

		// For every colliding, filp the sprite and move to opposite direction
		if (colliding) {

			Debug.Log("Colliding");

			transform.localScale = new Vector2(-1*transform.localScale.x,transform.localScale.y);
			speed *= -1;
			colliding = false;
		}

		GetComponent<Rigidbody2D> ().velocity = new Vector2 (speed, GetComponent<Rigidbody2D> ().velocity.y);

	}




}


