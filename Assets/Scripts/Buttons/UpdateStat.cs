using System;
using System.Collections;
using UnityEngine;
using System.Linq;

class UpdateStat : MonoBehaviour {
	
	public void click(){
		string statName = this.gameObject.transform.parent.name;
		string target = GameObject.Find("Gamestate/PauseMenuCanvas/Body/StatusBody/Target").GetComponent<Player>().characterName;

		Player player = PauseMenuManager.Instance.getPlayer(target);
		player.statUp(statName);
		Gamestate.instance.addPlayerData(player.getData());

		PauseMenuManager.Instance.showStatus();		
	}
}

