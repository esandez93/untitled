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
		if(col.tag.Contains("Monster") || col.tag.Equals("Boss")){
			col.gameObject.GetComponent<MonsterMovementPlatform>().enteringBorder();
		}
	}
}
