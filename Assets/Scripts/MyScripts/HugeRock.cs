using UnityEngine;
using System.Collections;

public class HugeRock : MonoBehaviour {

	private GameObject playerObject = null;
	private PlayerStats playerStats = null;
	private Transform myTransform = null;
	private float damageForce = 0.0f;

	void Start() {
		playerObject = GameObject.FindGameObjectWithTag("Player");
		playerStats = playerObject.GetComponent<PlayerStats>();
		myTransform = this.gameObject.transform;
		damageForce = 50.0f;
	}

	void OnCollisionEnter(Collision other) {
		if(other.gameObject.tag == "Player") {
			MakeDamageRelatedToObjectSize();
		}
	}

	private void MakeDamageRelatedToObjectSize() {
		playerStats.CurrentPlayerHealth -= damageForce * (int)(myTransform.localScale.x);
	}

}
