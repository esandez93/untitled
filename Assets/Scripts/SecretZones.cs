using UnityEngine;
using System.Collections;

public class SecretZones : MonoBehaviour {
	
	void OnTriggerEnter2D(Collider2D col){
		if(col.tag.Equals("Strider") || col.tag.Equals("Gilgamesh") || col.tag.Equals("Ki"))
			col.gameObject.GetComponent<SpriteRenderer>().enabled = false;		
	}

	void OnTriggerExit2D(Collider2D col){
		if(col.tag.Equals("Strider") || col.tag.Equals("Gilgamesh") || col.tag.Equals("Ki"))
			col.gameObject.GetComponent<SpriteRenderer>().enabled = true;		
	}
}
