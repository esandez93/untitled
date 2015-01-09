using UnityEngine;
using System.Collections;

public class MonsterBehaviour : MonoBehaviour {

	public float movementSpeed = 10f;
	Animator animator;
	public bool isTurn = false;
	Monster thisMonster;
	Player enemy;
	public int action;
	public Character objective;
	Rigidbody2D rigidBody;
	Vector2 initialPosition;
	Vector2 objectivePosition;

	public animationState currentAnimationState;

	public enum animationState{
		MOVING, ATTACKING, MOVINGBACK, STANDING, RECEIVINGDAMAGE, DYING
	}

	public bool isAttacking = false;
	public bool startReceivingDamage = false;
	public bool startDeath = false;

	void Start () {
		thisMonster = GetComponent<Monster>();

		animator = GetComponent<Animator>();
		rigidBody = GetComponent<Rigidbody2D>();

		initialPosition = rigidBody.position;
		currentAnimationState = animationState.STANDING;
	}

	void FixedUpdate () {		
		//if(enemy != null){			
		switch(currentAnimationState){
		case animationState.STANDING:
			stand();
			break;
		case animationState.MOVING:
			goToObjective();
			break;
		case animationState.ATTACKING:
			attack();
			break;
		case animationState.MOVINGBACK: 
			backToPosition();
			break;
		case animationState.RECEIVINGDAMAGE:
			if(startReceivingDamage){
				receiveDamage();
			}
			else{
				stand ();
				BattleManager.attackFinished = true;
				//BattleManager.finishCurrentAttack();
			}
			break;
		case animationState.DYING:
			if(startDeath){
				die();
			}
			else{
				BattleManager.deathFinished = true;
			}
			/*else{
				stand ();
			}*/
			break;
		}
		//}
	}
	
	private void goToObjective(){		
		if(transform.position.x > (objectivePosition.x + 3)){
			animator.SetInteger("AnimationState", Animations.MOVE);
			changeAnimationState(animationState.MOVING);

			Vector2 direction = objectivePosition - (Vector2)transform.position;
			rigidbody2D.velocity = new Vector2 (-movementSpeed, direction.y);
		}
		else{
			rigidbody2D.velocity = new Vector2 (0,0);
			changeAnimationState(animationState.ATTACKING);
			isAttacking = true;
		}
	}

	private void attack(){
		if(isAttacking){
			animator.SetInteger("AnimationState", Animations.ATTACK);
		}
		if(!isAttacking){
			print (thisMonster.name + " attacked to " + enemy.name);
			thisMonster.basicAttack(enemy);
			BattleManager.setGUIPlayerInfo(enemy);
			changeAnimationState(animationState.MOVINGBACK);
		}
	}

	private void backToPosition(){
		if(transform.position.x < initialPosition.x){
			animator.SetInteger("AnimationState", Animations.MOVE_BACK);
			changeAnimationState(animationState.MOVINGBACK);

			Vector2 direction = initialPosition - (Vector2)transform.position;
			rigidbody2D.velocity = new Vector2 (movementSpeed, direction.y);
		}
		else{
			BattleManager.changePhase(BattleManager.BattlePhases.DOACTION);
			BattleManager.attackFinished = true;
			BattleManager.setGUIPlayerInfo(enemy);
			enemy = null;
			stand();
		}
	}

	private void stand(){
		rigidbody2D.velocity = new Vector2 (0,0);
		changeAnimationState(animationState.STANDING);
		animator.SetInteger("AnimationState", Animations.STAND);
	}

	public void basicAttack(Player player){
		enemy = player;
		objectivePosition = enemy.body.position;

		if(!BattleManager.attackStarted){
			changeAnimationState(animationState.MOVING);
			initialPosition = rigidbody2D.position;
		}

		BattleManager.attackStarted = true;
	}

	public void receiveDamage(){
		if(startReceivingDamage){
			animator.SetInteger("AnimationState", Animations.RECEIVE_DAMAGE);
			changeAnimationState(animationState.RECEIVINGDAMAGE);
		}
	}

	public void die(){
		if(startDeath){
			animator.SetInteger("AnimationState", Animations.DIE);
			changeAnimationState(animationState.DYING);
		}
	}

	private void changeAnimationState(animationState state){
		currentAnimationState = state;
	}

	public void attackFinished(){
		isAttacking = false;
	}	

	public void damageStarted(){
		startReceivingDamage = true;
	}

	public void damageFinished(){
		startReceivingDamage = false;
	}

	public void deathStarted(){
		startDeath = true;
	}

	public void finishDeath(){
		startDeath = false;
		BattleManager.finishCurrentAttack();
		animator.enabled = false;
	}
	
	public class Animations{
		public const int STAND = 0;
		public const int MOVE = 1;
		public const int ATTACK = 2;
		public const int USE_SKILL = 3;
		public const int MOVE_BACK = 4;
		public const int RECEIVE_DAMAGE = 5;
		public const int DIE = 6;
	}
}
