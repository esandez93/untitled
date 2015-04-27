using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BattleResults {
	public Dictionary<string, int> droppedItems;
	public float gainedExp;

	public List<Player> players;

	public BattleResults(){
		droppedItems = new Dictionary<string, int>();
		players = new List<Player>();
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

	private bool playerExists(Player player){
		return players.Contains(player);
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

	public Dictionary<string, int> getDrops(){
		return droppedItems;
	}

	public void addExp(float exp){
		//if(!playerExists(playerName)){
			gainedExp += exp;
			/*players.Add(playerName);
		}
		else{
			gainedExp[playerName] = gainedExp[playerName] + exp;
		}	*/	
	}

	public float getExp(){
		return gainedExp;
	}

	public void addPlayer(Player player){
		if(!playerExists(player)){
			players.Add(player);
		}
	}

	public Player getPlayer(int position){
		return players[position-1];
	}

	public int getNumberOfPlayers(){
		return players.Count;
	}
}
