using UnityEngine;
using System.Collections; 

public class AdvancedController : MonoBehaviour {

	static AdvancedController instance = null;
	static readonly object padlock = new object();
	
	AdvancedController() {
	}
	
	public static AdvancedController Instance {
		get {
			lock (padlock) {
				if(instance == null) {
					instance = new AdvancedController();
				}
				return instance;
			}
		}
	}
	 
	private GameObject nearestEnemy = null;
	private bool canRun = true;
	private bool canAttack = false;
	private CharacterFightingState characterFightingState = CharacterFightingState.Neutral;

	[SerializeField]
	private float gravity = 0.0f;
	[SerializeField]
	private float moveSpeed = 0.0f;
	[SerializeField]
	private float jumpForce = 0.0f;
	[SerializeField]
	private float flyForce = 0.0f;
	[SerializeField]
	private float doubleJumpForce = 0.0f;
	private float impactForce = 0.0f;
	private float timeCount = 0.0f;
	private float timeCountMax = 0.0f;
	private float groundDistance = 0.0f;
	private int dirTo = 0;
	private float destroyCountValue = 0.0f;
	private Vector3 dir = Vector3.zero;
	private Vector3 moveDirection = Vector3.zero;
	private Vector3 startPosition = Vector3.zero;
	private bool canJump = true;
	
	private CollisionFlags collisionFlags = 0;
	private CharacterMovingState characterMovingState = CharacterMovingState.Idle;
	
	private PlayerStats playerStats = null;
	private Transform myTransform = null;
	private CharacterController controller = null;
	private GameObject flashlight = null;
	private GameObject GameControllerGameObject = null;
	private GameController gameController = null;
	private EnemyStats enemyStats = null;
	private Animation myAnimation = null;
	private GameObject childGameObject_2 = null;
	private GameObject childGameObject_3 = null;
	private ParticleSystem child_2_ParticleSystem = null;
	private ParticleSystem child_3_ParticleSystem = null;
	private AudioClip fireClip = null;
	private Vector3 audioPosition = Vector3.zero;
	private GameObject camera = null;

	public GameObject SetNearestEnemy {
		set { nearestEnemy = value; }
	}

	void Start() {

		groundDistance = 30.0f;
		camera = GameObject.FindGameObjectWithTag("MainCamera");
		audioPosition = camera.transform.position;
		fireClip = Resources.Load("Sounds/Fire") as AudioClip;

		doubleJumpForce = 150.0f;
		destroyCountValue = 0.9f;
		gravity = 900.0f;
		moveSpeed = 100.0f;
		jumpForce = 300.0f;
		impactForce = 2000.0f;

		#if UNITY_ANDROID
			dir = Vector3.zero;
		#endif

		moveDirection = Vector3.zero;
		characterMovingState = CharacterMovingState.Idle;

		nearestEnemy = null;
		myTransform = this.transform;
		myAnimation = this.GetComponent<Animation>();

		childGameObject_2 = myTransform.GetChild(10).gameObject;
		childGameObject_3 = myTransform.GetChild(11).gameObject;
		child_2_ParticleSystem = childGameObject_2.GetComponent<ParticleSystem>();
		child_3_ParticleSystem = childGameObject_3.GetComponent<ParticleSystem>();


		playerStats = this.GetComponent<PlayerStats>();
		GameControllerGameObject = GameObject.FindGameObjectWithTag("GameController");
		gameController = GameControllerGameObject.GetComponent<GameController>();
		controller = GetComponent<CharacterController>();
		flashlight = myTransform.GetChild(12).gameObject;

		myAnimation["jump"].wrapMode = WrapMode.ClampForever;

		InvokeRepeating("SetDefaultAnimSpeed", 0, 0.3f);
		InvokeRepeating("CheckAndTerminatePlayer", 0, 0.1f);

		SetStartPlayerPosition();
		CheckAndSetCharacterMovingStateIdle();
	}

	void Update() {

		Debug.Log("characterMovingState" + characterMovingState);

		GetEnemy();
		canRun = true;

		#if UNITY_ANDROID
			dir = Vector3.zero;
			if(Input.acceleration.x < 0) dirTo = -1; 
			if(Input.acceleration.x > 0) dirTo = 1;
			dir.x = -Input.acceleration.x;
		#endif

		CheckAndSetCharacterMovingState();
		AnimatePlayerState();

		if(controller.isGrounded) { 
			CheckAndSetCharacterMovingStateIdle();
			Run();
			SetParentToGround();
			SetGroundedPlayerMoveDirection();
			SetTimerOnGround();
		}
		else {
			SetTimeCounterOnAir();
			SetParentToNull();
			if(!GroundDistanceCheck()) {
				characterMovingState = CharacterMovingState.Jumping;
			}
		}

		CheckAndJump();
		AddGravityForce();
		MovePlayer();
		UseFlashlight();
		moveSpeed = 100.0f;
		jumpForce = 60.0f;

		if(Input.GetAxis("Fire") > 0) {
			characterFightingState = CharacterFightingState.Fighting;
			SetCharacterStatic();
			AttackEnemy();
		}
		else {
			SetCharacterDefaults();
			characterFightingState = CharacterFightingState.Neutral;
		}

		if(myTransform.position.x != 5.0f && gameController.GetLoadedLevel != GameController.LoadedLevel.MainMenu) {
			myTransform.position = new Vector3(myTransform.position.x, myTransform.position.y, 5.0f);
		}

	}

	private void MovePlayerToStartPosition() {
		myTransform.position = startPosition;
	} 
	
	private void SetStartPlayerPosition() {
		startPosition = this.transform.position;
	}

	private void GetEnemy() {
		if(nearestEnemy != null) {
			enemyStats = nearestEnemy.GetComponent<EnemyStats>();
		}
	}

	private void AttackEnemy() {
		child_2_ParticleSystem.Emit(2);
		StartCoroutine("WaitToAtack");
		if(characterFightingState == CharacterFightingState.Fighting && enemyStats != null)
			enemyStats.CurrentEnemyHealth -= playerStats.GetAttackPoints * Time.deltaTime;
	}

	private IEnumerator WaitToAtack() {
		yield return new WaitForSeconds(2);
	}

	public void SetCharacterDefaults() {
		canRun = true;
		canJump = true;
		moveSpeed = 100.0f;
		jumpForce = 60.0f;
	}

	public void SetCharacterStatic() {
		canRun = false;
		canJump = false;
		moveSpeed = 0.0f;
		jumpForce = 0.0f;
	}

	private void SetDefaultAnimSpeed() {
		myAnimation["runFast"].speed = 3.0f;
		myAnimation["walk1"].speed = 4.0f;
	}

	private void UseFlashlight() {
		if(Input.GetKeyDown("f")) {
			if(flashlight.gameObject.active) 
				FlashlightDisable();
			else 
				FlashlightEnable();
		}
	}

	private void AnimatePlayerState() {

		if (characterMovingState == CharacterMovingState.Idle) {
			myAnimation.Play("idle");
		}

		if (characterMovingState == CharacterMovingState.Walking) {
			myAnimation.Play("walk1");
		}

		if (characterMovingState == CharacterMovingState.Running) {
			myAnimation.Play("runFast");
		}

		if (characterMovingState == CharacterMovingState.Jumping) {
			myAnimation.Play("jump");
		}

		if (characterMovingState == CharacterMovingState.Pushing) {
			myAnimation.Play("pushing");
		}

		if (characterFightingState == CharacterFightingState.Fighting) {
			myAnimation.Play("fire");
			AudioSource.PlayClipAtPoint(fireClip, audioPosition);
		}

		if (characterFightingState == CharacterFightingState.GettingDamage) {
			myAnimation.Play("damage");
		}
	}
	
	private enum CharacterMovingState {
		Idle = 0,
		Walking = 1,
		Running = 2,
		Jumping = 3,
		CastingLight = 4,
		Pushing = 5,
	}

	private enum CharacterFightingState {
		Neutral = 0,
		GettingDamage = 1,
		Fighting = 2,
	}


	private void Run() {
		if(Input.GetAxis("Run") > 0 && canRun) {
			moveSpeed *= 0.5f;
		}
	}

	private void CheckAndJump() {
		if(GroundDistanceCheck() && canJump)
			CheckAndSetCharacterMovingStateJumping();
	}

	private bool GroundDistanceCheck() {
		RaycastHit hit;
		if (Physics.Raycast(myTransform.position, Vector3.down, out hit) && hit.distance < groundDistance)
			return true;
		else return false;
	}
	
	private void CheckAndSetCharacterMovingState() {
		CheckAndSetCharacterMovingStateForward();
		CheckAndSetCharacterMovingStateBackward();
    }
	
	private void SetTimerOnGround() {
		timeCount = 0.0f;
	}
	
	private void SetTimeCounterOnAir() {
		timeCount += Time.deltaTime;
		timeCountMax = timeCount;
	}
	
	private void SetGroundedPlayerMoveDirection() {
		#if UNITY_ANDROID
			moveDirection = new Vector3(-dir.sqrMagnitude * dirTo, 0, 0);	
		#endif

		#if UNITY_STANDALONE_WIN
			moveDirection = new Vector3(-Input.GetAxis("Moving"), 0, 0);	
		#endif

		#if UNITY_EDITOR
			moveDirection = new Vector3(-Input.GetAxis("Moving"), 0, 0);	
		#endif

		if((moveDirection.x > 0 && moveDirection.x < 0.01f) || (moveDirection.x < 0 && moveDirection.x > -0.01f))	// dead zone
			moveDirection = new Vector3(0.0f, moveDirection.y, moveDirection.z);

		moveDirection *= moveSpeed;
	}
	
	private void AddGravityForce() {
		moveDirection.y -= gravity * Time.deltaTime * (timeCount + 0.1f);	// multiplier is Never equal zero
	}
	
	private void MovePlayer() {
        collisionFlags = controller.Move(moveDirection * Time.deltaTime);
	}
	
	private void SetParentToGround() {
		RaycastHit hit;
		float hitDistance;
		Rigidbody rigidbody;
		Transform objTransform;

		if(Physics.Raycast(myTransform.position, Vector3.down, out hit)) {
			if(hit.collider.gameObject.tag == "platformCJ") {
				myTransform.parent = hit.collider.gameObject.transform;
				canJump = false;
			}
			else if(hit.collider.gameObject.tag != "platformCJ" && hit.collider.gameObject.tag != "iceGround") canJump = true;
	    }

		if(Physics.Raycast(myTransform.position, Vector3.down, out hit)) {
			if(hit.collider.gameObject.tag == "platform") {
				myTransform.parent = hit.collider.gameObject.transform;
			}
		}

		if(Physics.Raycast(myTransform.position, Vector3.down, out hit)) {
			if(hit.collider.gameObject.tag == "slowGround" && controller.isGrounded) {
				myAnimation["run"].speed = 1;
				moveSpeed *= 0.2f;
			}
		}

		if(Physics.Raycast(myTransform.position, Vector3.down, out hit)) {
			if(hit.collider.gameObject.tag == "iceGround") {
				myTransform.position = new Vector3(myTransform.position.x - 85 * Time.deltaTime, myTransform.position.y - 55 * Time.deltaTime, myTransform.position.z);
				canJump = false;
			}
			else if(hit.collider.gameObject.tag != "platformCJ" && hit.collider.gameObject.tag != "iceGround") canJump = true;
		}

		if(Physics.Raycast(myTransform.position, Vector3.down, out hit)) {
			if(hit.collider.gameObject.tag == "lever") {
				hit.collider.gameObject.GetComponent<Rigidbody>().AddForceAtPosition(hit.collider.transform.InverseTransformDirection(Vector3.back.normalized * impactForce * Time.deltaTime * 80.0f), hit.collider.transform.position);
			}
	    }

		if(Physics.Raycast(myTransform.position, myTransform.TransformDirection(Vector3.forward.normalized), out hit)) {

			hitDistance = Vector3.Distance(this.transform.position, hit.collider.gameObject.transform.position);
			if(hit.collider.gameObject.tag == "lever" && hitDistance < 20.0f) {
				rigidbody = hit.collider.gameObject.GetComponent<Rigidbody>();
				objTransform = hit.collider.transform;
				myAnimation.Play("pushing");
				rigidbody.AddForceAtPosition(objTransform.TransformDirection(myTransform.InverseTransformDirection(Vector3.forward.normalized) * impactForce * Time.deltaTime * 70.0f), hit.point);
			}
	    }
	}
	
	private void SetParentToNull() {
		this.transform.parent = null;
	}
	
	private void CheckAndSetCharacterMovingStateForward() {
		#if UNITY_STANDALONE_WIN
			if(Input.GetAxis("Moving") > 0 && controller.isGrounded && canRun && moveSpeed > 0) {
				myTransform.eulerAngles = new Vector3(myTransform.eulerAngles.x, 270.0f, myTransform.eulerAngles.z);
				if(Input.GetAxis("Run") == 0)
					characterMovingState = CharacterMovingState.Running;
				else if(Input.GetAxis("Run") > 0)
					characterMovingState = CharacterMovingState.Walking;
			}
		#endif

		#if UNITY_EDITOR
			if(Input.GetAxis("Moving") > 0 && controller.isGrounded && canRun && moveSpeed > 0) {
				myTransform.eulerAngles = new Vector3(myTransform.eulerAngles.x, 270.0f, myTransform.eulerAngles.z);
				if(Input.GetAxis("Run") == 0)
					characterMovingState = CharacterMovingState.Running;
				else if(Input.GetAxis("Run") > 0)
					characterMovingState = CharacterMovingState.Walking;
			}
		#endif

		#if UNITY_ANDROID
			if(dir.sqrMagnitude * dirTo > 0 && controller.isGrounded && canRun && moveDirection.x != 0.0f && moveSpeed > 0) {
				myTransform.eulerAngles = new Vector3(myTransform.eulerAngles.x, 270.0f, myTransform.eulerAngles.z);
				if(Input.GetAxis("Run") == 0)
					characterMovingState = CharacterMovingState.Running;
				else if(Input.GetAxis("Run") > 0)
					characterMovingState = CharacterMovingState.Walking;
			}
		#endif
	}

	private void CheckAndSetCharacterMovingStateBackward() {
		#if UNITY_STANDALONE_WIN
			if(Input.GetAxis("Moving") < 0 && controller.isGrounded && canRun && moveSpeed > 0) {
				myTransform.eulerAngles = new Vector3(myTransform.eulerAngles.x, 450.0f, myTransform.eulerAngles.z);
				if(Input.GetAxis("Run") == 0)
					characterMovingState = CharacterMovingState.Running;
				else if(Input.GetAxis("Run") > 0)
					characterMovingState = CharacterMovingState.Walking;
			}
		#endif

		#if UNITY_EDITOR
			if(Input.GetAxis("Moving") < 0 && controller.isGrounded && canRun && moveSpeed > 0) {
				myTransform.eulerAngles = new Vector3(myTransform.eulerAngles.x, 450.0f, myTransform.eulerAngles.z);
				if(Input.GetAxis("Run") == 0)
					characterMovingState = CharacterMovingState.Running;
				else if(Input.GetAxis("Run") > 0)
					characterMovingState = CharacterMovingState.Walking;
			}
		#endif

		#if UNITY_ANDROID
			if(dir.sqrMagnitude * dirTo < 0 && controller.isGrounded && canRun && moveDirection.x != 0.0f && moveSpeed > 0) {
				myTransform.eulerAngles = new Vector3(myTransform.eulerAngles.x, 450.0f, myTransform.eulerAngles.z);
				if(Input.GetAxis("Run") == 0)
					characterMovingState = CharacterMovingState.Running;
				else if(Input.GetAxis("Run") > 0)
					characterMovingState = CharacterMovingState.Walking;
			}
		#endif
	}
	
	private void CheckAndSetCharacterMovingStateJumping() {
		if(Input.GetAxis("Vertical") < 0) {
		    jumpForce = doubleJumpForce;
		}
		if(Input.GetKeyDown("space")  && canJump) {
			moveDirection.y += jumpForce; 
			characterMovingState = CharacterMovingState.Jumping;
		}
	}

	#if UNITY_ANDROID
		public void CheckAndSetCharacterMovingStateJumpingPhone() {
			myTransform.position = new Vector3(myTransform.position.x, myTransform.position.y + 60.0f, myTransform.position.z);
			characterMovingState = CharacterMovingState.Jumping;
		}
	#endif

	private void FlashlightEnable() {
		flashlight.SetActive(true);
	}
	
	private void FlashlightDisable() {
		flashlight.SetActive(false);
	}

	private void CheckAndSetCharacterMovingStateIdle() {
		if(Input.GetAxis("Moving") == 0) 
			characterMovingState = CharacterMovingState.Idle;
	}
	
	private void CheckAndTerminatePlayer() {
		if(timeCountMax >= destroyCountValue && controller.isGrounded || playerStats.CurrentPlayerHealth <= 0) {
			gameController.PlayerIsDead = true;
		}
	}
	
}
