using UnityEngine;
using System.Collections;

public class CloudScript : MonoBehaviour {
	
	public Transform[] cloudPrefab = new Transform[4];

	private bool cloudCreated = false;

	public float cloudRate = 7;

	public float upperSpeed = 10.0f;
	public float randomCloudScaleLower = 20.0f;
	public int cloudSize = 30;
	public float startXPos = -2000.5f;
	public float lowerSpeed = 10;
	public float lowerY = -100f;
	public float upperY = 100;
	private float cloudCoolDown;
	// Use this for initialization
	void Start () {
		//cloudPrefab = new Transform[4];
		//cloudTransform = new Transform();
		cloudCoolDown = 0f;

	}
	
	// Update is called once per frame
	void Update () {

		/*if (randomCloudSpeedUpper < 1f)
						randomCloudSpeedUpper = 1f;*/

//		if (randomCloudScaleLower > 1f)
//						randomCloudScaleLower = cloudSize;

		if (cloudCoolDown <= 0f)
		{
			cloudCoolDown = cloudRate;
			int index = Random.Range(0,4);
			var cloudTransform = Instantiate (cloudPrefab [index]) as Transform;

			//cloudCreated = true;

			Vector3 offset = new Vector3 (startXPos, 20.5f + Random.Range(lowerY, upperY), -50f);
			cloudTransform.position = GameObject.Find ("Character").transform.position + offset;

			CloudMovement movement = cloudTransform.gameObject.GetComponent<CloudMovement>();

			movement.speed = movement.speed * Random.Range (lowerSpeed, upperSpeed);
			float scale = Random.Range(randomCloudScaleLower, cloudSize);
			cloudTransform.localScale = new Vector3(scale*1, scale*1, 1);
		}
		else
		{
			cloudCoolDown -= Time.deltaTime;
		}
	}

}
