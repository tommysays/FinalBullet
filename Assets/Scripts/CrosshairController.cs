﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// If the player touches a crosshair, the boss takes damage.
public class CrosshairController : CommandObjectController {
	public int damage = 10;

	void OnTriggerEnter(Collider other) {
		if (other.tag == "Pointer") {
			// TODO Visual indicator that this was picked up and boss gets damaged.
			gameController.crosshairAttack(transform.position, damage);
			Destroy(this.gameObject);
		}
	}
}
