using System;
using System.Collections;
using UnityEngine;

class ChangeTarget : MonoBehaviour {	
	public void click(){
		PauseMenuManager.Instance.changeTarget(this.gameObject.tag);
	}
}