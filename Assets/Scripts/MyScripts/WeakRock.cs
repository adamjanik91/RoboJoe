using UnityEngine;
using System.Collections;

public class WeakRock: MonoBehaviour {

	private ParticleSystem myParticleSystem = null;
	private MeshRenderer myMeshRenderer = null;
	private Collider myCollider = null;
	private GameObject myParentObject = null;

	void Start() {
		myParticleSystem = this.transform.parent.gameObject.GetComponent<ParticleSystem>();
		myMeshRenderer = this.transform.parent.gameObject.GetComponent<MeshRenderer>();
		myCollider = this.transform.parent.gameObject.GetComponent<Collider>();
		myParentObject = this.transform.parent.gameObject;
	}

	void OnTriggerEnter(Collider other) {
		if(other.GetComponent<Collider>().gameObject.tag == "Player") {
			myParticleSystem.Play();
			StartCoroutine("WaitAndDeactivateComponent");
			StartCoroutine("WaitAndDestroyObject");
		}
	}

	private IEnumerator WaitAndDeactivateComponent() {
		yield return new WaitForSeconds(2);
		myMeshRenderer.enabled = false;
		myCollider.enabled = false;
	}

	private IEnumerator WaitAndDestroyObject() {
		yield return new WaitForSeconds(2.5f);
		Destroy(myParentObject);
	}

}
