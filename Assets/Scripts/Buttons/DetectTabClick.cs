using UnityEngine;
using System.Collections;

public class DetectTabClick : MonoBehaviour {

	void Update () {
		detectClick();
	}

	private void detectClick(){
		Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		
		if(collider2D.OverlapPoint(mousePosition)){
			
		}	
	}
}
