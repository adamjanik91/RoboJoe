using UnityEngine;
using System.Collections;

public class LevelExit : MonoBehaviour {

	private GameObject gameControllerGameObject = null;
	private GameController gameController = null;
	private GUIManager guiManager = null;
	private GameObject playerGameObject = null;

	void Start() {
		gameControllerGameObject = GameObject.Find("GameController");
		gameController = gameControllerGameObject.GetComponent<GameController>();
		playerGameObject = GameObject.FindGameObjectWithTag("Player");
		guiManager = playerGameObject.GetComponent<GUIManager>();
	}

	void OnTriggerEnter(Collider other) {
		if(other.gameObject.tag == "Player") 
			guiManager.HasWon = true;
	}

}
