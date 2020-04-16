using UnityEngine;
using System.Collections;

public class GUIPositioner : MonoBehaviour {

	private float screenWidth = 0.0f;
	private float screenHeight = 0.0f;
	private float screenAspectRatio = 0.0f;
	private Transform myTransform = null;
	
	void Start() {
		myTransform = this.transform;
		SetObjectPosition();
	}

	private float GetScreenAspectRatio() {
		screenHeight = Screen.height;
		screenWidth = Screen.width;
		screenAspectRatio = (screenWidth / screenHeight);

		return screenAspectRatio *= 10;
	}

	private void SetObjectPosition() {
		if((int)GetScreenAspectRatio() == 15) myTransform.position = new Vector3(-125.0f, myTransform.position.y, myTransform.position.z);
		if((int)GetScreenAspectRatio() == 12) myTransform.position = new Vector3(-106.0f, myTransform.position.y, myTransform.position.z);
		if((int)GetScreenAspectRatio() == 13) myTransform.position = new Vector3(-111.0f, myTransform.position.y, myTransform.position.z);
		if((int)GetScreenAspectRatio() == 16) myTransform.position = new Vector3(-133.0f, myTransform.position.y, myTransform.position.z);
		if((int)GetScreenAspectRatio() == 17) myTransform.position = new Vector3(-147.0f, myTransform.position.y, myTransform.position.z);
	}

}
