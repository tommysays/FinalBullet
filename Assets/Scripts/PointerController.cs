using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointerController : MonoBehaviour {
	private float speed = 500f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		float adjustedSpeed = Time.deltaTime * speed;
		float x = Input.GetAxis("Horizontal") * adjustedSpeed;
		float y = Input.GetAxis("Vertical") * adjustedSpeed;
		transform.Translate(x, 0, y);
	}
}
