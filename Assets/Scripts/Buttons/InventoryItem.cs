using UnityEngine;
using System.Collections;

public class InventoryItem : MonoBehaviour {

	public void click(){
		PauseMenuManager.Instance.setItemDescription(this.gameObject);
	}
}
