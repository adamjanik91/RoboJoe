using UnityEngine;
using System.Collections;

public class Energy : MonoBehaviour {

	private int sourceEnergy = 0;
	private GameObject playerObject = null;
	private PlayerStats playerStats = null;
	
	void Start() {
		playerObject = GameObject.FindGameObjectWithTag("Player");
		playerStats = playerObject.GetComponent<PlayerStats>();
		sourceEnergy = 50;
	}

	void OnTriggerStay(Collider other) {
		if(other.GetComponent<Collider>().gameObject.tag == "Player") {
			if(playerStats.CurrentPlayerHealth < 100 && sourceEnergy > 0) {
				playerStats.CurrentPlayerHealth ++;
				sourceEnergy --;
			}
		}
	}

}
