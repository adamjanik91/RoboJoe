using UnityEngine;
using System.Collections;

public class WeakPlatform : MonoBehaviour {

	private ParticleSystem myParticleSystem = null;
	private MeshRenderer myMeshRenderer = null;
	private Collider myCollider = null;
	private GameObject myGameObject = null;
	
	void Start() {
		myParticleSystem = this.GetComponent<ParticleSystem>();
		myMeshRenderer = this.GetComponent<MeshRenderer>();
		myCollider = this.GetComponent<Collider>();
		myGameObject = this.gameObject;
		myParticleSystem.startSize = 0.4f;
	}

	void Update() {
		CheckAndDestroyPlatform();
	}

	private void CheckAndDestroyPlatform() {
		RaycastHit hit;
		if(Physics.Raycast(this.transform.position, Vector3.up, out hit)){
			if(hit.collider.gameObject.tag == "Player") {
				myParticleSystem.Play();
				StartCoroutine("WaitAndDeactivate");
				myParticleSystem.startSize = 2.0f;
				StartCoroutine("WaitAndDestroy");
			}
		}
	}

	private IEnumerator WaitAndDeactivate() {
		yield return new WaitForSeconds(2);
		myMeshRenderer.enabled = false;
		myCollider.enabled = false;
	}

	private IEnumerator WaitAndDecreaseSize() {
		yield return new WaitForSeconds(1);
		myParticleSystem.startSize = 0.4f;
	}

	private IEnumerator WaitAndDestroy() {
		yield return new WaitForSeconds(3);
		Destroy(myGameObject);
	}

}
