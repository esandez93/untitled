using System;
using System.Collections;
using UnityEngine;
using System.Linq;

class UpdateSkill : MonoBehaviour {
	
	public void click(){
		string skillName = this.gameObject.transform.parent.name;
		string target = GameObject.Find("Gamestate/PauseMenuCanvas/Body/SkillsBody/Target").GetComponent<Player>().characterName;

		Player player = PauseMenuManager.Instance.getPlayer(target);
		player.skillUp(skillName);
		Gamestate.instance.addPlayerData(player.getData());

		PauseMenuManager.Instance.showSkills();
		PauseMenuManager.Instance.showCurrentSkills(player.getBranch(skillName));
	}
}