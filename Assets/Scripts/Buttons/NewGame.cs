using UnityEngine;
using System.Collections;

public class NewGame : MonoBehaviour{

	void Start (){
		try{			
			GameObject optionsCanvas = GameObject.Find("Gamestate/OptionsCanvas");
			if (optionsCanvas != null)
				optionsCanvas.SetActive(false);

			GameObject loadCanvas = GameObject.Find("Gamestate/LoadGameCanvas");
			if (loadCanvas != null)
				loadCanvas.SetActive(false);

			if(SaveManager.Instance.loadStartGame()){
				Time.timeScale = 1;
				/*Gamestate.instance.playersData[0].matk = 150; // DEBUG
				Gamestate.instance.playersData[0].agi = 150; // DEBUG*/
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

