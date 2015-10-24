﻿using UnityEngine;
using System.Collections;

public class ArrowShooting : MonoBehaviour {

	public GameObject arrowClone;
	public Transform shotSpawn;
	public float fireRate;
	public float arrowSpeed;

	private Quaternion ArrowRotation;
	private Transform objectPosition;
	private Vector2 direction;
	
	// Use this for initialization
	void Start () {
	
		// Initialize the rotation instance
		ArrowRotation = new Quaternion(0,0,0,0);

		// Get the initial postion of the object
		objectPosition = gameObject.GetComponent<Transform> ().transform;

		// Start shooting in 0.1f second, call shoot method every fireRate second
		InvokeRepeating ("shoot", 0.1f, fireRate);

		// Get the vector from object position to shotSpawn position
		direction = new Vector2 (shotSpawn.position.x - objectPosition.position.x,
		                                 shotSpawn.position.y - objectPosition.position.y);

		// Get the rotation value that is needed for arrow
		ArrowRotation.SetFromToRotation (new Vector3 (1, 0, 0), direction);

	}

	void shoot(){
	
		// Instantiate the arrow game object 
		GameObject clone =  Instantiate(arrowClone, shotSpawn.position, ArrowRotation) as GameObject;

		// set the speed for the arrow
		clone.GetComponent<Rigidbody2D> ().velocity = direction * arrowSpeed;

	}



}
