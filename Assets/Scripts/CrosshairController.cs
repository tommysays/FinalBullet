using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// If the player touches a crosshair, the boss takes damage.
public class CrosshairController : MonoBehaviour {
	public GameController gameController;
	public int damage = 10;

	void OnTriggerEnter(Collider other) {
		if (other.tag == "Pointer") {
			Debug.Log("Player damaged the boss for " + damage + " damage!");
			// TODO Visual indicator that this was picked up and boss gets damaged.
			gameController.damageBoss(damage);
			Destroy(this.gameObject);
		}
	}
}
