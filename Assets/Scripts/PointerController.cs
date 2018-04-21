using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointerController : MonoBehaviour {
	private Rigidbody body;
	private float speed = 300f;

	void Start () {
		body = GetComponent<Rigidbody>();
	}
	
	void FixedUpdate () {
		float x = Input.GetAxis("Horizontal") * speed;
		float z = Input.GetAxis("Vertical") * speed;
		body.velocity = new Vector3(x, 0, z);
	}
}
