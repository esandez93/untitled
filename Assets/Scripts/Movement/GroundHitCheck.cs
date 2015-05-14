using UnityEngine;
using System.Collections;

public class GroundHitCheck : MonoBehaviour {

	public static bool isGrounded = false;

	public bool isGroundedLeft = false;
	public bool isGroundedCenter = false;
	public bool isGroundedRight = false;

	public Transform groundCheck;
	public int groundLayers;
	public float radius = 1f;

	public Vector2 vec = new Vector2(0.05f,0);

	void FixedUpdate(){
		/*groundLayers = 1 << LayerMask.NameToLayer ("Ground");
		isGrounded = Physics2D.OverlapCircle(groundCheck.position, radius, groundLayers);*/

		//isGrounded = Physics2D.Raycast(transform.position, -Vector2.up, radius);

		checkGrounded();
		
		isGrounded = grounded();		
	}

	private void checkGrounded(){
		bool left = Physics2D.Raycast((Vector2)transform.position - vec, -Vector2.up, radius);
		bool center = Physics2D.Raycast(transform.position, -Vector2.up, radius);
		bool right = Physics2D.Raycast((Vector2)transform.position + vec, -Vector2.up, radius);

		if (left)
			isGroundedLeft = Physics2D.Raycast((Vector2)transform.position - vec, -Vector2.up, radius).transform.gameObject.tag.Equals("Ground");
		else
			isGroundedLeft = false;

		if (center)
			isGroundedCenter = Physics2D.Raycast(transform.position, -Vector2.up, radius).transform.gameObject.tag.Equals("Ground");
		else
			isGroundedCenter = false;

		if (right)
			isGroundedRight = Physics2D.Raycast((Vector2)transform.position + vec, -Vector2.up, radius).transform.gameObject.tag.Equals("Ground");
		else
			isGroundedRight = false;
	}

	private bool grounded(){
		return isGroundedLeft || isGroundedCenter || isGroundedRight;
	}
}