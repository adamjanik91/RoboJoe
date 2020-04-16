using UnityEngine;
using System.Collections;

public class PlayerStats : MonoBehaviour {

	static PlayerStats instance = null;
	static readonly object padlock = new object();
	
	PlayerStats() {
	}
	
	public static PlayerStats Instance {
		get {
			lock (padlock) {
				if(instance == null) {
					instance = new PlayerStats();
				}
				return instance;
			}
		}
	}
	
	private float currentPlayerHealth = 0.0f;
	private float minPlayerHealth = 0.0f;
	private float maxPlayerHealth = 0.0f;
	private int currentPlayerPoints = 0;
	private int totalNumberOfDeaths = 0;
	private float attackPoints = 0.0f;
	private string playerName = null;

	public float CurrentPlayerHealth {
		get { return currentPlayerHealth; }
		set { currentPlayerHealth = value; }
	}

	public int CurrentPlayerPoints {
		get { return currentPlayerPoints; }
		set { currentPlayerPoints = value; }
	}

	public int TotalNumberOfDeaths {
		get { return totalNumberOfDeaths; }
		set { totalNumberOfDeaths = value; }
	}

	public float GetAttackPoints {
		get { return attackPoints; }
	}

	public float GetMinPlayerHealth {
		get { return minPlayerHealth; }
	}

	public string PlayerName {
		get { return playerName; }
		set { playerName = value; }
	}

	void Start() {
		currentPlayerHealth = 100;
		minPlayerHealth = 1;
		maxPlayerHealth = 100;
		currentPlayerPoints = 0;
		totalNumberOfDeaths = 0;
		attackPoints = 80.0f;
	}

}
