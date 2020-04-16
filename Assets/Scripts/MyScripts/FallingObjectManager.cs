using UnityEngine;
using System.Collections;

public class FallingObjectManager : MonoBehaviour {
	
	private float distanceToCheck = 0.0f;
	private float timeToDeactivate = 0.0f;
	private float timeToDestroy = 0.0f;
	private int counter = 0;
	private PlayerStats playerStats = null;
	private GameObject playerObject = null;
	private Transform myTransform = null;
	private GameObject gameItemInitializatorGameObject = null;
	private GameItemInitializator gameItemInitializator = null;

	void Start() {

		gameItemInitializatorGameObject = GameObject.FindGameObjectWithTag("GameItemInitializator");
		gameItemInitializator = gameItemInitializatorGameObject.GetComponent<GameItemInitializator>();

		distanceToCheck = gameItemInitializator.GetDistanceToCheck;
		timeToDeactivate = gameItemInitializator.GetTimeToDeactivate;
		timeToDestroy =	gameItemInitializator.GetTimeToDestroy;
	}
	
	void Awake() {
		counter = 24;
		playerObject = GameObject.FindGameObjectWithTag("Player");
		playerStats = playerObject.GetComponent<PlayerStats>();
	}

	public void DestroyOnTime(GameObject go) {
		counter -= 1;
		if(counter == 0) {
			counter = 24;
			Destroy(go);
		}
	}

	public void CollisionCheck(GameObject go) {
		RaycastHit hit;
		myTransform = go.transform;
		if(Physics.Raycast(myTransform.position, Vector3.down, out hit,  distanceToCheck)) {
			
			if(hit.collider.tag == "Player") {
				if((playerStats.CurrentPlayerHealth != playerStats.GetMinPlayerHealth) && (go.GetComponent<MeshRenderer>().enabled == true))
					playerStats.CurrentPlayerHealth --;
			}
			
			if(go.GetComponent<Rigidbody>().angularVelocity.magnitude == 0 && go != null) {
				go.GetComponent<MeshRenderer>().enabled = false;
				PlayParticleAnimation(go);
				StartCoroutine(WaitAndDeactivateMeshComponent(go));
				WaitAndDestroyObject(go);
				if(hit.collider.gameObject.layer == 9)
					Physics.IgnoreCollision(hit.collider, GetComponent<Collider>());
			}
		}
	}
	
	private void PlayParticleAnimation(GameObject go) {
		go.GetComponent<ParticleSystem>().Play();
	}
	
	private IEnumerator WaitAndDeactivateMeshComponent(GameObject go) {
		yield return new WaitForSeconds(timeToDeactivate);
		if(go != null)
			go.GetComponent<Collider>().enabled = false;
	}
	
	private void WaitAndDestroyObject(GameObject go) {
		Destroy(go, timeToDestroy);
	}
	
}
