using UnityEngine;
using System.Collections;

public class DeadlySkewer : MonoBehaviour {

	private int skewerDamage = 0;
	private GameObject playerObject = null;
	private PlayerStats playerStats = null;

	void Start() {
		skewerDamage = 1;
		playerObject = GameObject.FindGameObjectWithTag("Player");
		playerStats = playerObject.GetComponent<PlayerStats>();
	}

	void OnTriggerStay(Collider other) {
		if(other.GetComponent<Collider>().gameObject.tag == "Player") {
			playerStats.CurrentPlayerHealth -= skewerDamage;
			if(playerStats.CurrentPlayerHealth < 0) 
				playerStats.CurrentPlayerHealth = 0;
		}
	}

}
