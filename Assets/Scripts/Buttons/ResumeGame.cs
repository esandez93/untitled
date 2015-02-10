	using UnityEngine;
using System.Collections;

public class ResumeGame : MonoBehaviour{

	private bool active = false;
	private Pause pause;

	void Update(){
		if(active){
			if(pause == null){
				pause = Gamestate.instance.gameObject.GetComponent<Pause>();
			}
			
			pause.togglePause();
			active = false;
		}
	}

	public void activate(){
		active = true;
	}
}

