using UnityEngine;
using System.Collections;

public class DownSceneCollider : MonoBehaviour {

	private void OnTriggerEnter(Collider other) {
		if(other.gameObject.tag != "Player")
			Destroy(other.gameObject);
	}

}
