using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class GUIManager : MonoBehaviour {

	static GUIManager instance = null;
	static readonly object padlock = new object();
	
	GUIManager() {
	}
	
	public static GUIManager Instance {
		get {
			lock (padlock) {
				if(instance == null) {
					instance = new GUIManager();
				}
				return instance;
			}
		}
	}

	private string playerName = null;
	private bool ruleButtonPressed = false;
	private bool controllsButtonPressed = false;
	private bool recordsButtonPressed = false;
	private float offsetX = 0.0f;
	private float offsetY = 0.0f;
	private int maxPlayerNameLength = 0;
	private Texture2D menuBackground = null;
	private Texture2D rulesTexture = null;
	private Texture2D lifeBarBackground = null;
	private Texture2D lifeBarForeground = null;
	private GameController gameController = null;
	private PlayerStats playerStats = null;
	private Texture2D logoTexture = null;
	private TextMesh pointsText = null;
	private TextMesh timeValue = null; 
	private Texture2D gameOverTexture = null;
	private Texture2D gameFinishTexture = null;
	private GameObject pointsGameObject = null;
	private GameObject timeGameObject = null;
	private Texture2D controlTexture = null;
	private AdvancedController advancedController = null;
	private GameObject playerGameobject = null;
	private GameObject gameControllerGameObject = null;
	private GUISkin mySkin = null;
	private bool hasWon = false;

	public bool HasWon {
		get { return hasWon; }
		set { hasWon = value; }
	}

	void Start() {
		hasWon = false;
		mySkin = Resources.Load("GUISkins/MySkin") as GUISkin;

		maxPlayerNameLength = 10;
		offsetX = 32.25f;
		offsetY = 11.2f;
		playerName = "Gracz";

		playerGameobject = GameObject.FindGameObjectWithTag("Player");
		advancedController = playerGameobject.GetComponent<AdvancedController>();
		gameControllerGameObject = GameObject.FindGameObjectWithTag("GameController");
		gameController = gameControllerGameObject.GetComponent<GameController>();
		pointsGameObject = GameObject.FindGameObjectWithTag("points");
		timeGameObject = GameObject.Find("timeValue");
		playerStats = playerGameobject.GetComponent<PlayerStats>();

		lifeBarForeground = Resources.Load("Textures/MyTextures/roboLifeFG") as Texture2D;
		lifeBarBackground = Resources.Load("Textures/MyTextures/roboLifeBG") as Texture2D;
		rulesTexture = Resources.Load("Textures/MyTextures/Rules") as Texture2D;
		gameFinishTexture = Resources.Load("Textures/MyTextures/congratulations") as Texture2D;
		gameOverTexture = Resources.Load("Textures/MyTextures/gameOver") as Texture2D;
		logoTexture = Resources.Load("Textures/MyTextures/logoTexture") as Texture2D;
		menuBackground = Resources.Load("Textures/MyTextures/MenuBGB") as Texture2D;
		controlTexture = Resources.Load("Textures/MyTextures/Control") as Texture2D;
	}

	void Update() {
		if(timeGameObject != null) {	
			timeValue = timeGameObject.GetComponent<TextMesh>();
			timeValue.text = gameController.GameCounter.ToString();
		}

		if(pointsGameObject != null) {	
			pointsText = pointsGameObject.GetComponent<TextMesh>();
			pointsText.text = playerStats.CurrentPlayerPoints.ToString();
		}
	}

	public void ButtonClicked(string name) {
		if(name == "ExitOption") gameController.ExitApplication();
		if(name == "PlayOption") gameController.LoadNextScene();
		if(name == "RulesOption") ruleButtonPressed = true;
		if(name == "ControllsOption") controllsButtonPressed = true;
		if(name == "RecordsOption") {
			string file = "Records.txt";
			if(File.Exists(file) && new FileInfo(file).Length > 0 ) {
				gameController.ReadFile(file);
			}
			recordsButtonPressed = true;
		}
	}

	void OnGUI() {

		GUI.skin = mySkin;

		#if UNITY_ANDROID
			if(gameController.loadedLevel != GameController.LoadedLevel.MainMenu) {
				if(GUI.Button(new Rect((Screen.width - 70) * 0.5f, (Screen.height - 50) * 0.5f, 70, 50), "JUMP")) {
					advancedController.CheckAndSetCharacterMovingStateJumpingPhone(); 
				}
			}
		#endif

		if(gameController.GetLoadedLevel == GameController.LoadedLevel.MainMenu) {
			gameController.SetGameStatusRunning();
			if(ruleButtonPressed == true)	DrawGameRulesMenu();
			if(recordsButtonPressed == true)	DrawGameRecordsMenu();
			if(controllsButtonPressed == true)	DrawGameControllsMenu();
		}
		else {
			if(!gameController.PlayerIsDead)
				DrawPlayerHealthBar();

			if(gameController.SceneLoadedFirst) {
				gameController.SetGameStatusPaused();
				DrawGamePauseMenu();
			}
			
			if(Input.GetKeyDown("p")) {
				gameController.SceneLoadedFirst = true;
				gameController.SetGameStatusPaused();
				DrawGamePauseMenu();
			}
			
			if(gameController.PlayerIsDead) {
				gameController.SetGameStatusPaused();
				DrawGameOverMenu();
			}

			if(hasWon) {
				gameController.SetGameStatusPaused();
				DrawGameFinish();
			}
			
			else if(!gameController.PlayerIsDead && !gameController.SceneLoadedFirst) 
				gameController.SetGameStatusRunning();
		}
	}
	
	private void DrawPlayerHealthBar() {
		GUI.DrawTexture(new Rect(Screen.width/offsetX, Screen.height/offsetY, 100.0f, 50.0f), lifeBarBackground);
		GUI.DrawTexture(new Rect(Screen.width/offsetX, Screen.height/offsetY, playerStats.CurrentPlayerHealth, 50.0f), lifeBarForeground);
	}
	
	private void DrawMainMenu() {
		GUI.DrawTexture(new Rect((Screen.width - 400) * 0.5f, (Screen.height - 400) * 0.5f, 400, 400), menuBackground);
		if(GUI.Button(new Rect((Screen.width - 70) * 0.5f, (Screen.height - 50) * 0.5f, 70, 50), "Play", mySkin.button) && playerName != null) {
			gameController.GamePaused = false;
			gameController.SceneLoadedFirst = false;
			ruleButtonPressed = false;
			controllsButtonPressed = false;
		}
	}

	private void DrawGameControllsMenu() {
		GUI.DrawTexture(new Rect((Screen.width - 400) * 0.5f, (Screen.height - 400) * 0.5f, 400, 400), menuBackground);
		GUI.DrawTexture(new Rect((Screen.width - 300) * 0.5f, (Screen.height - 300) * 0.5f, 300, 200), controlTexture);
		if(GUI.Button(new Rect((Screen.width - 70) * 0.5f, (Screen.height + 150) * 0.5f, 70, 50), "Powrót", mySkin.button)) {
			gameController.GamePaused = false;
			gameController.SceneLoadedFirst = false;
			ruleButtonPressed = false;
			controllsButtonPressed = false;
		}
	}

	private void DrawGameRulesMenu() {
		GUI.DrawTexture(new Rect((Screen.width - 400) * 0.5f, (Screen.height - 400) * 0.5f, 400, 400), menuBackground);
		GUI.DrawTexture(new Rect((Screen.width - 300) * 0.5f, (Screen.height - 300) * 0.5f, 300, 200), rulesTexture);
		if(GUI.Button(new Rect((Screen.width - 70) * 0.5f, (Screen.height + 150) * 0.5f, 70, 50), "Powrót", mySkin.button)) {
			gameController.GamePaused = false;
			gameController.SceneLoadedFirst = false;
			ruleButtonPressed = false;
			controllsButtonPressed = false;
		}
	}

	private int[] GetPlayerPoints() {
		int[] pointsTab = new int[5];
		int i = 0;
		foreach(int item in gameController.GetPlayersPoints()) {
			pointsTab[i] = gameController.GetPlayersPoints()[i];
			i++;
		}
		return pointsTab;
	}

	private string[] GetPlayerNames() {
		string[] namesTab = new string[5];
		int i = 0;
		foreach(string item in gameController.GetPlayersNames()) {
			namesTab[i] = gameController.GetPlayersNames()[i];
			i++;
		}
		return namesTab;
	}

	private void DrawGameRecordsMenu() {

		GUI.DrawTexture(new Rect((Screen.width - 400) * 0.5f, (Screen.height - 400) * 0.5f, 400, 400), menuBackground);
		GUI.Label(new Rect((Screen.width - 200) * 0.5f, (Screen.height - 250) * 0.5f, 200, 20), GetPlayerNames()[0] + GetPlayerPoints()[0].ToString(), mySkin.label);
		GUI.Label(new Rect((Screen.width - 200) * 0.5f, (Screen.height - 190) * 0.5f, 200, 20), GetPlayerNames()[1] + GetPlayerPoints()[1].ToString(), mySkin.label);
		GUI.Label(new Rect((Screen.width - 200) * 0.5f, (Screen.height - 130) * 0.5f, 200, 20), GetPlayerNames()[2] + GetPlayerPoints()[2].ToString(), mySkin.label);
		GUI.Label(new Rect((Screen.width - 200) * 0.5f, (Screen.height - 70) * 0.5f, 200, 20), GetPlayerNames()[3] + GetPlayerPoints()[3].ToString(), mySkin.label);
		GUI.Label(new Rect((Screen.width - 200) * 0.5f, (Screen.height - 10) * 0.5f, 200, 20), GetPlayerNames()[4] + GetPlayerPoints()[4].ToString(), mySkin.label);

		if(GUI.Button(new Rect((Screen.width - 70) * 0.5f, (Screen.height + 150) * 0.5f, 70, 50), "Powrót", mySkin.button)) {
			gameController.GamePaused = false;
			gameController.SceneLoadedFirst = false;
			ruleButtonPressed = false;
			recordsButtonPressed = false;
		}
	}
	
	private void DrawGamePauseMenu() {
		GUI.DrawTexture(new Rect((Screen.width - 400) * 0.5f, (Screen.height - 400) * 0.5f, 400, 400), menuBackground);
		GUI.DrawTexture(new Rect((Screen.width - 200) * 0.5f, (Screen.height - 300) * 0.5f, 200, 100), logoTexture);

		if(GUI.Button(new Rect((Screen.width - 90) * 0.5f, (Screen.height - 50) * 0.5f, 90, 50), "Graj", mySkin.button)) {
			gameController.GamePaused = false;
			gameController.SceneLoadedFirst = false;
		}

		if(GUI.Button(new Rect((Screen.width - 90) * 0.5f, (Screen.height + 50) * 0.5f, 90, 50), "Restartuj", mySkin.button)) {
			gameController.GamePaused = false;
			gameController.SceneLoadedFirst = false;
			gameController.ReloadCurrentLevel();
			gameController.GameCounter = 0;
		}

		if(GUI.Button(new Rect((Screen.width - 90) * 0.5f, (Screen.height + 150) * 0.5f, 90, 50), "Menu Główne", mySkin.button)) {
			gameController.GamePaused = false;
			gameController.SceneLoadedFirst = false;
			gameController.PlayerIsDead = false;
			gameController.LoadMainMenu();
		}
	}
	
	private void DrawGameOverMenu() {
		GUI.DrawTexture(new Rect((Screen.width - 400) * 0.5f, (Screen.height - 400) * 0.5f, 400, 400), menuBackground);
		GUI.DrawTexture(new Rect((Screen.width - 200) * 0.5f, (Screen.height - 300) * 0.5f, 200, 100), gameOverTexture);

		playerName = GUI.TextField(new Rect((Screen.width - 200) * 0.5f, (Screen.height - 100) * 0.5f, 200, 20), playerName, maxPlayerNameLength, mySkin.textArea);
		playerStats.PlayerName = playerName;

		if(GUI.Button(new Rect((Screen.width - 90) * 0.5f, (Screen.height - 50) * 0.5f, 90, 50), "Restartuj", mySkin.button) && playerName != "Wprowadź nazwę użytkownika...") {
			gameController.SavePlayerRecord(playerName);
			gameController.GamePaused = false;
			gameController.SceneLoadedFirst = false;
			gameController.ReloadCurrentLevel();
		}


		if(GUI.Button(new Rect((Screen.width - 90) * 0.5f, (Screen.height + 50) * 0.5f, 90, 50), "Menu Główne", mySkin.button) && playerName != "Wprowadź nazwę użytkownika...") {
			gameController.SavePlayerRecord(playerName);
			gameController.GamePaused = false;
			gameController.SceneLoadedFirst = false;
			gameController.PlayerIsDead = false;
			gameController.LoadMainMenu();
		}
	}

	private void DrawGameFinish() {
		GUI.DrawTexture(new Rect((Screen.width - 400) * 0.5f, (Screen.height - 400) * 0.5f, 400, 400), menuBackground);
		GUI.DrawTexture(new Rect((Screen.width - 200) * 0.5f, (Screen.height - 300) * 0.5f, 200, 100), gameFinishTexture);
		
		playerName = GUI.TextField(new Rect((Screen.width - 200) * 0.5f, (Screen.height - 100) * 0.5f, 200, 20), playerName, maxPlayerNameLength, mySkin.textArea);
		playerStats.PlayerName = playerName;
		
		if(GUI.Button(new Rect((Screen.width - 90) * 0.5f, (Screen.height - 50) * 0.5f, 90, 50), "Restartuj", mySkin.button) && playerName != "Wprowadź nazwę użytkownika...") {
			gameController.SavePlayerRecord(playerName);
			gameController.GamePaused = false;
			gameController.SceneLoadedFirst = false;
			gameController.ReloadCurrentLevel();
		}
		if(GUI.Button(new Rect((Screen.width - 90) * 0.5f, (Screen.height + 50) * 0.5f, 90, 50), "Menu Główne", mySkin.button) && playerName != "Wprowadź nazwę użytkownika...") {
			gameController.SavePlayerRecord(playerName);
			gameController.GamePaused = false;
			gameController.SceneLoadedFirst = false;
			gameController.PlayerIsDead = false;
			gameController.LoadMainMenu();
		}
	}
}	
