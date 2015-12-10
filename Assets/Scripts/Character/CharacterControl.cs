using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.UI;

public class CharacterControl : MonoBehaviour {
    //Camera parameters
    public Camera cam;
    public int cameraX1Offset = 0;
    public int cameraX2Offset = 0;
    bool freeCamera = false;

    //Audio
    public AudioSource JumpAudio;
    public AudioSource DeathAudio;

    //Movement params
    public static Rigidbody2D myBody;
    private int new_direction = 1;
	public int velocity = 50;
    public float moveForce = 5, boostMultiplier = 2;
    public bool m_useAutomaticMovement = true;
    public float m_autoMoveSpeed = 5.0f;
    public int m_velDir = -1;
    public float maxFallVelocity = -100;

	//timer in game
	public Text timerText;
	public static Text timerTextt;
	public static float  totalTime = 0f;
	public static float  totalTimee = 16f;

    //Jump params
	public int jumpForce = 5;
    public Vector2 direction = new Vector2(1f, 0f);
    bool fly;
    float lastFrameVelY = 0;
    float playerAccel = 0;
    float lastPlayerAccel = 0;	public static bool m_canDraw=true;
	public static bool isTap=true;

    //Deprecated params ?
	Sprite playerFwd;
	Sprite playerReverse;
	Sprite playerSticky;

    //public Transform topLeft;
    //public Transform botRight;
    public LayerMask raycastMask;

    enum CharacterState { GROUND, JUMPING, FALLING };
	public enum InputState { TAP, GESTURE, NOTHING};
	CharacterState m_state;
	public static InputState i_state;
    public static Animator m_animator;

	// Use this for initialization
	void Start () {
		//powerMgr = (PowerupController)gun.GetComponent ("PowerupController");
		new_direction = (int) direction.x;
		playerFwd = Resources.Load<Sprite> ("Character");
		playerSticky = Resources.Load<Sprite> ("CharacterSticky"); 
		playerReverse = playerSticky;//Resources.Load<Sprite> ("Character_reversed"); 
		myBody = this.GetComponent<Rigidbody2D>();
        m_animator = GetComponent<Animator>();
        cam = Camera.main;
		StartCoroutine(crSetReferences());
	}

    IEnumerator crSetReferences()
    {
        yield return null;
        timerText = GameObject.FindGameObjectWithTag("ScoreText").GetComponent<Text>();
	timerTextt=GameObject.FindGameObjectWithTag("Angrytimer").GetComponent<Text>();
    }

	void FixedUpdate ()
	{
		if(UIManager.isPaused || Win.isWon || DamagePlayer.isLost){

			return;
		}

		Vector2 moveVec = new Vector2(CrossPlatformInputManager.GetAxis("Horizontal"),0) * moveForce;

        UpdateCustomGravity();

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
            OnJumpButtonClicked();
            
        }
	
		if (m_state==CharacterState.GROUND) {
			//Jump
			
			
			if (Input.touchCount == 2 && Input.GetTouch (0).phase== TouchPhase.Stationary && Input.GetTouch (1).phase==TouchPhase.Stationary) 
			{
				m_canDraw = false;
				OnJumpButtonClicked();				
				
			} 	else if(Input.touchCount==1 && Input.GetTouch(0).phase==TouchPhase.Moved) {
				m_canDraw=true;	
			}		
		} else {
			
			if (Input.touchCount == 1 && Input.GetTouch(0).phase==TouchPhase.Moved) {
				
				m_canDraw = true;
			}
			
		}
		/* if (Input.touchCount == 1 && Input.GetTouch (0).phase == TouchPhase.Stationary) {
				i_state = InputState.TAP;
		} else if (Input.GetTouch (0).phase == TouchPhase.Moved && Input.touchCount==1) {

				i_state = InputState.GESTURE;
			}
				
		if (m_state == CharacterState.GROUND) {

			if (i_state == InputState.TAP) {
				m_animator.Play ("JUMP");
				m_animator.SetBool ("Airborn", true);
				myBody.AddForce (Vector2.up * jumpForce * 1000);
				m_state = CharacterState.JUMPING;
				i_state = InputState.NOTHING;
			} 
		}
	
		
	*/


        UpdateCharacterState();
	}

    void UpdateCustomGravity()
    {
        
        if(myBody.velocity.y < maxFallVelocity)
            myBody.velocity = new Vector2(myBody.velocity.x,maxFallVelocity);
        
    }
	
    void UpdateCameraPosition()
    {
        MoveObject camScr = cam.GetComponent("MoveObject") as MoveObject;
        //Update camera position
        if (m_velDir == -1)
        {
            camScr.updateGoalPosition(new Vector3(myBody.transform.position.x -50,camScr.getPostion().y,camScr.getPostion().z));
        }
        else
        {
            camScr.updateGoalPosition(new Vector3(myBody.transform.position.x + 50, camScr.getPostion().y, camScr.getPostion().z));
        }
        float offSet = (myBody.transform.position.x - camScr.getPostion().x);
        float absOffSet = offSet > 0 ? offSet : -offSet;
        

        //Vertical Camera follow - it's ok for now
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
        float maxBound = -10 - (float)(cam.orthographicSize * cam.aspect);
        float minBound = -210 + (float)(cam.orthographicSize * cam.aspect);
        float clampX = Mathf.Clamp(camScr.getPostion().x, minBound, maxBound);
        //camScr.updatePosition(new Vector3(clampX, camScr.getPostion().y, camScr.getPostion().z));

        minBound = 0+ (float)(cam.orthographicSize);
        maxBound = 375 - (float)(cam.orthographicSize);
        clampX = Mathf.Clamp(camScr.getPostion().y, minBound, maxBound);
        camScr.updatePosition(new Vector3(camScr.getPostion().x, clampX, camScr.getPostion().z));
		/*
		if (camScr.getPostion().x < cameraX1Offset)
        {
			camScr.updatePosition(new Vector3(cameraX1Offset, camScr.getPostion().y, camScr.getPostion().z));
        }
		else if (camScr.getPostion().x > cameraX1Offset)
        {
			camScr.updatePosition(new Vector3(cameraX2Offset, camScr.getPostion().y, camScr.getPostion().z));
        }*/
    }

	// Update is called once per frame
	void Update () {
		int tempScore = 0;
int tempScoree = 0;
        UpdateCameraPosition();

		//added timer ingame
		if((!Win.isWon)&&(!DamagePlayer.isLost)){
			totalTime += Time.deltaTime;
			tempScore = (int) totalTime;
			timerText.text = "T i m e : " + tempScore.ToString ();

if(Angrypower.angry)
{			
	
			totalTimee-= Time.deltaTime;
			tempScoree = (int) totalTimee;
		timerTextt.text = "T i m e : " + tempScoree.ToString ();



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
    
    void UpdateCharacterState()
    {
        float playerAccel = myBody.velocity.y - lastFrameVelY;
        float deltaAccel = playerAccel - lastPlayerAccel; //change in acceleration
        float fallingAccelTolerance = -50.0f;
        
        if (isGrounded() && m_state == CharacterState.FALLING) //Testing
        {
            m_state = CharacterState.GROUND;
            if (Angrypower.angry == true)
            {
                m_animator.Play("AngryBlobwalk");
            }
            else
            {
                m_animator.Play("lAND");
            }
        }

        if (m_state == CharacterState.JUMPING && lastFrameVelY > 0 && myBody.velocity.y <= 0) 
        {
            //velocity just changed from positive to negative so character now is falling
            m_state = CharacterState.FALLING;
        }
        else if (m_state == CharacterState.GROUND && !isGrounded())
        {
            m_state = CharacterState.FALLING;
			if(Angrypower.angry==true)
			{
				m_animator.Play ("AngryBlobwalk");
			}
			else
			{
            m_animator.Play("AIRBORN");
			}
        }
        else if ( ( m_state == CharacterState.FALLING  ) && isGrounded())
        {
            //Character decelerated so it is landing
            m_state = CharacterState.GROUND;
			if(Angrypower.angry==true)
			{
				m_animator.Play ("AngryBlobwalk");


			}

			else{
            m_animator.Play("lAND");
			}
			}

        lastPlayerAccel = playerAccel;
        lastFrameVelY = myBody.velocity.y;
	}

    void ManualCharacterMovement()
    {
      /*  //Move character
       float inputX = Input.GetAxis("Horizontal");
        Vector3 moveDirection = new Vector3(-inputX, 0, 0);
        myBody.velocity = new Vector2(moveDirection.x * Time.deltaTime * velocity * 50.0f, myBody.velocity.y); //FPS independent velocity

        //Lock camera to follow character
        freeCamera = false;
    */}

    void AutomaticCharacterMovement()
    {
        float deltaAccel = playerAccel - lastPlayerAccel;
        if(myBody.velocity.x == 0) /*&& m_state == CharacterState.GROUND*/
        {
            //if(Mathf.Abs(deltaAccel) < 1)
            if (isBesideWall())
            {
                //We have collided and not moving anymore, so change direction
                m_velDir = (m_velDir == 1) ? -1 : 1;
                Vector3 TempScale = transform.localScale;
                TempScale.x *= -1;
                transform.localScale = TempScale;
            }
        }
        
        float velX = m_velDir * Time.deltaTime * m_autoMoveSpeed * 50.0f;
        float velY = myBody.velocity.y;
        if (m_state == CharacterState.JUMPING || m_state == CharacterState.FALLING)
        {
            velY -= (Time.deltaTime * 300.0f);
        }
        myBody.velocity = new Vector2(velX, velY); //FPS independent velocity

        //Lock camera to follow character
        freeCamera = false;
    }

    void OnCollisionEnter2D(Collision2D col)
    {

    }

/*	void OnCollisionEnter2D(Collision2D col){



			if (Angrypower.sticky == true) {
				//Destroy (GameObject.Find("Character"));
			if(col.gameObject.name=="Heap (1)")
			{
			col.transform.parent = myBody.transform ;
			myBody.isKinematic=true;
				col.gameObject.isS
				m_useAutomaticMovement = false;
			}	}
		}

*/

    //bool isGrounded()
    //{
    //    //RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 5.0f);
    //    Renderer rend = GetComponent<Renderer>();
    //    //Vector2 topLeft = new Vector2(-rend.bounds.extents.x / 3.0f , 0);
    //    //Vector2 botRight = new Vector2(rend.bounds.extents.x / 3.0f, -rend.bounds.extents.y *5.0f);
    //    Collider2D col = Physics2D.OverlapArea(topLeft.position,botRight.position);
    //    bool grounded = false;
    //    if(col.gameObject.tag != "Player")
    //    {
    //        Debug.Log("Character is grounded");
    //        grounded = true;
    //    }
        
    //    return grounded;
    //}

    bool isGrounded()
    {
        Renderer rend = GetComponent<Renderer>();
        Collider2D col = GetComponent<Collider2D>();
        float distance = col.bounds.extents.y * 1.5f;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, distance);
       
        bool grounded = false;
        if(hit.collider == null)
        {
            grounded = false;
        }

        else if(hit.collider.gameObject.tag != "Player")
        {
      //      Debug.Log("Character is grounded");
            grounded = true;
        }

        return grounded;
    }

    bool isBesideWall()
    {
        Renderer rend = GetComponent<Renderer>();
        
        Vector3 position = GetComponent<Collider2D>().bounds.center;
        float distance = GetComponent<Collider2D>().bounds.extents.x*1.5f;
        if (checkRaycastCollision(Vector2.left, position, Vector3.zero/*new Vector3(rend.bounds.extents.x, 0, 0)*/,distance, "Player"))
        {
            return true;
        }
        else if (checkRaycastCollision(Vector2.right, position, Vector3.zero/*new Vector3(rend.bounds.extents.x, 0, 0)*/, distance, "Player"))
        {
            return true;
        }

        /*
        Renderer rend = GetComponent<Renderer>();
        RaycastHit2D leftHit = Physics2D.Raycast(transform.position - new Vector3(rend.bounds.extents.x, 0, 0), Vector2.left, 5.0f);
        RaycastHit2D leftHit = Physics2D.Raycast(transform.position + new Vector3(rend.bounds.extents.x, 0, 0), Vector2.left, 5.0f);
        
        bool colliding = false;
        if (leftHit.collider == null)
        {
            colliding = false;
        }

        */
        return false;
    }

    bool checkRaycastCollision(Vector2 direction, Vector3 position, Vector3 offset, float distance, string excludeTag)
    {
        RaycastHit2D hit = Physics2D.Raycast(position + offset, direction, distance);

        bool colliding = false;
        if (hit.collider == null)
        {
            return false;
        }
        else if(hit.collider.tag == excludeTag)
        {
            return false;
        }
        else
            return true;

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
			if(Angrypower.angry==true)
			{
				m_animator.Play ("AngryBlobwalk");
			}
			else{
                JumpAudio.Play();
            m_animator.Play("JUMP");
			}
				m_animator.SetBool("Airborn", true);
            //if (CrossPlatformInputManager.GetButton("JumpButton"))
            //myBody.AddForce(Vector2.up * jumpForce * 1000);
            myBody.velocity = new Vector2(myBody.velocity.x, 200);

            //isGround = false;
            //myBody.velocity = new Vector2(myBody.velocity.x, jumpForce * 200);
        }
    }

    public void OnDeath()
    {
        DeathAudio.Play();
        m_animator.Play("Death");
        myBody.GetComponent<Collider>().enabled = false;
       // while (m_animator.GetCurrentAnimatorStateInfo(0).IsName("Death"));
    }
}
