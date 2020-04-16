using UnityEngine;
using System.Collections;

public class Collectable : MonoBehaviour {

	private CollectableType collectableType = CollectableType.diamond;
	private GameObject playerObject = null;
	private PlayerStats playerStats = null;
	private Transform myTransform = null;
	private GameObject childObject = null;
	private Collider myCollider = null;
	private MeshRenderer myMeshRenderer = null;
	private ParticleSystem myParticleSystem = null;
	private GameObject myGameObject = null;
	private AudioClip collectClip = null;
	private Vector3 audioPosition = Vector3.zero;
	private GameObject camera = null;
	private GameObject gameItemInitializatorGameObject = null;
	private GameItemInitializator gameItemInitializator = null;
	private int collectPoints = 0;
	private string collectableName = null;

	void Start() {

		gameItemInitializatorGameObject = GameObject.FindGameObjectWithTag("GameItemInitializator");
		gameItemInitializator = gameItemInitializatorGameObject.GetComponent<GameItemInitializator>();

		myGameObject = this.gameObject;
		collectableName = myGameObject.name;
		SetCollectableType();

		camera = GameObject.FindGameObjectWithTag("MainCamera");
		audioPosition = camera.transform.position;
		playerObject = GameObject.FindGameObjectWithTag("Player");
		playerStats = playerObject.gameObject.GetComponent<PlayerStats>();
		myTransform = this.transform;
		childObject = myTransform.GetChild(0).gameObject;
		myCollider = this.gameObject.GetComponent<Collider>();
		myMeshRenderer = this.gameObject.GetComponent<MeshRenderer>();
		myParticleSystem = this.GetComponent<ParticleSystem>();
	} 

	private void SetCollectableType() {
		if(collectableName == "diamond") collectableType = CollectableType.diamond;
	}

	void OnTriggerEnter(Collider other) {
		if(other.gameObject.tag == "Player") {
			if(collectableType == CollectableType.diamond) {
				collectClip = gameItemInitializator.GetDiamondCollectClip;
				collectPoints = gameItemInitializator.GetDiamondCollectPoints;
				AudioSource.PlayClipAtPoint(collectClip, audioPosition);
				myParticleSystem.Play();
				playerStats.CurrentPlayerPoints += collectPoints;
				childObject.SetActive(true);
				myCollider.enabled = false;
				myMeshRenderer.enabled = false;
				StartCoroutine("WaitAndDestroy");
			}
		}
    }
	
	public IEnumerator WaitAndDestroy() {
		yield return new WaitForSeconds(1.5f);
		Destroy(myGameObject);
	}

	private enum CollectableType {
		diamond = 0,
	}

}
