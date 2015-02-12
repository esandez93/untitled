using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class getSkillButtonText : MonoBehaviour {
	
	private bool clicked = false;
	private BattleManager bm;
	
	void Update () {
		if(clicked){
			clickButton ();
		}
	}
	
	public void clickButton(){
		if(bm == null){
			bm = GameObject.Find("BattleCanvas").GetComponent<BattleManager>();
		}
		
		bm.setSkillName(getName());
		clicked = false;
	}
	
	public void click (){
		clicked = true;
	}

	private string getName(){
		/*string label = this.GetComponentInChildren<Text>().text;
		string name = label.Remove(label.Length - 6); // Fireball Lv. 1

		return name;*/
		return this.gameObject.GetComponent<Skill>().id;
	}
}
