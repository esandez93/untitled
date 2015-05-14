using UnityEngine;
using System.Collections;

public class CommonMovementPlatform : MonoBehaviour
{	
	//This will be our maximum speed as we will always be multiplying by 1
	private float maxSpeed = 4f;
	//a boolean value to represent whether we are facing left or not
	bool facingRight = true;
	//a value to represent our Animator
	Animator anim;
	//to check ground and to have a jumpforce we can change in the editor
	bool isGrounded = false;
//	float groundRadius = 1f;
	private float jumpForce = 625f;
	
	// Use this for initialization
	void Start () {
		//anim = GetComponent <Animator>();		
	}	
	
	void FixedUpdate () {
		isGrounded = GroundHitCheck.isGrounded;	
		
		float move = Input.GetAxis("Horizontal");

		rigidbody2D.velocity = new Vector2 (move * maxSpeed, rigidbody2D.velocity.y);	

		if (move < 0 && facingRight) {			
			Flip ();
		} else if (move > 0 && !facingRight) {
			Flip ();
		}
	}
	
	void Update(){
		if(isGrounded && Input.GetButtonDown("Jump")){
			rigidbody2D.AddForce (new Vector2(0,jumpForce));
		}		
	}
	
	void Flip(){
		facingRight = !facingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
}