using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class getItemButtonText : MonoBehaviour {

	bool clicked = false;

	void Update () {
		if(clicked){
			clickButton ();
		}
	}

	public void clickButton(){
		GameObject.Find("BattleCanvas").GetComponent<BattleManager>().setItemName(getName());
		clicked = false;
	}

	public void click (){
		clicked = true;
	}

	private string getName(){
		string[] label = this.GetComponentInChildren<Text>().text.Split(' ');
		string name = "";

		for(int i = 0; i < label.Length-1; i++){
			name += label[i];
		}

		return name;
	}
}
