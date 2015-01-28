using UnityEngine;
using System.Collections;

public class BattleEncounterManager : MonoBehaviour{

	//Player player;
	bool isGrounded;

	void Start (){

	}	

	void FixedUpdate (){
		isGrounded = GroundHitCheck.isGrounded;	

		if(isGrounded){
			//int rand = Random.Range();
		}
	}
}

