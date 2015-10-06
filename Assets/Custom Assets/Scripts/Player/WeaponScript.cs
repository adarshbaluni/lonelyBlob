using UnityEngine;
using System.Collections;

public class WeaponScript : MonoBehaviour {

	public Transform shotPrefab;

	public float shootingRate = 0.25f;

	private float cooldown;

	// Use this for initialization
	void Start () {

		cooldown = 0f;
	
	}
	
	// Update is called once per frame
	void Update () {

		if (cooldown > 0) {
			cooldown -= Time.deltaTime;
				}
	
	}

	public void Fire(bool isLeft)
	{
		if (cooldown <= 0f) 
		{
			cooldown = shootingRate;
			var shotTransform = Instantiate (shotPrefab) as Transform;

			Vector3 offset = new Vector3(1.5f, -0.5f, 0.0f);
			shotTransform.position = transform.position + offset;
			shotTransform.rotation = transform.rotation;

			ShotScript shot = shotTransform.gameObject.GetComponent<ShotScript> ();

			if (shot != null) 
			{
				shot.isEnemyHit = false;
			}

			ShotMove movement = shotTransform.gameObject.GetComponent<ShotMove>();

			movement.direction = this.transform.right; 

			Debug.Log ("Transform.Right: " + this.transform.right);
			Debug.Log ("Movement.Direction: " + movement.direction);
		}

	}
}
