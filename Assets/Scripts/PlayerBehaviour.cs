﻿using UnityEngine;
using System.Collections;

public class PlayerBehaviour : MonoBehaviour {

	private static int COMBAT_DISTANCE = 2;

	public float movementSpeed = 10f;
	Animator animator;
	public bool isTurn = false;
	Player thisPlayer;
	Character enemy;
	public int action;
	public Character objective;
	Vector2 initialPosition;
	Vector2 objectivePosition;
	public string skillName;
		
	public AudioSource hitSound;

	public animationState currentAnimationState;

	public enum animationState{
		MOVING, ATTACKING, USINGSKILL, BLOCKING, MOVINGBACK, STANDING, RECEIVINGDAMAGE, DYING, RUNNING
	}

	public bool isAttacking = false;
	public bool isBlocking = false;
	public bool startReceivingDamage = false;
	public bool startDeath = false;

	public bool moving = true;

	void Start () {
		loadSounds();

		thisPlayer = GetComponent<Player>();

		animator = GetComponent<Animator>();

		initialPosition = transform.position;
		currentAnimationState = animationState.STANDING;
	}

	public void loadSounds() {
		this.hitSound = this.gameObject.AddComponent<AudioSource>();

		AudioClip audio = Resources.Load<AudioClip>("Sounds/Effects/sword_slash");

		if (audio != null){
			this.hitSound.minDistance = 10f;
			this.hitSound.maxDistance = 10f;
			this.hitSound.loop = false;
			this.hitSound.clip = audio;
		}
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
			case animationState.USINGSKILL:
				skill();
				break;
			case animationState.BLOCKING:
				defend();
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
			case animationState.RUNNING:
				run();
				break;
		}
	}
	
	private void goToObjective(){		
		if(transform.position.x < (objectivePosition.x - COMBAT_DISTANCE)){
			animator.SetInteger("AnimationState", Animations.MOVE);
			changeAnimationState(animationState.MOVING);

			Vector2 direction = objectivePosition - (Vector2)transform.position;
			rigidbody2D.velocity = new Vector2 (+movementSpeed, direction.y);
		}
		else{
			rigidbody2D.velocity = new Vector2 (0,0);
			changeAnimationState(animationState.ATTACKING);
			isAttacking = true;
		}
	}

	private void attack(){
		if(isAttacking) {
			animator.SetInteger("AnimationState", Animations.ATTACK);		
			this.hitSound.Play();
		}
		else{
			thisPlayer.basicAttack(enemy);
			BattleManager.Instance.setGUIPlayerInfo(enemy);

			if(moving)
				changeAnimationState(animationState.MOVINGBACK);				
			else
				changeAnimationState(animationState.STANDING);			
		}
	}

	private void block() {		
		animator.SetInteger("AnimationState", Animations.BLOCK);
		changeAnimationState(animationState.BLOCKING);	
	}

	private void skill(){
		if(isAttacking)
			animator.SetInteger("AnimationState", Animations.USE_SKILL);		
		else{
			thisPlayer.useSkill(skillName, enemy);
			BattleManager.Instance.setGUIPlayerInfo(enemy);

			if(moving)
				changeAnimationState(animationState.MOVINGBACK);			
			else
				changeAnimationState(animationState.STANDING);
		}
	}

	private void backToPosition(){
		if(transform.position.x > initialPosition.x){
			animator.SetInteger("AnimationState", Animations.MOVE_BACK);
			changeAnimationState(animationState.MOVINGBACK);

			Vector2 direction = initialPosition - (Vector2)transform.position;
			rigidbody2D.velocity = new Vector2 (-movementSpeed, direction.y*5);
		}
		else{
			BattleManager.Instance.changePhase(BattleManager.BattlePhases.DOACTION);
			BattleManager.Instance.attackFinished = true;
			BattleManager.Instance.setGUIPlayerInfo(enemy);
			enemy = null;
			stand();
		}		
	}

	public void stand(){
		rigidbody2D.velocity = new Vector2 (0,0);
		changeAnimationState(animationState.STANDING);
		animator.SetInteger("AnimationState", Animations.STAND);
	}

	public void basicAttack(Character objective){
		this.moving = true;

		basicAttack(objective, true);
	}

	public void basicAttack(Character objective, bool moving){
		this.moving = moving;

		this.enemy = objective;
		this.objectivePosition = enemy.body.position;

		if(!BattleManager.Instance.attackStarted){
			if(moving)
				changeAnimationState(animationState.MOVING);			
			else
				changeAnimationState(animationState.ATTACKING);					
		}

		BattleManager.Instance.attackStarted = true;
	}

	public void useSkill(Character objective, string skillName){
		this.moving = true;

		useSkill(objective, skillName, true);
	}

	public void useSkill(Character objective, string skillName, bool moving){

		this.moving = moving;
		this.enemy = objective;
		this.skillName = skillName;
		this.objectivePosition = enemy.body.position;

		if(!BattleManager.Instance.attackStarted){
			if(moving)
				changeAnimationState(animationState.MOVING);			
			else
				changeAnimationState(animationState.USINGSKILL);			
		}

		BattleManager.Instance.attackStarted = true;
	}

	public void receiveDamage(){
		if(startReceivingDamage){
			animator.SetInteger("AnimationState", Animations.RECEIVE_DAMAGE);
			changeAnimationState(animationState.RECEIVINGDAMAGE);
		}
	}

	public void defend() {
		animator.SetInteger("AnimationState", Animations.BLOCK);
		changeAnimationState(animationState.BLOCKING);	
	}

	public void die(){
		animator.SetInteger("AnimationState", Animations.DIE);
		changeAnimationState(animationState.DYING);		
	}

	public void run() {
		animator.SetInteger("AnimationState", Animations.MOVE_BACK);
		changeAnimationState(animationState.RUNNING);
		rigidbody2D.velocity = new Vector2(-movementSpeed, 0);
		
		BattleManager.Instance.attackStarted = true;
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
		animator.SetInteger("AnimationState", Animations.DIE);
		changeAnimationState(animationState.DYING);
	}

	public void finishDeath(){
		startDeath = false;
		//BattleManager.Instance.finishCurrentAttack();
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
		public const int BLOCK = 7;
	}
}
