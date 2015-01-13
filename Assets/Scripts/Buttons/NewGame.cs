using UnityEngine;
using System.Collections;

public class NewGame : MonoBehaviour{

	void Start (){
		try{
			if(SaveManager.Instance.loadStartGame()){
				Application.LoadLevel(Gamestate.instance.map.mapName);
			}
			else{
				Debug.Log ("New Game failed.");
			}
		}
		catch{
			Debug.Log ("New game failed.");
		}
		
		this.enabled = false;
	}
}

