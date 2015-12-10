using UnityEngine;
using System.Collections;

public class ArrowShooting : MonoBehaviour {

	public GameObject arrowClone;
	private float shotPoint_x;
	public float fireRate;
	public float arrowSpeed;

	public bool facingRight;

	//private Quaternion ArrowRotation;
	private Transform objectPosition;
	private Vector2 direction;
	
	// Use this for initialization
	void Start () {
	
		if(!facingRight) gameObject.transform.localRotation = Quaternion.Euler(0, 180, 0);

		// Initialize the rotation instance
		//ArrowRotation = new Quaternion(0,0,0,0);

		// Get the initial postion of the object
		objectPosition = gameObject.GetComponent<Transform> ().transform;

		// Start shooting in 0.1f second, call shoot method every fireRate second
		InvokeRepeating ("shoot", 0.1f, fireRate);

		// Set up shooting point
		shotPoint_x = facingRight ? objectPosition.position.x - 7 : objectPosition.position.x + 7;		

		// Get the vector from object position to shotSpawn position
		direction = new Vector2 (shotPoint_x, 0);

		// Get the rotation value that is needed for arrow
		//ArrowRotation.SetFromToRotation (new Vector3 (1, 0, 0), direction);

	}

	void shoot(){
	
		// Instantiate the arrow game object 

		GameObject clone = Instantiate (arrowClone, 
		                                new Vector3 (shotPoint_x, objectPosition.position.y+2,objectPosition.position.z),
		                                Quaternion.identity) as GameObject;

		// set the speed for the arrow
		clone.GetComponent<Rigidbody2D> ().velocity = direction * arrowSpeed;

		if(!facingRight) clone.transform.localRotation = Quaternion.Euler(0, 180, 0);

	}



}
