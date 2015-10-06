using UnityEngine;
using System.Collections;

public class FallingObject : MonoBehaviour {

	public float timer = 3;    // how long will the object shaking
	public float speed = 0.2f; //how fast it shakes
	public float amount = 0.25f; //how much it shakes
	public bool vibrateToRight = false;
	

	// Use this for initialization
	void Start () {

		// Start shaking
		InvokeRepeating ("shake", 0.01f, speed);

		GetComponent<Rigidbody2D> ().isKinematic = true;

	
	}

	// Update is called once per frame
	void Update () {
	
		timer -= Time.deltaTime;
	
		if (timer < 0) {

			CancelInvoke("shake");
			objectDrop ();
		}
	}

	void OnTriggerEnter2D(Collider2D collider){

		// Destory itself if it hits something
		Destroy (gameObject);

	}


	void objectDrop(){

		// Make the object to be affected by gravity, so it falls
		GetComponent<Rigidbody2D> ().isKinematic = false;

	}
	

	void shake(){
		
		Vector2 newPosition = GetComponent<Transform> ().position;
		Vector2 movement = new Vector2 (amount, 0);
		
		if (vibrateToRight == true) {

			GetComponent<Transform>().position = newPosition + movement;
			vibrateToRight = false;
			
		}else{
			
			GetComponent<Transform>().position = newPosition - movement;
			vibrateToRight = true;
		}
		
	}



}
