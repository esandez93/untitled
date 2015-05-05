using System;
using System.Collections;
using UnityEngine;
using System.Linq;

class UpdateStat : MonoBehaviour {
	
	public void click(){
		string statName = this.gameObject.transform.parent.name;
		string target = GameObject.Find("Gamestate/PauseMenuCanvas/Body/StatusBody/Target").GetComponent<Player>().characterName;
		//Gamestate.instance.getPlayerData(target);
		//foreach (Player p in Gamestate.instance.players)
			//Debug.Log(p.characterName + " - " + p.name);

		//Player pl = Gamestate.instance.players.Where<Player>(x => x.characterName == target).FirstOrDefault();//.statUp(statName);
		//Debug.Log(pl.name);
		Player player = PauseMenuManager.Instance.getPlayer(target);
		player.statUp(statName);
		Gamestate.instance.addPlayerData(player.getData());

		PauseMenuManager.Instance.showStatus();		
	}
}

