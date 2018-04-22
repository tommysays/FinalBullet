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
				gameController.healPlayer(healthPerTick);
				maxHealAmount -= healthPerTick;
				if (maxHealAmount < 0) {
					// TODO Visual indication that the potion is spent.
					Destroy(this.gameObject);
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
}