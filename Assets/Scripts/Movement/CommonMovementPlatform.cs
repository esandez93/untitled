using UnityEngine;
using System.Collections;

public class CommonMovementPlatform : MonoBehaviour {	
	private float maxSpeed = 4f;
	bool facingRight = true;
	Animator anim;
	bool isGrounded = false;
	private float jumpForce = 625f;
	private bool startLanding = false;
	
	void Start () {
		anim = GetComponent <Animator>();		
	}	
	
	void FixedUpdate () {
		isGrounded = GroundHitCheck.isGrounded;	
		
		float move = Input.GetAxis("Horizontal");

		rigidbody2D.velocity = new Vector2 (move * maxSpeed, rigidbody2D.velocity.y);	
		
		if (move < 0 && facingRight) 
			Flip ();		
		else if (move > 0 && !facingRight) 
			Flip ();		

		if(rigidbody2D.velocity.x == 0 || (anim.GetInteger("AnimationState") == Animations.FALL && isGrounded))
			anim.SetInteger("AnimationState", Animations.STAND);
		else
			anim.SetInteger("AnimationState", Animations.MOVE);
	}
	
	void Update(){
		if(isGrounded && Input.GetButtonDown("Jump")){
			Debug.Log("JUMPING");
			rigidbody2D.AddForce (new Vector2(0,jumpForce));
		}

		if(!isGrounded && rigidbody2D.velocity.y > 0)
			anim.SetInteger("AnimationState", Animations.JUMP);
		if(!isGrounded && rigidbody2D.velocity.y <= 0)
			anim.SetInteger("AnimationState", Animations.FALL);
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
		public const int JUMP = 2;
		public const int FALL = 3;
		public const int LAND = 4;
	}
}