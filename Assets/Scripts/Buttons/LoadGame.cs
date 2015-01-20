using UnityEngine;
using System.Collections;

public class LoadGame : MonoBehaviour{

	void Start (){
		try{ 
			#if UNITY_EDITOR
			if(SaveManager.Instance.load()){
				Application.LoadLevel(Gamestate.instance.map.mapName);
			}
			else{
				Debug.Log ("Load game failed.");
			}
			#endif
		}
		catch{
			Debug.Log ("Load game failed.");
		}
		
		this.enabled = false;
	}
}

