using UnityEngine;
using System.Collections;
using System.Runtime.Serialization;

[System.Serializable]
public class SaveData : ISerializable{
	//public PlayerData knight;
	//public PlayerData rogue;
	public PlayerData mage;
	public Inventory inventory;
	public MapInfo map;
	//public Vector2 currentposition;
	//public Monster monsterCollided;

	public SaveData(){

	}

	public SaveData (SerializationInfo info, StreamingContext ctxt) { 
		//Get the values from info and assign them to the appropriate properties 
		
		mage = (PlayerData)info.GetValue("mage", typeof(PlayerData)); 
		inventory = (Inventory)info.GetValue("inventory", typeof(Inventory));	
		map = (MapInfo)info.GetValue("map", typeof(MapInfo));
	}
	
	//Serialization function.

	public void GetObjectData (SerializationInfo info, StreamingContext ctxt){		
		info.AddValue("mage", mage);
		info.AddValue("inventory", inventory);
		info.AddValue("map", map);
	}
}

