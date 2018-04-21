using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// Bullets can collide with the player to damage them, or collide with a wall to despawn.
/// They don't always move on their own; instead, they may have a Carrier parent that moves for them.
public class BulletController : MonoBehaviour {
	public GameController gameController;
	public int damage = 10;
	void OnTriggerEnter(Collider other) {
		if (other.tag == "Pointer") {
			// TODO Visual indication that player was hit.
			gameController.damagePlayer(damage);
			Destroy(this.gameObject);
		} else if (other.tag == "Wall") {
			Destroy(this.gameObject);
		}
	}
}
