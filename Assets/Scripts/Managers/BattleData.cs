using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BattleData {
	public static MapInfo map {get; set;}
	public static List<Player> players {get; set;}

	public static void addPlayer(Player player){
		if (players == null){
			players = new List<Player>();
		}

		if(!playerExist(player)){
			players.Add(player);
		}
	}
	
	private static bool playerExist(Player player){
		foreach(Player p in players){
			if(p.name.Equals(player.name)){
				return true;
			}
		}
		
		return false;		
	}
}

