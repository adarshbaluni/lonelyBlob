using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class CharacterControl : MonoBehaviour {
    //Camera parameters
    public Camera cam;
    public int cameraYOffset = 30;
    public int cameraXOffset = 10;
    bool freeCamera = false;

    //Movement params
    Rigidbody2D myBody;
    private int new_direction = 1;
	public int velocity = 50;
    public float moveForce = 5, boostMultiplier = 2;
    public bool m_useAutomaticMovement = true;
    public float m_autoMoveSpeed = 5.0f;
    public int m_velDir = -1;

    //Jump params
	public int jumpForce = 5;
    public Vector2 direction = new Vector2(1f, 0f);
    bool isGround = true;
    bool fly;
    float lastFrameVelY = 0;
    float playerAccel = 0;
    float lastPlayerAccel = 0;

    //Deprecated params ?
	Sprite playerFwd;
	Sprite playerReverse;
	Sprite playerSticky;

    enum CharacterState { GROUND, JUMPING, FALLING };
    CharacterState m_state;

    Animator m_animator;

	// Use this for initialization
	void Start () {
		//powerMgr = (PowerupController)gun.GetComponent ("PowerupController");
		new_direction = (int) direction.x;
		playerFwd = Resources.Load<Sprite> ("Character");
		playerReverse = Resources.Load<Sprite> ("Character_reversed"); 
		playerSticky = Resources.Load<Sprite> ("CharacterSticky"); 
		myBody = this.GetComponent<Rigidbody2D>();
        m_animator = GetComponent<Animator>();
	}

	void FixedUpdate ()
	{
		if(UIManager.isPaused || Win.isWon || DamagePlayer.isLost){

			return;
		}

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
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //Jump
            if (m_state == CharacterState.GROUND)
            {
                m_state = CharacterState.JUMPING;
                m_animator.Play("JUMP");
                m_animator.SetBool("Airborn", true);
                //if (CrossPlatformInputManager.GetButton("JumpButton"))
                myBody.AddForce(Vector2.up * jumpForce * 1000);
                //isGround = false;
                //myBody.velocity = new Vector2(myBody.velocity.x, jumpForce * 200);
            }
        }
        UpdateCharacterState();
	}
	
    void UpdateCameraPosition()
    {
        MoveObject camScr = cam.GetComponent("MoveObject") as MoveObject;
        //Update camera position
        float offSet = (myBody.transform.position.x - camScr.getPostion().x);
        float absOffSet = offSet > 0 ? offSet : -offSet;
        if (absOffSet > 15)
        {

            camScr.updatePosition(new Vector3(myBody.transform.position.x, camScr.getPostion().y, camScr.getPostion().z));
            if (offSet > 0)
            {
                camScr.updatePosition(new Vector3(myBody.transform.position.x - 15, camScr.getPostion().y, camScr.getPostion().z));
            }
            else
            {
                camScr.updatePosition(new Vector3(myBody.transform.position.x + 15, camScr.getPostion().y, camScr.getPostion().z));
            }
        }

        offSet = (myBody.transform.position.y - camScr.getPostion().y);
        absOffSet = offSet > 0 ? offSet : -offSet;
        if (absOffSet > 20)
        {

            // camScr.updatePosition(new Vector3(myBody.transform.position.x, camScr.getPostion().y, camScr.getPostion().z));
            if (offSet > 0)
            {
                camScr.updatePosition(new Vector3(camScr.getPostion().x, myBody.transform.position.y - 20, camScr.getPostion().z));
            }
            else
            {
                camScr.updatePosition(new Vector3(camScr.getPostion().x, myBody.transform.position.y + 20, camScr.getPostion().z));
            }
        }

        if (camScr.getPostion().x < -300)
        {
            camScr.updatePosition(new Vector3(-300, camScr.getPostion().y, camScr.getPostion().z));
        }
        else if (camScr.getPostion().x > 20)
        {
            camScr.updatePosition(new Vector3(20, camScr.getPostion().y, camScr.getPostion().z));
        }
    }

	// Update is called once per frame
	void Update () {
        UpdateCameraPosition();
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
    
    void UpdateCharacterState()
    {
        float playerAccel = myBody.velocity.y - lastFrameVelY;
        float deltaAccel = playerAccel - lastPlayerAccel; //change in acceleration
        float fallingAccelTolerance = -3.0f;
        if (m_state == CharacterState.JUMPING && lastFrameVelY > 0 && myBody.velocity.y <= 0)
        {
            //velocity just changed from positive to negative so character now is falling
            m_state = CharacterState.FALLING;
        }
        else if (m_state == CharacterState.FALLING && deltaAccel < fallingAccelTolerance)
        {
            //Character decelerated so it is landing
            m_state = CharacterState.GROUND;
            m_animator.Play("lAND");
        }

        lastPlayerAccel = playerAccel;
        lastFrameVelY = myBody.velocity.y;
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
        float deltaAccel = playerAccel - lastPlayerAccel;
        if(myBody.velocity.x == 0) /*&& m_state == CharacterState.GROUND*/
        {
            if(Mathf.Abs(deltaAccel) < 1)
            {
                //We have collided and not moving anymore, so change direction
                m_velDir = (m_velDir == 1) ? -1 : 1;
                Vector3 TempScale = transform.localScale;
                TempScale.x *= -1;
                transform.localScale = TempScale;
            }
        }
        
        float velX = m_velDir * Time.deltaTime * m_autoMoveSpeed * 50.0f;

        myBody.velocity = new Vector2(velX, myBody.velocity.y); //FPS independent velocity

        //Lock camera to follow character
        freeCamera = false;
    }

	void OnCollisionEnter2D(Collision2D col){

	}


	void OnCollisionStay2D(Collision2D col){

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

    public void OnJumpButtonClicked()
    {
        if (m_state == CharacterState.GROUND)
        {
            m_state = CharacterState.JUMPING;
            m_animator.Play("JUMP");
            m_animator.SetBool("Airborn", true);
            //if (CrossPlatformInputManager.GetButton("JumpButton"))
         //   myBody.AddForce(Vector2.up * jumpForce * 1000);
            myBody.velocity = new Vector2(myBody.velocity.x, 200);

            //isGround = false;
            //myBody.velocity = new Vector2(myBody.velocity.x, jumpForce * 200);
        }
    }
}
