using UnityEngine;
using System.Collections;

public class MonsterMovementPlatform : MonoBehaviour{

	public bool canGetDirection = true;
	public bool inBorder = false;
	public float maxSpeed = 4f;
	bool facingRight = false;

	Animator animator;
	//BoxCollider2D collider;
	
	public animationState currentAnimationState;
	
	public enum animationState{
		MOVING, MOVINGBACK, STANDING, DYING
	}

	void Start () {
		animator = GetComponent <Animator>();
		//collider = GetComponent<BoxCollider2D>();
	}	
	
	void FixedUpdate () {
		float move = getDirection();			
		
		if (move != 0) {
			changeAnimationState(animationState.MOVING);
			animator.SetInteger("AnimationState", Animations.MOVE);
			rigidbody2D.velocity = new Vector2 (move * maxSpeed, rigidbody2D.velocity.y);
		} 
		else 
			stand();		
	}
	
	void Update(){
		//Debug.Log ("Can get direction: " + canGetDirection);
	}

	private float getDirection(){
		if(canGetDirection){
			if(isInBorder() && facingRight)
				return -1;			
			else if(isInBorder() && !facingRight)
				return 1;			
			else if(!isInBorder() && facingRight)
				return 1;			
			else if(!isInBorder() && !facingRight)
				return -1;			
			else
				return 0;			
		}
		else
			return 0;		
	}

	private void stand(){
		rigidbody2D.velocity = new Vector2 (0,0);
		changeAnimationState(animationState.STANDING);
		animator.SetInteger("AnimationState", Animations.STAND);
	}

	private void changeAnimationState(animationState state){
		currentAnimationState = state;
	}

	private bool isInBorder(){
		return inBorder;
	}

	public void enteringBorder(){
		canGetDirection = false;
		inBorder = true;
		stand ();
	}

	public void exitingBorder(){
		inBorder = false;
		Flip();
		canGetDirection = true;
	}
	
	void Flip(){
		facingRight = !facingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	public class Animations{
		public const int STAND = 0;
		public const int MOVE = 1;
	}
}

