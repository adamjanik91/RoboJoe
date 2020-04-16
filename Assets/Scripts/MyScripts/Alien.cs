using UnityEngine;
using System.Collections;

public class Alien : IEnemy {

	private float minEnemyHealth = 1.0f;
	private float maxEnemyHealth = 100.0f;
	private float attackPoints = 10.0f;
	private float enemyAttackDelay = 2.0f;
	private float enemyDistanceToFollow = 100.0f;
	private float enemyDistanceToAttack = 50.0f;
	private float enemyFollowSpeed = 40.0f;
	private AudioClip enemyAlienClip = Resources.Load("Sounds/alien") as AudioClip;

	float IEnemy.minEnemyHealth {
		get { return minEnemyHealth; }
	}

	float IEnemy.maxEnemyHealth {
		get { return maxEnemyHealth; }
	}

	float IEnemy.attackPoints {
		get { return attackPoints; }
	}

	float IEnemy.enemyAttackDelay {
		get { return enemyAttackDelay; }
	}

	float IEnemy.enemyDistanceToFollow {
		get { return enemyDistanceToFollow; }
	}

	float IEnemy.enemyDistanceToAttack {
		get { return enemyDistanceToAttack; }
	}

	float IEnemy.enemyFollowSpeed {
		get { return enemyFollowSpeed; }
	}

	AudioClip IEnemy.enemyAlienClip {
		get { return enemyAlienClip; }
	}
	
}
