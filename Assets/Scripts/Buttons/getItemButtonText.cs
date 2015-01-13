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
		string[] itemButtonText = this.GetComponentInChildren<Text>().text.Split(' ');
		GameObject.Find("BattleCanvas").GetComponent<BattleManager>().setItemName(itemButtonText[0]);
		clicked = false;
	}

	public void click (){
		clicked = true;
	}
}
