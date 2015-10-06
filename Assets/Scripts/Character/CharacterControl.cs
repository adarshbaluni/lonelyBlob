using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class CharacterControl : MonoBehaviour {

	public int velocity = 50;
	public int jumpForce = 3;
	public int cameraYOffset = 30;
	public int cameraXOffset = 10;
	bool freeCamera = false;
	private int new_direction=1;
	public Vector2 direction = new Vector2(1f, 0f);
	Sprite playerFwd;
	Sprite playerReverse;
	Sprite playerSticky;
	public Camera cam;
	bool isGround;
    bool fly;
	public float moveForce = 5, boostMultiplier = 2;
	Rigidbody2D myBody;
    public bool m_useAutomaticMovement = true;
    public float m_autoMoveSpeed = 5.0f;
    public int m_velDir = 1;


	// Use this for initialization
	void Start () {
		//powerMgr = (PowerupController)gun.GetComponent ("PowerupController");
		new_direction = (int) direction.x;
		playerFwd = Resources.Load<Sprite> ("Character");
		playerReverse = Resources.Load<Sprite> ("Character_reversed"); 
		playerSticky = Resources.Load<Sprite> ("CharacterSticky"); 
		myBody = this.GetComponent<Rigidbody2D>();
	}

	void FixedUpdate ()
	{
		Vector2 moveVec = new Vector2(CrossPlatformInputManager.GetAxis("Horizontal"),0) * moveForce;
        bool isBoosting = CrossPlatformInputManager.GetButton("Jump");
        if (isBoosting)
            Debug.Log("Boosting");
		//Debug.Log(isBoosting ? boostMultiplier : 1); //returns boostMultiplier if true, 1 if false
		myBody.AddForce(moveVec * (isBoosting ? boostMultiplier : 1));

        if (!m_useAutomaticMovement && Input.GetAxis("Horizontal") != 0)
        {
            ManualCharacterMovement();
        }
        else
        {
           AutomaticCharacterMovement();   
        }


        //if (CrossPlatformInputManager.GetButton("JumpButton"))
         //   GetComponent<Rigidbody2D>().AddForce(Vector2.up * jumpForce * 1000);
	}
	
	// Update is called once per frame
	void Update () {

		//Update camera position
		if (!freeCamera) {
			MoveObject camScr = cam.GetComponent("MoveObject") as MoveObject;
			camScr.updatePosition(new Vector3(transform.position.x-cameraXOffset,transform.position.y+cameraYOffset,cam.transform.position.z));
		}

		if(Input.GetKeyDown(KeyCode.Space)){
            //Jump
            if (isGround)
            {
                //if (CrossPlatformInputManager.GetButton("JumpButton"))
                    GetComponent<Rigidbody2D>().AddForce(Vector2.up * jumpForce * 1000);
                //myBody.velocity = new Vector2(myBody.velocity.x, jumpForce * 200);
            }
            else
                fly = true;
		}
        if (Input.GetKey(KeyCode.Space))
        {
            if (fly)
            {
                GetComponent<Rigidbody2D>().AddForce(Vector2.up * 250);
            }
        }
		if(Input.GetAxis("Vertical") != 0){
			//Create move vector based on keyboard input
			Vector3 move = new Vector3 (0, Input.GetAxis ("Vertical"), 0);

			//Make the camera move freely by player
			freeCamera = true;

			//Make movement FPS independent and add constant velocity
			move *= Time.deltaTime * velocity;	
			cam.transform.Translate (move);
		}
	}

    void ManualCharacterMovement()
    {
        //Move character
        float inputX = Input.GetAxis("Horizontal");
        Vector3 moveDirection = new Vector3(-inputX, 0, 0);
        myBody.velocity = new Vector2(moveDirection.x * Time.deltaTime * velocity * 50.0f, myBody.velocity.y); //FPS independent velocity

        //Lock camera to follow character
        freeCamera = false;
    }

    void AutomaticCharacterMovement()
    {
        if(myBody.velocity.x == 0)
        {
            //We have collided and not moving anymore, so change direction
            m_velDir = (m_velDir == 1) ? -1 : 1;
        }

        float velX = m_velDir * Time.deltaTime * m_autoMoveSpeed * 50.0f;
        

        myBody.velocity = new Vector2(velX, myBody.velocity.y); //FPS independent velocity

        //Lock camera to follow character
        freeCamera = false;
    }

	void OnCollisionEnter2D(Collision2D col){

	}


	void OnCollisionStay2D(Collision2D col){
        if (GetComponent<Rigidbody2D>().velocity.y > 0)
            isGround = false;
        else
        {
            isGround = true;
            fly = false;
        }
     }

	void reverseDirection(float inputX){
		if (inputX < 0)
			new_direction = 1;
		else if (inputX > 0)
			new_direction = -1;

		if (((int)direction.x != new_direction)) {
			direction.x = (float)new_direction;

			SpriteRenderer rend = GetComponent<SpriteRenderer> ();

			if (new_direction < 0)
				rend.sprite = playerReverse;
			else
				rend.sprite = playerFwd;
		}
	}
}
