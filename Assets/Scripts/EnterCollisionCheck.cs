using UnityEngine;
using System.Collections;

public class EnterCollisionCheck : MonoBehaviour {

	public static bool enterBattle = false;
	public static bool interactable = false;
	
	public Transform collideCheck;

	public int monsterLayers;
	public Collider2D enemyCollider;

	public int interactableLayers;
	public Collider2D interactableCollider;

	public Chest chest;
	
	void Update(){
		monsterLayers = 1 << LayerMask.NameToLayer("Enemies");
		enterBattle = Physics2D.OverlapCircle(collideCheck.position, 0.1f, monsterLayers);
		enemyCollider = Physics2D.OverlapCircle(collideCheck.position, 0.2f, monsterLayers);

		interactableLayers = 1 << LayerMask.NameToLayer("Interactable");
		interactable = Physics2D.OverlapCircle(collideCheck.position, 0.5f, interactableLayers);
		interactableCollider = Physics2D.OverlapCircle(collideCheck.position, 0.5f, interactableLayers);

		if(enterBattle){
			if(enemyCollider != null){				
				GameObject monsterCollided = enemyCollider.gameObject;
				Gamestate.instance.bossBattle = monsterCollided.tag.Equals("Boss");
				Gamestate.instance.disableEnemy(monsterCollided);
			}

			Singleton.Instance.playerPositionInMap = this.transform.parent.position;

			Application.LoadLevel("forestBattle");			
		}

		if(interactable){
			if(interactableCollider != null){				
				GameObject interactableCollided = interactableCollider.gameObject;
				
				switch(interactableCollided.tag){
					case Interactable.CHEST:
						chest = interactableCollided.GetComponent<Chest>();

						if(Input.GetButtonDown("Action") && !chest.isOpened()){
							chest.open();
						}
						break;

				}
			}
		}
		else{
			nullInteractables();
		}
	}

	private void nullInteractables(){
		chest = null;
	}

	private class Interactable{
		public const string CHEST = "Chest";
	}
}
