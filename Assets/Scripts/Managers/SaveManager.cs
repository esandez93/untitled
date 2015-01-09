using UnityEngine;
using System.Collections; 
using System.Collections.Generic; 
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEditor;

public class SaveManager : MonoBehaviour{
	private static string SAVE_PATH = "/Gamedata/Saves";
	private static string AUTOSAVE_PATH = "/Gamedata/Saves";

	private Gamestate gamestate;

	public GameData data;

	private static SaveManager instance = null;
	public static SaveManager Instance{
		get{
			if (instance == null)
				//instance = (BattleManager)FindObjectOfType(typeof(BattleManager));
				instance = new GameObject("SaveManager").AddComponent<SaveManager>();
			return instance;
		}
	}
	public void initialize(){
		data = new GameData();
		gamestate = Gamestate.instance;
	}

	public void save(){
		string savePath = EditorUtility.SaveFilePanel("Save File", Application.dataPath + SAVE_PATH, "savegame", "sav"); 
		Debug.Log("SavePath: " + savePath);
		
		if (savePath.Length != 0) {
			
			FileManager.Instance.initialize();
			
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(savePath, FileMode.OpenOrCreate);
			
			data.knight = GameObject.Find("Knight").GetComponent<Knight>();;
			data.rogue = GameObject.Find("Rogue").GetComponent<Rogue>();
			data.mage = GameObject.Find("Mage").GetComponent<Mage>();
			data.inventory = Singleton.inventory;
			//data.money = money;
			
			bf.Serialize(file, data);
			file.Close();
			
			Debug.Log("Game saved successfully.");
		}
	}
	
	public void load(){
		string openPath = EditorUtility.OpenFilePanel("Open File", Application.dataPath + "/Gamedata/Saves", "sav"); 
		
		if (openPath.Length != 0) { 
			if(File.Exists(openPath)){
				BinaryFormatter bf = new BinaryFormatter();
				FileStream file = File.Open(openPath, FileMode.Open);
				
				data = (GameData)bf.Deserialize(file);
				
				file.Close();
				
				gamestate.setKnight(data.knight);
				gamestate.setRogue(data.rogue);
				gamestate.setMage(data.mage);
				Singleton.inventory = data.inventory;
				//money = data.money;
				
				Debug.Log("Game loaded successfully.");
			}
		}
	}
	
	public void autoSave(){
		string savePath = Application.dataPath + AUTOSAVE_PATH + "/autosave.asav"; 
		
		if (savePath.Length != 0) {
			
			FileManager.Instance.initialize();
			
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(savePath, FileMode.OpenOrCreate);
			
			data.knight = GameObject.Find("Knight").GetComponent<Knight>();//knight;
			data.rogue = GameObject.Find("Rogue").GetComponent<Rogue>();//rogue;
			data.mage = GameObject.Find("Mage").GetComponent<Mage>();//mage;
			data.inventory = Singleton.inventory;
			data.currentposition = Singleton.playerPositionInMap;
			//data.monsterCollided = 
			
			bf.Serialize(file, data);
			file.Close();
			
			Debug.Log("Game saved successfully.");
		}
	}
	
	public bool autoLoad(){
		string openPath = Application.dataPath + AUTOSAVE_PATH + "/autosave.asav"; 
		
		if (openPath.Length != 0) {
			if(File.Exists(openPath)){
				BinaryFormatter bf = new BinaryFormatter();
				FileStream file = File.Open(openPath, FileMode.Open);
				
				data = (GameData)bf.Deserialize(file);
				
				file.Close();
				
				gamestate.setKnight(data.knight);
				gamestate.setRogue(data.rogue);
				gamestate.setMage(data.mage);
				Singleton.inventory = data.inventory;
				Singleton.playerPositionInMap = data.currentposition;
				//money = data.money;
				
				Debug.Log("Game loaded successfully.");

				return true;
			}
		}

		return false;
	}
}

[System.Serializable]
public class GameData {
	public Knight knight;
	public Rogue rogue;
	public Mage mage;
	public Inventory inventory;
	public MapInfo map;
	public Vector2 currentposition;
	public Monster monsterCollided;
}
