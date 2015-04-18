using UnityEngine;
using System.Collections;

public class CraftItem : MonoBehaviour {

	public void click(){
		PauseMenuManager.Instance.setRecipeDescription(this.gameObject);
	}
}
