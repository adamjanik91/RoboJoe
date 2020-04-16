using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameItemInitializator : MonoBehaviour {

	private CollectableFactory collectableFactory = null;
	private ICollectable diamondCollectable = null;
	private AudioClip diamondCollectClip = null;
	private int diamondCollectPoints = 0;

	private RockFactory rockFactory = null;
	private IRock smallRock = null;
	private IRock backgroundRock = null;
	private float distanceToCheck = 0.0f;
	private float timeToDeactivate = 0.0f;
	private float timeToDestroy = 0.0f;
	private List<Vector3> list_rock_003 = new List<Vector3>();
	private List<Vector3> list_rock_004 = new List<Vector3>(); 

	private EnemyFactory enemyFactory = null;
	private IEnemy alien = null;
	private float minEnemyHealth = 0.0f;
	private float maxEnemyHealth = 0.0f;
	private float attackPoints = 0.0f;
	private float enemyAttackDelay = 0.0f;
	private float enemyDistanceToFollow = 0.0f;
	private float enemyDistanceToAttack = 0.0f;
	private float enemyFollowSpeed = 0.0f;
	private AudioClip enemyAlienClip = null;

	public IEnemy Getalien {
		get { return alien; }
	}

	public AudioClip GetDiamondCollectClip {
		get { return diamondCollectClip; }
	}

	public int GetDiamondCollectPoints {
		get { return diamondCollectPoints; }
	}

	public float GetDistanceToCheck {
		get { return distanceToCheck; }
	}
	
	public float GetTimeToDeactivate {
		get { return timeToDeactivate; }
	}

	public float GetTimeToDestroy {
		get { return timeToDestroy; }
	}

	public List<Vector3> GetList_rock_003 {
		get { return list_rock_003; }
	}

	public List<Vector3> GetList_rock_004 {
		get { return list_rock_004; }
	}

	public float GetMinEnemyHealth {
		get { return minEnemyHealth; }
	}

	public float GetMaxEnemyHealth {
		get { return maxEnemyHealth; }
	}
	
	public float GetAttackPoints {
		get { return attackPoints; }
	}

	public float GetEnemyAttackDelay {
		get { return enemyAttackDelay; }
	}

	public float GetEnemyDistanceToFollow {
		get { return enemyDistanceToFollow; }
	}

	public float GetEnemyDistanceToAttack {
		get { return enemyDistanceToAttack; }
	}

	public float GetEnemyFollowSpeed {
		get { return enemyFollowSpeed; }
	}

	public AudioClip GetEnemyAlienClip {
		get { return enemyAlienClip; }
	}

	void Start() {

		if(alien == null) AlienInitialize();
		if(diamondCollectable == null) CollectableInitialize();
		if(smallRock == null && backgroundRock == null) RockInitialize();
	}

	public void AlienInitialize() {
		EnemyFactory enemyFactory = new EnemyFactory();
		enemyFactory.SaveEnemy(0, new Alien());
		alien = enemyFactory.GetEnemy(0);
		minEnemyHealth = alien.minEnemyHealth;
		maxEnemyHealth = alien.maxEnemyHealth;
		attackPoints = alien.attackPoints;
		enemyAttackDelay = alien.enemyAttackDelay;
		enemyDistanceToFollow = alien.enemyDistanceToFollow;
		enemyDistanceToAttack = alien.enemyDistanceToAttack;
		enemyFollowSpeed = alien.enemyFollowSpeed;
		enemyAlienClip = alien.enemyAlienClip;
	}

	public void RockInitialize() {
		rockFactory = new RockFactory();
		rockFactory.SaveRock(0, new SmallRock());
		rockFactory.SaveRock(1, new BackgroundRock());
		smallRock = rockFactory.GetRock(0);
		backgroundRock = rockFactory.GetRock(1);
		distanceToCheck = smallRock.distanceToCheck;
		timeToDeactivate = smallRock.timeToDeactivate;
		timeToDestroy =	smallRock.timeToDestroy;
		list_rock_003 = smallRock.positionList;
		list_rock_004 = backgroundRock.positionList;
	}

	public void CollectableInitialize() {
		collectableFactory = new CollectableFactory();
		collectableFactory.SaveCollectable(0, new DiamondCollectable());
		diamondCollectable = collectableFactory.GetCollectable(0);
		diamondCollectClip = diamondCollectable.audioClip;
		diamondCollectPoints = diamondCollectable.points;
	}
	
}
