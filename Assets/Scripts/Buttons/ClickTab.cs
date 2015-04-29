using System;
using System.Collections;
using UnityEngine;

class ClickTab : MonoBehaviour {
	
	public void click(){
		PauseMenuManager.Instance.clickSomeTab(this.gameObject);
	}
}

