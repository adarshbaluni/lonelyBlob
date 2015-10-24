using UnityEngine;
using System.Collections;

public class ArrowMover : MonoBehaviour {


	void OnCollisionEnter2D(Collision2D col){


		if (col.gameObject.tag == "Player") {
		
			// If arrow hit the player, destory the player and itself
			Destroy (col.gameObject);
			Destroy(gameObject);

		} else {

			// Otherwise, destory itself.
			Destroy(gameObject);

		}


	}

}
