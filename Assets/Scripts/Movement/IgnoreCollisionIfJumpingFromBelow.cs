using UnityEngine;
using System.Collections;

public class IgnoreCollisionIfJumpingFromBelow : MonoBehaviour {
	/*int inPlatform = 0;
	GameObject colliders;
	
	void Start () {
		// Colliders are in childobject named Colliders
		colliders = GameObject.Find(gameObject.name+"/Colliders");
	}
	
	void Update () {
		print (inPlatform);

		Vector2 vel = rigidbody2D.velocity;

		// Change collision layer for go-through platforms
		if (inPlatform <= 0) {
			inPlatform = 0;
			int newLayer = colliders.layer;
			if (vel.y >= 0) {
				// Only set GoThrough layer if upwards speed is slightly under jump speed
				// Prevents bug where player drops through angled platform when walking over it
				if (vel.y > 3.0f){
					newLayer = 9;
				}
			} else {
				newLayer = 0;
			}
			if (colliders.layer != newLayer){
				colliders.layer = newLayer;
			} 
		}
	}
	
	void OnTriggerEnter2D(Collider2D coll) {
		if ((coll.gameObject.layer == 9) && (colliders.layer == 9)) {
			inPlatform++;
		}
	}
	
	
	void OnTriggerExit2D(Collider2D coll) {
		if (coll.gameObject.layer == 9) {
			inPlatform--;
		}
	}*/
}

