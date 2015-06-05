using UnityEngine;
using System.Collections;

public class LoadGame : MonoBehaviour{

	void Start (){
		PauseMenuManager.Instance.showLoadMenuData();
		/*try{ 
			if(SaveManager.Instance.load()){
				Application.LoadLevel(Gamestate.instance.map.mapName);
			}
			else{
				Debug.Log ("Load game failed.");
			}
		}
		catch{
			Debug.Log ("Load game failed.");
		}
		
		this.enabled = false;*/

	}
}

