using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic; 
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEditor;

public class SaveManager : MonoBehaviour{
	private static string SAVE_PATH = "/Gamedata/Saves";
	private static string AUTOSAVE_PATH = "/Gamedata/Saves";

	private Gamestate gamestate;

	//public GameData data;

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
		//data = new GameData();
		gamestate = Gamestate.instance;
		Debug.Log("SaveManager initialized.");
	}

	public bool save(){
		string savePath = EditorUtility.SaveFilePanel("Save File", Application.dataPath + SAVE_PATH, "savegame", "sav"); 
//		Debug.Log("SavePath: " + savePath);
		
		return saveData(savePath);
	}
	
	public bool load(){
		string openPath = EditorUtility.OpenFilePanel("Open File", Application.dataPath + "/Gamedata/Saves", "sav"); 

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

					//gamestate.setKnight(data.knight);
					//gamestate.setRogue(data.rogue);
					gamestate.setMage(data.mage);
					Singleton.Instance.inventory = data.inventory;
					gamestate.setMap(data.map);

					//Singleton.Instance.playerPositionInMap = data.currentposition;

					//FileManager.Instance.writeToLog(data.mage.toString());
					
					Debug.Log("Game loaded successfully.");
					
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
				SaveData data = new SaveData();
				
				//FileManager.Instance.initialize();

				//data.knight = gamestate.getPlayerData("Knight");//GameObject.Find("Knight").GetComponent<Knight>().getData();
				//data.rogue = gamestate.getPlayerData("Rogue");//GameObject.Find("Rogue").GetComponent<Rogue>().getData();
				data.mage = gamestate.getPlayerData("Mage");//GameObject.Find("Mage").GetComponent<Mage>().getData();
				data.inventory = Singleton.Instance.inventory;
				data.map = gamestate.map;
				//data.currentposition = Singleton.Instance.playerPositionInMap;

				//FileManager.Instance.writeToLog(data.mage.toString());

				Stream file = File.Open(savePath, FileMode.Create);
				BinaryFormatter bf = new BinaryFormatter();
				bf.Binder = new VersionDeserializationBinder();

				bf.Serialize(file, data);
				file.Close();
				
				Debug.Log("Game saved successfully.");

				res = true;
			}
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
			//Knight knight = GameObject.FindWithTag("Knight").GetComponent<Knight>();
			//Rogue rogue = GameObject.FindWithTag("Rogue").GetComponent<Rogue>();
			Mage mage = GameObject.FindWithTag("Mage").GetComponent<Mage>();
			mage.initializePlayer(Player.Job.MAGE);
			MapInfo map = Singleton.Instance.allMaps["Forest"];

			//FileManager.Instance.writeToLog(map.toString());

			gamestate.setMage(mage.getData());
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
