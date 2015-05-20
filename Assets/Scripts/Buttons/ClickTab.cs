using System;
using System.Collections;
using UnityEngine;

class ClickTab : MonoBehaviour {
	
	public void click(){
		PauseMenuManager.Instance.clickSomeTab(this.gameObject);
	}

	public void clickSkillTab(){
		PauseMenuManager.Instance.showCurrentSkills(this.gameObject.transform.parent.gameObject.tag);
	}
}

