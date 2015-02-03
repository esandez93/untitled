using UnityEngine;
using System.Collections;

public class EnterBattleCheck : MonoBehaviour {

	public static bool enterBattle = false;
	
	public Transform monsterCheck;
	public int monsterLayers;

	//public Collider2D[] colliders;
	public Collider2D enemyCollider;
	
	void FixedUpdate(){
		monsterLayers = 1 << LayerMask.NameToLayer("Enemies");
		enterBattle = Physics2D.OverlapCircle(monsterCheck.position, 0.1f, monsterLayers);
		//colliders = Physics2D.OverlapCircleAll(monsterCheck.position, 0.2f, 1 << LayerMask.NameToLayer("Enemies"));
		enemyCollider = Physics2D.OverlapCircle(monsterCheck.position, 0.2f, 1 << LayerMask.NameToLayer("Enemies"));
		
		/*if(colliders != null && colliders.Length > 0){
			foreach(Collider2D collider in colliders){
				Debug.Log(collider.name);
			}
		}*/

		if(enterBattle){
			if(enemyCollider != null){				
				GameObject monsterCollided = enemyCollider.gameObject;
				Gamestate.instance.disableEnemy(monsterCollided);
			}

			Singleton.Instance.playerPositionInMap = this.transform.parent.position;
			SaveManager.Instance.autoSave();
			Application.LoadLevel("forestBattle");
		}
	}
}
