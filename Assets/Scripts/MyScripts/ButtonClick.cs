using UnityEngine;
using System.Collections;

public class ButtonClick : MonoBehaviour {

	private GameObject playerObject = null;
	private GUIManager guiManager = null;
	private string name = null;

	void Start() {
		playerObject = GameObject.FindGameObjectWithTag("Player");
		guiManager = playerObject.GetComponent<GUIManager>();
		name = this.gameObject.name;
	}

	void OnMouseDown() {
		guiManager.ButtonClicked(name);
	}

}
