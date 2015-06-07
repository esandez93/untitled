using UnityEngine;
using System.Collections;

public class CheckBorder : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D col){
		if(col.tag.Contains("Monster") || col.tag.Equals("Boss")){
			col.gameObject.GetComponent<MonsterMovementPlatform>().enteringBorder();
		}
	}
}
