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

        colliding = (colliding) ? false : true;
		
	}

	void OnTriggerEnter2D(Collider2D collider){

		if (collider.gameObject.tag == "Player") {
			Destroy(gameObject);
		}

	}


	// Update is called once per frame
	void Update () {


		GetComponent<Rigidbody2D> ().velocity = new Vector2 (speed, GetComponent<Rigidbody2D> ().velocity.y);

		// For every colliding, filp the sprite and move to opposite direction
		if (colliding) {
			transform.localScale = new Vector2(transform.localScale.x *-1,transform.localScale.y);
			speed *= -1;
			colliding = false;

		}


	}




}


