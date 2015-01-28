using UnityEngine;
using System.Collections;

public class EnterBattleCheck : MonoBehaviour {

	public static bool enterBattle = false;
	
	public Transform monsterCheck;
	public int monsterLayers;
	
	void FixedUpdate(){
		monsterLayers = 1 << LayerMask.NameToLayer("Enemies");
		enterBattle = Physics2D.OverlapCircle(monsterCheck.position, 0.1f, monsterLayers);

		if(enterBattle){
			Singleton.Instance.playerPositionInMap = this.transform.parent.position;
			SaveManager.Instance.autoSave();
			Application.LoadLevel("forestBattle");
		}
	}
}
