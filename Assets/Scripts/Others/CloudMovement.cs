using UnityEngine;
using System.Collections;

public class CloudMovement : MonoBehaviour {
	
	public float speed = 5f;

	public Vector2 direction = new Vector2(1, 0);

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		Vector3 playerPos = GameObject.Find ("Character").transform.position;

		if (transform.position.x - playerPos.x > 500)
			Destroy (gameObject);

	
	}

	void FixedUpdate()
	{
		GetComponent<Rigidbody2D>().velocity = new Vector2(speed * direction.x, speed * direction.y);
	}
}
