using UnityEngine;
using System.Collections;
using System.IO;
using System;
using System.Collections.Generic;

public class GameController : MonoBehaviour {
	
	static GameController instance = null;
	static readonly object padlock = new object();
	
	GameController() {
	}
	
	public static GameController Instance {
		get {
			lock (padlock) {
				if(instance == null) {
					instance = new GameController();
				}
				return instance;
			}
		}
	}

	private int gameCounter = 0;
	private LoadedLevel loadedLevel = LoadedLevel.MainMenu;
	private bool gamePaused = false;
	private bool playerIsDead = false;
	private bool sceneLoadedFirst = true;

	private List<int> points = new List<int>();
	private List<string> chars = new List<string>();
	private string []textToWrite = new string[10];
	private PlayerStats playerStats = null;
	private AdvancedController advancedController = null;
	private string [] textLines = null;
	private bool fileReaded = false;
	private string file = null;
	private GameObject playerGameObject = null;

	public bool GamePaused {
		get { return gamePaused; }
		set { gamePaused = value; }
	}

	public bool SceneLoadedFirst {
		get { return sceneLoadedFirst; }
		set { sceneLoadedFirst = value; }
	}

	public LoadedLevel GetLoadedLevel {
		get { return loadedLevel; }
	}

	public int GameCounter {
		get { return gameCounter; }
		set { gameCounter = value; }
	}

	public bool PlayerIsDead {
		get { return playerIsDead; }
		set { playerIsDead = value; }
	}
	
	void Start() {
		DontDestroyOnLoad(this);
		playerGameObject = GameObject.FindGameObjectWithTag("Player");
		advancedController = playerGameObject.GetComponent<AdvancedController>();
		playerStats = playerGameObject.GetComponent<PlayerStats>();

		file = "Records.txt";
		if(File.Exists(file) && new FileInfo(file).Length > 0) {
			ReadFile(file);
		}

		InvokeRepeating("IncrementGameCounter", 0, 1);
	}
	
	void Update() {
		CheckLoadedLevel();
		PauseRequestCheck();
		if(Application.loadedLevel == (int)LoadedLevel.MainMenu) {
			advancedController.SetCharacterStatic();
			gameCounter = 0;
		}
	}

	private void IncrementGameCounter() {
		if(loadedLevel != LoadedLevel.MainMenu)
			gameCounter ++;
	}
	
	private void PauseRequestCheck() {
		if(Input.GetKeyDown("escape")) sceneLoadedFirst = true;
	}
	
	public void	LoadNextScene() {
		#if UNITY_ANDROID
		Application.LoadLevel((int)(loadedLevel+1));
		sceneLoadedFirst = false;
		advancedController.SetCharacterDefaults();
		gameCounter = 0;
		#endif

		#if UNITY_STANDALONE_WIN
		Application.LoadLevel((int)(loadedLevel+1));
		sceneLoadedFirst = false;
		advancedController.SetCharacterDefaults();
		gameCounter = 0;
		#endif

		#if UNITY_EDITOR
		Application.LoadLevel((int)(loadedLevel+1));
		sceneLoadedFirst = false;
		advancedController.SetCharacterDefaults();
		gameCounter = 0;
		#endif
	}
	
	public void LoadMainMenu() {
		Application.LoadLevel((int)(LoadedLevel.MainMenu));
	}
	
	public void ExitApplication() {
		Application.Quit();
	}
	
	private void CheckLoadedLevel() {
		if(Application.loadedLevel == 0) loadedLevel = LoadedLevel.MainMenu;
		if(Application.loadedLevel == 1) loadedLevel = LoadedLevel.Level_1;
		if(Application.loadedLevel == 2) loadedLevel = LoadedLevel.Level_2;
		if(Application.loadedLevel == 3) loadedLevel = LoadedLevel.Level_3;
	}
	
	public enum LoadedLevel {
		MainMenu = 0,
		Level_1 = 1,
		Level_2 = 2,
		Level_3 = 3,
	}
	
	public void SetGameStatusPaused() {
		Time.timeScale = 0;
		gamePaused = true;
	}
	
	public void SetGameStatusRunning() {
		Time.timeScale = 1;
		gamePaused = false;
	}
	
	public void ReloadCurrentLevel() {
		Application.LoadLevel((int)loadedLevel);
		playerIsDead = false;
	}
	
	public void SavePlayerRecord(string name) {
		ReadFile("Records.txt");
	}
	
	public void ReadFile(string file) {
		string[] text;

		if(!fileReaded) {
			text = File.ReadAllText(file).
				Split(new String[] { " ", Environment.NewLine },
				StringSplitOptions.RemoveEmptyEntries);

			foreach (String item in text) {
				int value = 0;
				if (Int32.TryParse(item.Trim(), out value)) {
					points.Add(value);
				}
				else {
					chars.Add(item);
				}
			}
			fileReaded = true;
		}
		else {
			text = File.ReadAllText(file).
			Split(new String[] { " ", Environment.NewLine },
			StringSplitOptions.RemoveEmptyEntries);
			int x = 0;
			int y = 0;
			foreach (String item in text) {
				int value = 0;
				if (Int32.TryParse(item.Trim(), out value)) {
					points[x] = value;
					x++;
				}
				else {
					chars[y] = item;
					y++;
				}
			}
		}

		string tmpChar = null;
		int tmp = 0;
		bool toSort = false;
		for(int i = 0; i < 4; i++) {
			if(points[i] < points[i + 1]) {
				tmpChar = chars[i + 1];
				tmp = points[i+1];
				chars[i + 1] = chars[i];
				points[i + 1] = points[i];
				chars[i] = tmpChar;
				points[i] = tmp;
				toSort = true;
			}
			if(toSort && i == 3) {
				i = -1; 
				toSort = false;
			}
		}
		int playerPoints;

		playerPoints = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>().CurrentPlayerPoints - (int)(gameCounter/10) - GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>().TotalNumberOfDeaths;
		if(playerPoints < 0) playerPoints = 0;
		if(playerIsDead) {
			int index = 0;
			bool change = false;
			bool updateNeeded = false;
			foreach (int number in points) {
				if(number <= playerPoints) {
					index = points.IndexOf(number);
					change = true;
					updateNeeded = true;
				}
			}

			int nameLength = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>().PlayerName.Length;
			if(nameLength < 10) {
				for(int j = 10 - nameLength; j > 0; j--) {
					GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>().PlayerName += ".";
				}
			}

			if(change) {
				points[index] = playerPoints;
				chars[index] =  GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>().PlayerName;
				change = false;
			}

			int i = 0;
			foreach(string character in chars) {
				textToWrite[i] = character;
				i++;
			}
			foreach(int point in points) {
				textToWrite[i] = point.ToString();
				i++;
			}
			if(updateNeeded)
				WriteFile(file, textToWrite);
			}
	}
	
	private void WriteFile(string file, string[] text) {
		File.WriteAllLines(file, text);
	}
	
	public List<int> GetPlayersPoints() {
		return points;
	}
	
	public List<string> GetPlayersNames() {
		return chars;
	}
}
//////this Must be DONT DESTROY ON LOAD