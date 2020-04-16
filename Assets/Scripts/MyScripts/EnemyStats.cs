using UnityEngine;
using System.Collections;

public class EnemyStats : MonoBehaviour {

	static EnemyStats instance = null;
	static readonly object padlock = new object();
	
	EnemyStats() {
	}
	
	public static EnemyStats Instance {
		get {
			lock (padlock) {
				if(instance == null) {
					instance = new EnemyStats();
				}
				return instance;
			}
		}
	}

	private GameObject gameItemInitializatorGameObject = null;
	private GameItemInitializator gameItemInitializator = null;
	private float currentEnemyHealth = 0.0f;
	private float minEnemyHealth = 0.0f;
	private float maxEnemyHealth = 0.0f;
	private float attackPoints = 0.0f;
	private float attackDelay = 0.0f;

	public float CurrentEnemyHealth {
		get { return currentEnemyHealth; }
		set { currentEnemyHealth = value; }
	}

	public float GetAttackPoints {
		get { return attackPoints; }
	}

	public float GetAttackDelay {
		get { return attackDelay; }
	}

	void Start() {
	
		gameItemInitializatorGameObject = GameObject.FindGameObjectWithTag("GameItemInitializator");
		gameItemInitializator = gameItemInitializatorGameObject.GetComponent<GameItemInitializator>();

		if(gameItemInitializator.Getalien != null) {
			currentEnemyHealth = gameItemInitializator.GetMaxEnemyHealth;
			minEnemyHealth = gameItemInitializator.GetMinEnemyHealth;
			maxEnemyHealth = 100;
			attackPoints = gameItemInitializator.GetAttackPoints;
			attackDelay = gameItemInitializator.GetEnemyAttackDelay;
		}
		else {
			gameItemInitializator.AlienInitialize();

			currentEnemyHealth = gameItemInitializator.GetMaxEnemyHealth;
			minEnemyHealth = gameItemInitializator.GetMinEnemyHealth;
			maxEnemyHealth = 100;
			attackPoints = gameItemInitializator.GetAttackPoints;
			attackDelay = gameItemInitializator.GetEnemyAttackDelay;
		}
	}

}
