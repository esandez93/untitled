using UnityEngine;
using System.Collections;

public class ClickCredits : MonoBehaviour{

	public void clickCredits() {
		PauseMenuManager.Instance.showCredits();
	}

	public void loadCredits() {
		this.gameObject.GetComponent<Credits>();
	}
}

