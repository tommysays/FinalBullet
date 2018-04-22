using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombController : CommandObjectController {
	public GameObject fuseObj;
	public int damage = 30;
	private Rigidbody fuseBody;
	private bool isLit = false;
	private float fuseStart;
	private float fuseDelay = 1f;

	// Use this for initialization
	void Start() {
		fuseBody = fuseObj.GetComponent<Rigidbody>();
	}

	void Update() {
		if (isLit && !despawning) {
			float delta = Time.time - fuseStart;
			if (delta > fuseDelay) {
				gameController.bombAttack(transform.position, damage);
				despawn();
			}
		}
	}

	void OnTriggerEnter(Collider other) {
		if (other.tag == "Pointer" && !despawning) {
			isLit = true;
			fuseStart = Time.time;
			fuseObj.SetActive(true);
			fuseBody.angularVelocity = CommandObjectController.fastSpin;
		}
	}

	void OnTriggerExit(Collider other) {
		if (other.tag == "Pointer" && !despawning) {
			isLit = false;
			fuseObj.SetActive(false);
		}
	}

	public override void despawn() {
		// TODO
		if (!despawning) {
			despawning = true;
		}
		Destroy(this.gameObject);
	}
}
