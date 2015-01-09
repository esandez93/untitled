using UnityEngine;
using System.Collections;

public class CheckBorder : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D col){
		if(col.tag.Contains("Monster")){
			Debug.Log ("MONSTER IN EDGE");
			col.gameObject.GetComponent<MonsterMovementPlatform>().enteringBorder();
		}
	}
}
