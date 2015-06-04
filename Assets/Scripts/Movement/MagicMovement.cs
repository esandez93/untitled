using UnityEngine;
using System.Collections;

public class MagicMovement : MonoBehaviour {

	string skillName;
	public float movementSpeed = 10f;
	Animator animator;
	//Monster thisMonster;
	Character attacker;
	Character enemy;
	//Player enemy;
	public int action;
	public Character objective;
	Rigidbody2D rigidBody;
	Vector2 objectivePosition;
	Vector2 initialPosition;
	SpriteRenderer spriteRenderer;

	float damage;
	float modifier;
	Skill skill;
	string status;

	public animationState currentAnimationState;
	
	public enum animationState{
		MOVING, HIT
	}
	
	public bool isAttacking = false;
	
	void Start () {
		spriteRenderer = GetComponent<SpriteRenderer>();
		animator = GetComponent<Animator>();
		rigidBody = GetComponent<Rigidbody2D>();

		initialPosition = attacker.body.position;

		transform.position = initialPosition;

		spriteRenderer.enabled = true;
	}
	
	void FixedUpdate () {		
		if(enemy != null){			
			switch(currentAnimationState){
			case animationState.MOVING:
				goToObjective();
				break;
			case animationState.HIT:
				attack();
				break;
			}
		}
	}


	private void goToObjective(){
		if(transform.position.x < (objectivePosition.x - 0.5f)){
			animator.SetInteger("AnimationState", Animations.MOVE);
			changeAnimationState(animationState.MOVING);
		
			Vector2 direction = objectivePosition - (Vector2)transform.position;
			rigidbody2D.velocity = new Vector2 (movementSpeed, direction.y + 1.5f);
		}
		else{
			rigidBody.velocity = new Vector2 (0,0);
			changeAnimationState(animationState.HIT);
			isAttacking = true;
		}
	}
	
	/*private void goToObjective(){

		if(transform.position.x > (objectivePosition.x)){
			animator.SetInteger("AnimationState", Animations.MOVE);
			changeAnimationState(animationState.MOVING);
			
			Vector2 direction = objectivePosition - (Vector2)transform.position;
			rigidBody.velocity = new Vector2 (-movementSpeed, direction.y);
		}
		else{
			rigidBody.velocity = new Vector2 (0,0);
			changeAnimationState(animationState.HIT);
			isAttacking = true;
		}
	}*/
	
	private void attack(){
		stand ();
		if(isAttacking){
			animator.SetInteger("AnimationState", Animations.HIT);
		}
		if(!isAttacking){
			spriteRenderer.enabled = false;
			enemy.doElementalDamage(damage, modifier, status, skill.chance, false);	
			enemy.showInfo();
			this.gameObject.SetActive(false);
		}
	}

	private void stand(){
		rigidBody.velocity = new Vector2 (0,0);
		changeAnimationState(animationState.HIT);
		animator.SetInteger("AnimationState", Animations.HIT);
	}

	public void useMagic(Character attacker, Character enemy, float damage, float modifier, string status, Skill skill){
		//this.skillName = skill.name;
		this.attacker = attacker;
		this.enemy = enemy;
		this.damage = damage;
		this.modifier = modifier;
		this.status = status;
		this.skill = skill;
		objectivePosition = enemy.body.position;

		//if(BattleManager.attackStarted){
			changeAnimationState(animationState.MOVING);
		//}
	}
	
	private void changeAnimationState(animationState state){
		currentAnimationState = state;
	}
	
	public void attackFinished(){
		isAttacking = false;
	}	
	
	public class Animations{
		public const int MOVE = 0;
		public const int HIT = 1;
	}
}
