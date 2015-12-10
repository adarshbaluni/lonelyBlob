using UnityEngine;
using System.Collections;

public class ArrowMover : MonoBehaviour {

public CharacterControl Character;

	void OnCollisionEnter2D(Collision2D col){


		if (col.gameObject.tag == "Player") {
		
			// If arrow hit the player, destroy the player and itself
		   
              DamagePlayer.isLost = true;		
              CharacterControl.m_animator.Play("Death");
		Destroy (col.gameObject);
			Destroy(gameObject);


		} else {

			// Otherwise, destroy itself.
			Destroy(gameObject);

		}


	}

}
