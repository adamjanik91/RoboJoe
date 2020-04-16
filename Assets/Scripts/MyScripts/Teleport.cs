using UnityEngine;
using System.Collections;

public class Teleport : MonoBehaviour {

	private GameObject playerObject = null;
	private Transform playerTransform = null;
	private string teleportName = null;
	private Vector3 []teleportDestinations = new Vector3[2];
	
	void Start() {
		playerObject = GameObject.FindGameObjectWithTag("Player");
		playerTransform = playerObject.GetComponent<Transform>();
		teleportName = this.gameObject.name;
		SetTeleportDestinations();
	}

	private void SetTeleportDestinations() {
		teleportDestinations[0] = new Vector3(-837.8796f, -193.5236f, 5.0f);
		teleportDestinations[1] = new Vector3(-2832.571f, -363.5753f, 5.0f);
	}

	void OnTriggerEnter(Collider other) {
		if(other.gameObject.tag == "Player") {
			var c = playerObject.GetComponent<CharacterController>();
			c.enabled = false;

			if (teleportName == "Teleport01") playerTransform.position = teleportDestinations[0];
			if (teleportName == "Teleport02") playerTransform.position = teleportDestinations[1];


			c.enabled = true;
		}
	}

}
