using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnScript : MonoBehaviour {

	private ObjectType objectType = ObjectType.rock_003;
	private float startCounterValue = 0.0f;
	private float counterValue = 0.0f;
	[SerializeField]
	private GameObject spawningObject = null;
	private GameObject gameItemInitializatorGameObject = null;
	private GameItemInitializator gameItemInitializator = null;

	void Start() {

		gameItemInitializatorGameObject = GameObject.FindGameObjectWithTag("GameItemInitializator");
		gameItemInitializator = gameItemInitializatorGameObject.GetComponent<GameItemInitializator>();

		if(spawningObject.name == "rock_003") objectType = ObjectType.rock_003;
		if(spawningObject.name == "rock_004") objectType = ObjectType.rock_004;

		startCounterValue = 1.0f;	//how fast objects will spawn
		counterValue = startCounterValue;
	}
	
	void Update() {
		counterValue -= Time.deltaTime;
		if(counterValue < 0 && objectType == ObjectType.rock_003) {
			Instantiate(spawningObject, gameItemInitializator.GetList_rock_003[GenerateRandomIndexValue()], Quaternion.identity);
			counterValue = startCounterValue;
		}
		if(counterValue < 0 && objectType == ObjectType.rock_004) {
			Instantiate(spawningObject, gameItemInitializator.GetList_rock_004[GenerateRandomIndexValue()], Quaternion.identity);
			counterValue = startCounterValue;
		}
	}

	public int GenerateRandomIndexValue() {
		if(objectType == ObjectType.rock_003) 
			return Random.Range(0,13);

		if(objectType == ObjectType.rock_004) 
			return Random.Range(0,18);

		else return 0;
	}

	public enum ObjectType {
		rock_003 = 0,
		rock_004 = 1,
	}

}
