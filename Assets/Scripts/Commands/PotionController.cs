using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionController : CommandObjectController {
	public GameObject glowObj;
	public int maxHealAmount = 20;
	public int healthPerTick = 3;
	/// Health regenerates by ticks every healDelay seconds.
	private float healDelay = 0.2f;
	private float touchDuration = 0f;
	private Rigidbody body;
	private Rigidbody glowBody;

	void Start() {
		body = GetComponent<Rigidbody>();
		glowBody = glowObj.GetComponent<Rigidbody>();

		body.angularVelocity = slowSpin;
		glowBody.angularVelocity = -fastSpin;
	}

	void OnTriggerEnter(Collider other) {
		if (other.tag == "Pointer") {
			body.angularVelocity = fastSpin;
			glowObj.SetActive(true);
		}
	}

	void OnTriggerStay(Collider other) {
		if (other.tag == "Pointer") {
			touchDuration += Time.deltaTime;
			if (touchDuration > healDelay) {
				touchDuration = 0f;
				maxHealAmount -= healthPerTick;
				if (maxHealAmount < 0) {
					gameController.explode(transform.position, Color.green);
					gameController.healPlayer(healthPerTick * 3);
					despawn();
				} else {
					gameController.healPlayer(healthPerTick);
				}
			}
		}
	}

	void OnTriggerExit(Collider other) {
		if (other.tag == "Pointer") {
			body.angularVelocity = slowSpin;
			glowObj.SetActive(false);
		}
	}

	public override void despawn() {
		// TODO
		Destroy(this.gameObject);
	}
}