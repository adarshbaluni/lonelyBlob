using UnityEngine;
using System.Collections;

public class ShotMove : MonoBehaviour {

	public float speed = 40;

	public Vector2 direction = new Vector2(1, 0);

	private Vector2 movement;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		movement = new Vector2 (speed * direction.x, speed * direction.y);

	}

	void FixedUpdate()
	{
		GetComponent<Rigidbody2D>().velocity = movement;
	}
}
