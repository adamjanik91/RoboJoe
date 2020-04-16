using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {

	static EnemyController instance = null;
	static readonly object padlock = new object();
	
	EnemyController() {
	}
	
	public static EnemyController Instance {
		get {
			lock (padlock) {
				if(instance == null) {
					instance = new EnemyController();
				}
				return instance;
			}
		}
	}

	private float startYPos = 0.0f;
	private float startZPos = 0.0f;
	private float followSpeed = 0.0f;
	private float gravity = 0.0f;
	private float distanceToFollow = 0.0f;
	private float distanceToAttack = 0.0f;
	private bool canAttack = false;
	private Vector3 moveDirection = Vector3.zero;

	private GameObject playerObject = null;
	private GameObject myGameObject = null;
	private GameObject target = null;
	private Transform myTransform = null;
	private PlayerStats playerStats = null;
	private EnemyStats enemyStats = null;
	private AdvancedController advancedController = null;
	private CharacterController characterController = null;
	private CollisionFlags collisionFlags = 0;
	private GameObject childGameObject_2 = null;
	private GameObject childGameObject_3 = null;
	private ParticleSystem child_2_ParticleSystem = null;
	private ParticleSystem child_3_ParticleSystem = null;
	private ParticleSystem myParticleSystem = null;
	private Animation myAnimation = null;
	private AudioClip alienClip = null;
	private Vector3 audioPosition = Vector3.zero;
	private GameObject camera = null;
	private GameObject gameItemInitializatorGameObject = null;
	private GameItemInitializator gameItemInitializator = null;

	void Start() {

		gameItemInitializatorGameObject = GameObject.FindGameObjectWithTag("GameItemInitializator");
		gameItemInitializator = gameItemInitializatorGameObject.GetComponent<GameItemInitializator>();
		camera = GameObject.FindGameObjectWithTag("MainCamera");
		audioPosition = camera.transform.position;
		alienClip = Resources.Load("Sounds/alien") as AudioClip;
		gravity = 5.0f;
		this.GetComponent<Animation>()["fire"].wrapMode = WrapMode.ClampForever;
		this.GetComponent<Animation>()["fire"].speed = 3.0f;
		moveDirection = Vector3.zero;

		if(gameItemInitializator.Getalien != null) {
			distanceToFollow = gameItemInitializator.GetEnemyDistanceToFollow;
			distanceToAttack = gameItemInitializator.GetEnemyDistanceToAttack;
			followSpeed = gameItemInitializator.GetEnemyFollowSpeed;
		}
		else {
			gameItemInitializator.AlienInitialize();
			
			distanceToFollow = gameItemInitializator.GetEnemyDistanceToFollow;
			distanceToAttack = gameItemInitializator.GetEnemyDistanceToAttack;
			followSpeed = gameItemInitializator.GetEnemyFollowSpeed;
		}

		myGameObject = this.gameObject;
		myTransform = this.transform;
		childGameObject_2 = myTransform.GetChild(2).gameObject;
		childGameObject_3 = myTransform.GetChild(3).gameObject;
		child_2_ParticleSystem = childGameObject_2.GetComponent<ParticleSystem>();
		child_3_ParticleSystem = childGameObject_3.GetComponent<ParticleSystem>();
		characterController = this.GetComponent<CharacterController>();
		playerObject = GameObject.FindGameObjectWithTag("Player");
		playerStats = playerObject.GetComponent<PlayerStats>();
		enemyStats = this.GetComponent<EnemyStats>();
		advancedController = playerObject.GetComponent<AdvancedController>();
		myParticleSystem = this.GetComponent<ParticleSystem>();
		myAnimation = this.GetComponent<Animation>();

		target = playerObject;
		startYPos = myTransform.position.y;
		startZPos = myTransform.position.z;

		SetEnemyLocalDirection();
	}

	private void SetEnemyLocalDirection() {
		var localDirection = transform.rotation * Vector3.forward;
		localDirection = transform.InverseTransformDirection(Vector3.forward);
	}
	
	void Update() {

		if (enemyStats.CurrentEnemyHealth <= 0) {
			DestroyEnemy();
		}

		if(characterController.isGrounded) {
			if (DetectTargetDistance() > distanceToFollow) {
				child_2_ParticleSystem.Stop();
				PlayIdleAnim();
			}

			if (DetectTargetDistance() < distanceToFollow && DetectTargetDistance() > distanceToAttack) {
				child_2_ParticleSystem.Play();
				FollowPlayer();
			}

			if (DetectTargetDistance() <= distanceToAttack) {
				advancedController.SetNearestEnemy = myGameObject;
				AttackPlayer();
			}
		}

		AddGravityForce();
		collisionFlags = characterController.Move(moveDirection * Time.deltaTime);
	}


	private bool CollisionForward() {
		RaycastHit hit;
		if (Physics.Raycast(myTransform.position, Vector3.left, out hit)) {
			float distance = Vector3.Distance(myTransform.position, hit.point);
			if(hit.collider.gameObject.tag != "Player" && distance < 10.0f)
				return true;
			else return false;
		}
		else return false;
	}

	private void AddGravityForce() {
		moveDirection.y -= gravity * Time.deltaTime;
	}

	private void DestroyEnemy() {
		myParticleSystem.Play();
		WaitToDestroy();
		Destroy(myGameObject);
	}
	
	private IEnumerator WaitToDestroy() {
		yield return new WaitForSeconds(1);
	}
	
	private void SetZEnemyPosition() {
		myTransform.position = new Vector3(myTransform.position.x, startYPos, startZPos);
	}
	
	private float DetectTargetDistance() {
		return Vector3.Distance(myTransform.position, target.transform.position);
	}
	
	private void PlayIdleAnim() {
		myAnimation.Play("idle_0");
	}
	
	private void FollowPlayer() {
		if(target.transform.position.x > myTransform.position.x)
			myTransform.eulerAngles = new Vector3(myTransform.eulerAngles.x, 180.0f, myTransform.eulerAngles.z);

		if(target.transform.position.x < myTransform.position.x)
			myTransform.eulerAngles = new Vector3(myTransform.eulerAngles.x, 0.0f, myTransform.eulerAngles.z);

		if(!CollisionForward()) {
			myTransform.Translate(Vector3.left * Time.deltaTime * followSpeed);
			myAnimation.Play("walk");
		}
		else PlayIdleAnim();
	}
	
	private void AttackPlayer() {
		AudioSource.PlayClipAtPoint(alienClip, audioPosition);
		myAnimation.Play("fire");
		StartCoroutine("WaitToAtack");
		playerStats.CurrentPlayerHealth -= enemyStats.GetAttackPoints * Time.deltaTime;
	}
	
	private IEnumerator WaitToAtack() {
		yield return new WaitForSeconds(2);
	}
	
}