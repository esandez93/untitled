using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic; 
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
//using UnityEditor;

public class SaveManager : MonoBehaviour{
	public static string SAVE_PATH = "/Gamedata/Saves";
	public static string AUTOSAVE_PATH = "/Gamedata/Saves";

	private Gamestate gamestate;

	public bool initialized = false;

	private static SaveManager instance = null;
	public static SaveManager Instance{
		get{
			if (instance == null){				
				//instance = (BattleManager)FindObjectOfType(typeof(BattleManager));
				instance = new GameObject("SaveManager").AddComponent<SaveManager>();
				instance.initialize();
				DontDestroyOnLoad(instance.gameObject);
			}
			return instance;
		}
	}
	public void initialize(){
		if(!initialized){
			gamestate = Gamestate.instance;
			Debug.Log("SaveManager initialized.");
			initialized = true;
		}
	}

	public bool save(){
		string savePath = Application.dataPath + SAVE_PATH + "/savegame.sav";//EditorUtility.SaveFilePanel("Save File", Application.dataPath + SAVE_PATH, "savegame", "sav"); 

		return saveData(savePath);
	}
	
	public bool load(){
		string openPath = Application.dataPath + SAVE_PATH + "/savegame.sav";//EditorUtility.OpenFilePanel("Open File", Application.dataPath + SAVE_PATH, "sav"); 

		return loadData(openPath);
	}
	
	public bool autoSave(){
		string savePath = Application.dataPath + AUTOSAVE_PATH + "/autosave.asav"; 
		
		return saveData(savePath);
	}
	
	public bool autoLoad(){
		string openPath = Application.dataPath + AUTOSAVE_PATH + "/autosave.asav"; 
		
		return loadData(openPath);
	}

	public bool loadData(string openPath){
		bool res = false;

		try{
			if (openPath.Length != 0) {
				if(File.Exists(openPath)){
					SaveData data = new SaveData();

					FileStream file = File.Open(openPath, FileMode.Open);
					BinaryFormatter bf = new BinaryFormatter();

					data = (SaveData)bf.Deserialize(file);
					file.Close();
					
					if(LanguageManager.Instance.compareLanguage(data.language)){ //if current language is different than the SaveData language
						data.translate();
					}

					gamestate.setKnight(data.knight);
					gamestate.setRogue(data.rogue);
					gamestate.setMage(data.mage);
					//Debug.Log(data.inventory.getItems().Count);
					Singleton.Instance.inventory = data.inventory;
					gamestate.setMap(data.map);
					Application.LoadLevel(data.map.mapName);
					PauseManager.Instance.togglePause();
					//gamestate.setPlayerPosition(data.positionInMap);

					//Singleton.Instance.playerPositionInMap = data.currentposition;

					//FileManager.Instance.writeToLog(data.mage.toString());
					
					//Debug.Log("Game loaded successfully.");
					
					res = true;
				}
			}
		}
		catch(Exception e){
			res = false;
			Debug.Log (e.Message);
		}

		return res;
	}

	public bool saveData(string savePath){
		bool res = false;

		try{
			if (savePath.Length != 0) {
				Debug.Log(savePath);
				SaveData data = new SaveData();
				
				//FileManager.Instance.initialize();

				data.knight = gamestate.getPlayerData("Gilgamesh");//GameObject.Find("Knight").GetComponent<Knight>().getData();
				data.rogue = gamestate.getPlayerData("Strider");//GameObject.Find("Rogue").GetComponent<Rogue>().getData();
				data.mage = gamestate.getPlayerData("Ki");//GameObject.Find("Mage").GetComponent<Mage>().getData();
				data.inventory = Singleton.Instance.inventory;
				data.map = gamestate.map;
				data.date = DateTime.Now;
				data.positionInMap = Gamestate.instance.getPlayerPosition();
				//data.currentposition = Singleton.Instance.playerPositionInMap;

				//FileManager.Instance.writeToLog(data.mage.toString());

				Stream file = File.Open(savePath, FileMode.Create);
				BinaryFormatter bf = new BinaryFormatter();
				bf.Binder = new VersionDeserializationBinder();

				bf.Serialize(file, data);
				file.Close();
				
				//Debug.Log("Game saved successfully.");

				res = true;
			}
		}
		catch(Exception e){
			res = false;
			Debug.Log (e.Message);
		}

		return res;
	}

	public bool saveBattleStatus(){
		bool res = false;

		try{
			//gamestate.setMage(gamestate.getPlayerData("Mage"));
			//gamestate.setKnight(GameObject.Find("Knight").GetComponent<Knight>().getData());
			gamestate.setRogue(GameObject.Find("Strider").GetComponent<Rogue>().getData());
		}
		catch(Exception e){
			res = false;
			Debug.Log (e.Message);
		}

		return res;
	
	}

	public bool loadStartGame(){
		bool res = false;

		try{
			Knight knight = GameObject.FindWithTag("Gilgamesh").GetComponent<Knight>();
			Rogue rogue = GameObject.FindWithTag("Strider").GetComponent<Rogue>();
			Mage mage = GameObject.FindWithTag("Ki").GetComponent<Mage>();

			knight.initializePlayer(Player.Job.KNIGHT);
			rogue.initializePlayer(Player.Job.ROGUE);
			mage.initializePlayer(Player.Job.MAGE);

			MapInfo map = Singleton.Instance.allMaps["Forest"];

			//FileManager.Instance.writeToLog(map.toString());

			gamestate.setKnight(knight, knight.getData());
			gamestate.setRogue(rogue, rogue.getData());
			gamestate.setMage(mage, mage.getData());

			gamestate.setMap(map);

			//players.Add(mage);
			
			//BattleData.map = map;
			//BattleData.players = players;

			res = true;
		}
		catch(Exception e){
			Debug.Log (e.ToString());
		}

		return res;
	}
	
	public SaveData getSavegameData(string path){
		SaveData data = new SaveData();

		try{
			if (path.Length != 0) {
				if(File.Exists(path)){					
					FileStream file = File.Open(path, FileMode.Open);
					BinaryFormatter bf = new BinaryFormatter();
					
					data = (SaveData)bf.Deserialize(file);
					file.Close();
					
					//Debug.Log("Game data loaded successfully.");
				}
			}
		}
		catch(Exception e){
			Debug.Log (e.Message);
		}

		return data;
	}

	public List<string> getSavegames(){
		List<string> savegames = FileManager.Instance.getFiles(Application.dataPath + SAVE_PATH, "*.sav");

		return savegames;
	}

	public SaveData getFormattedSavegame(string path){
		SaveData data = getSavegameData(path);
		data.setPath(path);

		if(data.isEmpty()){
			Debug.Log ("The savegame in " + path + " is empty.");
		}

		return data;
	}

	public List<SaveData> getFormattedSavegames(){
		List<string> games = getSavegames();
		List<SaveData> saveData = new List<SaveData>();

		SaveData data = new SaveData();
		foreach(string game in games){
			data = getSavegameData(game);

			if(!data.isEmpty()){
				data.setPath(game);
				saveData.Add(data);
			}
		}

		return saveData;
	}
}

[System.Serializable]
public class GameData {
	//public PlayerData knight;
	//public PlayerData rogue;
	public PlayerData mage;
	public Inventory inventory;
	public MapInfo map;
	//public Vector2 currentposition;
	//public Monster monsterCollided;
}
