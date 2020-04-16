using UnityEngine;
using System.Collections;

public class RockTrigger : MonoBehaviour {

	private GameObject targetObject = null;
	private Rigidbody targetRigidbody = null;
	private float forceToAdd = 0.0f;
	
	void Start() {
		forceToAdd = 1400.0f;
		targetObject = GameObject.Find("rock_004T");
		targetRigidbody = targetObject.GetComponent<Rigidbody>();
	}

	private void OnTriggerEnter(Collider other) {
		if(other.gameObject.tag == "Player") {
			targetRigidbody.AddForce(forceToAdd, 0.0f, 0.0f);
		}
	}

}
