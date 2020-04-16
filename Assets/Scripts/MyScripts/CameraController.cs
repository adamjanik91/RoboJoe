using UnityEngine;
using System.Collections;

public sealed class CameraController : MonoBehaviour {

	static CameraController instance = null;
	static readonly object padlock = new object();

	CameraController() {
	}

	public static CameraController Instance {
		get {
			lock (padlock) {
				if(instance == null) {
					instance = new CameraController();
				}
				return instance;
			}
		}
	}

	[SerializeField]
	private float distance = 0.0f;
	[SerializeField]
	private float height = 0.0f;
	[SerializeField]
	private GameObject target = null;
	private Transform myTransform = null;

	void Start() {
		distance = 80.0f;
		height = -20.0f;
		target = GameObject.FindGameObjectWithTag("Player");
		myTransform = this.transform;
	}
	
	void LateUpdate() {
		SetCameraPosition();
	}
	
	private void SetCameraPosition() {
		Vector3 myTransformCalculated;
		float targetPosX = target.transform.position.x;
		float targetPosZ = target.transform.position.z;
		float wantedHeight = Mathf.Lerp(myTransform.position.y, target.transform.position.y + height, 2.0f); 
		myTransformCalculated = new Vector3(targetPosX, wantedHeight, targetPosZ + distance);
		myTransform.position = myTransformCalculated;
	}
	
}
