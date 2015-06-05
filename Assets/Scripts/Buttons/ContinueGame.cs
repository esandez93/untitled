using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ContinueGame : MonoBehaviour{

	void Start (){
		//if(SaveManager.Instance.getSavegames().Count > 0) {
			try{
				if (SaveManager.Instance.autoLoad())
					Debug.Log("Continue game success");//Application.LoadLevel(Gamestate.instance.map.mapName);			
				else
					Debug.Log("AutoLoad failed.");			
			}
			catch{
				Debug.Log ("Continue failed.");
			}

			this.enabled = false;
		/*}
		else
			this.gameObject.GetComponent<Button>().interactable = false;*/		
	}
}

