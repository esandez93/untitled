using UnityEngine;
using System.Collections;

public class MovementIsometric : MonoBehaviour {

	private Animator animator;
	private Rigidbody2D player;
	private Transform playerTransform;
	
	//private float timer = 0;

	private bool facingRight = true;
	public float walkSpeed = 2.5f;

	// Use this for initialization
	void Start()
	{
		animator = this.GetComponent<Animator>();
		player = this.GetComponent<Rigidbody2D>();
		playerTransform = transform;
	}
		
	// Update is called once per frame
	void FixedUpdate()
	{	
		if (Input.GetKey (KeyCode.RightArrow)) {
			if (!facingRight) {
				facingRight = true;
				Flip ();
			}
			
			animator.SetInteger ("MovementState", 1);
			
			Vector2 curVel = player.velocity;
			curVel.x = Input.GetAxisRaw ("Horizontal") * walkSpeed;
			player.velocity = curVel;
			
		} else if (Input.GetKey (KeyCode.LeftArrow)) {
			if (facingRight) {
				facingRight = false;
				Flip ();
			}
			
			animator.SetInteger ("MovementState", 1);
			
			/*Vector2 curVel = player.velocity;
			curVel.x = Input.GetAxis("Horizontal") * -walkSpeed;
			player.velocity = -curVel;*/
			
			player.velocity = new Vector2 (-walkSpeed, player.velocity.y);
			
		} else if (Input.GetKey (KeyCode.LeftArrow)) {
			if (facingRight) {
				facingRight = false;
				Flip ();
			}
			
			animator.SetInteger ("MovementState", 1);
			
			/*Vector2 curVel = player.velocity;
			curVel.x = Input.GetAxis("Horizontal") * -walkSpeed;
			player.velocity = -curVel;*/
			
			player.velocity = new Vector2 (-walkSpeed, player.velocity.y);
			
		} else if (Input.GetKey (KeyCode.LeftArrow)) {
			if (facingRight) {
				facingRight = false;
				Flip ();
			}
			
			animator.SetInteger ("MovementState", 1);
			
			/*Vector2 curVel = player.velocity;
			curVel.x = Input.GetAxis("Horizontal") * -walkSpeed;
			player.velocity = -curVel;*/
			
			player.velocity = new Vector2 (-walkSpeed, player.velocity.y);
			
		}else {
			animator.SetInteger ("MovementState", 0);			
		}
		
		if ((Input.GetKeyUp (KeyCode.RightArrow)) || (Input.GetKeyUp (KeyCode.LeftArrow))) {
			player.velocity = new Vector3 (0, player.velocity.y);
		}
	}	
	
	void Flip (){		
		Vector3 theScale = playerTransform.localScale;
		theScale.x *= -1;
		playerTransform.localScale = theScale;
	}
}
