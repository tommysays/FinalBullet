using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// If the player touches a crosshair, the boss takes damage.
public class CrosshairController : CommandObjectController {
	public int damage = 10;
	private Rigidbody body;

	void Start() {
		body = GetComponent<Rigidbody>();
		body.angularVelocity = CommandObjectController.slowSpin;
	}

	void OnTriggerEnter(Collider other) {
		if (other.tag == "Pointer") {
			// TODO Visual indicator that this was picked up and boss gets damaged.
			gameController.crosshairAttack(transform.position, damage);
			body.angularVelocity = CommandObjectController.fastSpin;
			despawn();
		}
	}

	public override void despawn() {
		// TODO
		Destroy(this.gameObject);
	}
}
