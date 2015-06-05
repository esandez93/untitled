using UnityEngine;
using System;
using System.Collections;
using System.Runtime.Serialization;

[System.Serializable]
public class SaveData : ISerializable{
	public PlayerData knight;
	public PlayerData rogue;
	public PlayerData mage;
	public Inventory inventory;
	public MapInfo map;
	//public Vector2 currentposition;
	//public Monster monsterCollided;
	public int language;
	public DateTime date;
	public float[] positionInMap;

	private string path;

	public SaveData(){

	}

	public SaveData (SerializationInfo info, StreamingContext ctxt) { 
		//Get the values from info and assign them to the appropriate properties 
		
		mage = (PlayerData)info.GetValue("mage", typeof(PlayerData)); 
		knight = (PlayerData)info.GetValue("knight", typeof(PlayerData)); 
		rogue = (PlayerData)info.GetValue("rogue", typeof(PlayerData)); 
		inventory = (Inventory)info.GetValue("inventory", typeof(Inventory));	
		map = (MapInfo)info.GetValue("map", typeof(MapInfo));
		language = (int)info.GetValue("language", typeof(int));
		date = (DateTime)info.GetValue("date", typeof(DateTime));
		positionInMap = (float[])info.GetValue("positionInMap", typeof(float[]));
	}
	
	//Serialization function.

	public void GetObjectData (SerializationInfo info, StreamingContext ctxt){		
		info.AddValue("mage", mage);
		info.AddValue("knight", knight);
		info.AddValue("rogue", rogue);
		info.AddValue("inventory", inventory);
		info.AddValue("map", map);
		info.AddValue("language", language);
		info.AddValue("date", date);
		info.AddValue("positionInMap", positionInMap);
	}

	public bool isEmpty(){
		return rogue == null;
	}

	public void translate(){
		rogue.translate();
	}

	public void setPath(string path) {
		this.path = path;
	}

	public string getPath() {
		return this.path;
	}
}

