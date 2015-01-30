	using UnityEngine;
using System.Collections;

public class ResumeGame : MonoBehaviour{

	private bool active = false;

	void Update(){
		if(active){
			Gamestate.instance.gameObject.GetComponent<Pause>().togglePause();
			active = false;
		}
	}

	public void activate(){
		active = true;
	}
}

