using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class getSkillButtonText : MonoBehaviour {
	
	bool clicked = false;
	
	void Update () {
		if(clicked){
			clickButton ();
		}
	}
	
	public void clickButton(){
		string[] skillButtonText = this.GetComponentInChildren<Text>().text.Split(' ');
		GameObject.Find("BattleCanvas").GetComponent<BattleManager>().setSkillName(skillButtonText[0]);
		clicked = false;
	}
	
	public void click (){
		clicked = true;
	}
}
