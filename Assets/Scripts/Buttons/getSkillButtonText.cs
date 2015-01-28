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
		GameObject.Find("BattleCanvas").GetComponent<BattleManager>().setSkillName(getName());
		clicked = false;
	}
	
	public void click (){
		clicked = true;
	}

	private string getName(){
		string label = this.GetComponentInChildren<Text>().text;

		string name = label.Remove(label.Length - 6); // Fireball Lv. 1

		return name;
	}
}
