using UnityEngine;
using System.Collections;

public class NewGame : MonoBehaviour{

	void Start (){
		try{
			if(SaveManager.Instance.loadStartGame()){
				Application.LoadLevel("Forest");
			}
			else{
				Debug.Log ("AutoLoad failed.");
			}
		}
		catch{
			Debug.Log ("New game failed.");
		}
		
		this.enabled = false;
	}
}

