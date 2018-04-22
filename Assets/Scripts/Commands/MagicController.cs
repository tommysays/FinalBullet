using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicController : CommandObjectController {
	public GameObject glowObj;
	private Rigidbody body;
	private Rigidbody glowBody;
	private bool activated = false;

	void Start() {
		body = GetComponent<Rigidbody>();
		glowBody = glowObj.GetComponent<Rigidbody>();
		glowBody.angularVelocity = -fastSpin;
		glowObj.SetActive(false);
	}
	
	void OnTriggerEnter(Collider other) {
		if (other.tag == "Pointer" && !activated) {
			body.angularVelocity = fastSpin;
			glowObj.SetActive(true);
			activated = true;
			gameController.magicActivation(this);
		}
	}
}
