using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

class RunAway : MonoBehaviour {	

	private bool collided = false;

	void Update() {
		if (collided) {
			try {
				Application.LoadLevel(Gamestate.instance.map.mapName);	
			}
			catch (Exception e) {
				Application.LoadLevel("Forest");	
			}
		}
	}

	void OnTriggerEnter2D(Collider2D col){
		collided = true;
	}
}
