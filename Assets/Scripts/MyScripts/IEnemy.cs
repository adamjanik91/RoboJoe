using UnityEngine;
using System.Collections;

public interface IEnemy {
	
	float minEnemyHealth { get; }
	float maxEnemyHealth { get; }
	float attackPoints { get; }
	float enemyAttackDelay { get; }
	float enemyDistanceToFollow { get; }
	float enemyDistanceToAttack { get; }
	float enemyFollowSpeed { get; }
	AudioClip enemyAlienClip { get; }
	
}
