using System;
using System.Collections;
using UnityEngine;

class ClickButton : MonoBehaviour {
	bool clicked = true;
	
	public void click(){
		clicked = !clicked;
		PauseMenuManager.Instance.clickSomeButton(this.gameObject);
	}

	public void clickSave(){
		if(clicked)
			PauseMenuManager.Instance.clickSave(this.gameObject);
	}

	public void clickLoad(){
		if(clicked)
			PauseMenuManager.Instance.clickLoad(this.gameObject);
	}
}

