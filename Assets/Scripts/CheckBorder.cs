using UnityEngine;
using System.Collections;

public class CheckBorder : MonoBehaviour {

	MonsterMovementPlatform monsterMovement;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D col){
		if(col.tag.Contains("Monster")){
			if(monsterMovement == null){
				monsterMovement = col.gameObject.GetComponent<MonsterMovementPlatform>();
			}

			monsterMovement.enteringBorder();
		}
	}
}
