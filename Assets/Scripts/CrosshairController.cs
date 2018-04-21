using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// If the player touches a crosshair, the boss takes damage.
public class CrosshairController : MonoBehaviour {
	public int damage = 10;

	void OnTriggerEnter(Collider other) {
		if (other.tag == "Pointer") {
			Debug.Log("Player damaged the boss for " + damage + " damage!");
			// TODO Actually damage the boss.
			// TODO Visual indicator that player attacked boss.
			Destroy(this.gameObject);
		}
	}
}
