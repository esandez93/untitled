using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InventoryUseButton : MonoBehaviour{
	public void click(){
		PauseMenuManager.Instance.clickUseItem(LanguageManager.Instance.getMenuId(GameObject.Find("Gamestate/PauseMenuCanvas/Body/InventoryBody/Description/Item/Name").GetComponent<Text>().text));
	}
}
