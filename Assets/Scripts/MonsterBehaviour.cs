using UnityEngine;
using System.Collections;

public class MonsterBehaviour : MonoBehaviour {

	private static int COMBAT_DISTANCE = 3;

	public float movementSpeed = 10f;
	Animator animator;
	public bool isTurn = false;
	Monster thisMonster;
	Character enemy;
	public int action;
	public Character objective;
	Rigidbody2D rigidBody;
	Vector2 initialPosition;
	Vector2 objectivePosition;


	public animationState currentAnimationState;

	public enum animationState{
		MOVING, ATTACKING, SKILL, MOVINGBACK, STANDING, RECEIVINGDAMAGE, DYING
	}

	public bool isAttacking = false;
	public bool startReceivingDamage = false;
	public bool startDeath = false;


	public bool moving = true;

	void Start () {
		thisMonster = GetComponent<Monster>();

		animator = GetComponent<Animator>();
		rigidBody = GetComponent<Rigidbody2D>();

		initialPosition = transform.position;
		currentAnimationState = animationState.STANDING;
	}

	void Update () {		
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
			case animationState.SKILL:

				break;		
			case animationState.MOVINGBACK: 
				backToPosition();
				break;
			case animationState.RECEIVINGDAMAGE:
				if(startReceivingDamage)
					receiveDamage();			
				else
					stand ();		
				break;
			case animationState.DYING:
				if(startDeath)
					die();			
				else
					BattleManager.Instance.deathFinished = true;
				break;
		}
	}
	
	private void goToObjective(){		
		if(transform.position.x > (objectivePosition.x + COMBAT_DISTANCE)){
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
		if(isAttacking)
			animator.SetInteger("AnimationState", Animations.ATTACK);		
		if(!isAttacking){
			thisMonster.basicAttack(enemy);
			BattleManager.Instance.setGUIPlayerInfo(enemy);
			changeAnimationState(animationState.MOVINGBACK);
		}
	}

	private void backToPosition(){
		if(transform.position.x < initialPosition.x){
			animator.SetInteger("AnimationState", Animations.MOVE_BACK);
			changeAnimationState(animationState.MOVINGBACK);

			Vector2 direction = initialPosition - (Vector2)transform.position;
			rigidbody2D.velocity = new Vector2 (movementSpeed, direction.y*5);
		}
		else{
			BattleManager.Instance.changePhase(BattleManager.BattlePhases.DOACTION);
			BattleManager.Instance.attackFinished = true;
			BattleManager.Instance.setGUIPlayerInfo(enemy);
			enemy = null;
			stand();
		}
	}

	private void stand(){
		rigidbody2D.velocity = new Vector2 (0,0);
		changeAnimationState(animationState.STANDING);
		animator.SetInteger("AnimationState", Animations.STAND);
	}

	public void basicAttack(Character player){
		enemy = player;
		objectivePosition = enemy.body.position;

		if(!BattleManager.Instance.attackStarted){
			changeAnimationState(animationState.MOVING);
			//initialPosition = rigidbody2D.position;
		}

		BattleManager.Instance.attackStarted = true;
	}

	public void receiveDamage(){
		if(startReceivingDamage){
			animator.SetInteger("AnimationState", Animations.RECEIVE_DAMAGE);
			changeAnimationState(animationState.RECEIVINGDAMAGE);
		}
	}

	public void die(){
		//if(startDeath){
			animator.SetInteger("AnimationState", Animations.DIE);
			changeAnimationState(animationState.DYING);
		//}
	}

	private void changeAnimationState(animationState state){
		if(currentAnimationState != state){
			//Debug.Log("Changing state - Previous: " + currentAnimationState + ", actual: " + state);
			currentAnimationState = state;
		}		
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
		BattleManager.Instance.finishCurrentAttack();
		animator.enabled = false;
		//Debug.Log("FINISH DEATH");
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
