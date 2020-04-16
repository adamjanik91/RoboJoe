using UnityEngine;
using System.Collections;

public class Falling : MonoBehaviour {

	private GameObject myGameObject = null;
	private GameObject fallingObjectGameObject = null;
	private FallingObjectManager fallingObjectManager = null;

	void Start() {
		myGameObject = this.gameObject; 
		fallingObjectGameObject = GameObject.Find("FallingObjectManager");
		fallingObjectManager = fallingObjectGameObject.GetComponent<FallingObjectManager>();
		InvokeRepeating("DestroyOnTime", 0, 1);
	}
	
	void Update() {
		fallingObjectManager.CollisionCheck(myGameObject);
	}

	private void DestroyOnTime() {
		fallingObjectManager.DestroyOnTime(myGameObject);
	}
}
