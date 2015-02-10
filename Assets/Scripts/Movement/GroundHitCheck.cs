using UnityEngine;
using System.Collections;

public class GroundHitCheck : MonoBehaviour {

	public static bool isGrounded = false;

	public bool isGroundedLeft = false;
	public bool isGroundedCenter = false;
	public bool isGroundedRight = false;

	public Transform groundCheck;
	public int groundLayers;
	public float radius = 0.5f;

	public Vector2 vec = new Vector2(0.2f,0);

	void FixedUpdate(){
		/*groundLayers = 1 << LayerMask.NameToLayer ("Ground");
		isGrounded = Physics2D.OverlapCircle(groundCheck.position, radius, groundLayers);*/

		//isGrounded = Physics2D.Raycast(transform.position, -Vector2.up, radius);

		checkGrounded();
		
		isGrounded = grounded();		
	}

	private void checkGrounded(){
		isGroundedLeft = Physics2D.Raycast((Vector2)transform.position - vec, -Vector2.up, radius);
		isGroundedCenter = Physics2D.Raycast(transform.position, -Vector2.up, radius);
		isGroundedRight = Physics2D.Raycast((Vector2)transform.position + vec, -Vector2.up, radius);
	}

	private bool grounded(){
		return isGroundedLeft || isGroundedCenter || isGroundedRight;
	}
}