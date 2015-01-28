using UnityEngine;
using System.Collections;

public class ContinueGame : MonoBehaviour{

	void Start (){
		try{
			if(SaveManager.Instance.autoLoad()){
				Application.LoadLevel(Gamestate.instance.map.mapName);
			}
			else{
				Debug.Log ("AutoLoad failed.");
			}
		}
		catch{
			Debug.Log ("Continue failed.");
		}

		this.enabled = false;
	}
}

