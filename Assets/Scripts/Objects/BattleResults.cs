using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BattleResults {
	public Dictionary<string, int> droppedItems;
	public Dictionary<string, float> gainedExp;

	public List<string> players;

	public BattleResults(){
		droppedItems = new Dictionary<string, int>();
		gainedExp = new Dictionary<string, float>();
		players = new List<string>();
	}

	private bool itemExists(string itemName){
		bool res = false;

		foreach(KeyValuePair<string, int> entry in droppedItems){
			if(entry.Key.Equals(itemName)){
				res = true;
			}
		}

		return res;
	}

	private bool playerExists(string playerName){
		/*foreach(KeyValuePair<string, float> entry in gainedExp){
			if(entry.Key.Equals(playerName)){
				res = true;
			}
		}*/

		return players.Contains(playerName);
	}

	public void addDrops(Dictionary<string, int> drops){
		foreach(KeyValuePair<string, int> entry in drops){
			if(!itemExists(entry.Key)){
				droppedItems.Add(entry.Key, entry.Value);
			}
			else{
				droppedItems[entry.Key] = droppedItems[entry.Key] + entry.Value;
			}			
		}
	}

	public void addExp(string playerName, float exp){
		if(!playerExists(playerName)){
			gainedExp.Add(playerName, exp);
			players.Add(playerName);
		}
		else{
			gainedExp[playerName] = gainedExp[playerName] + exp;
		}		
	}

	public int getNumberOfPlayers(){
		return gainedExp.Count;
	}
}
