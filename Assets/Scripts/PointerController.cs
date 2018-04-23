using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// Controls the dot that the player controls (wasd or arrow keys).
/// Uses a different sprite based on current character.
public class PointerController : MonoBehaviour {
	public GameObject particleObj;
	public GameObject spriteObj;
	public Sprite warriorSprite;
	public Sprite thiefSprite;
	public Sprite mageSprite;
	private Rigidbody body;
	public float speed = 300f;
	private bool controlEnabled = true;

	void Start () {
		body = GetComponent<Rigidbody>();
	}
	
	void FixedUpdate () {
		if (controlEnabled) {
			float x = Input.GetAxis("Horizontal") * speed;
			float z = Input.GetAxis("Vertical") * speed;
			body.velocity = new Vector3(x, 0, z);
		}
	}

	public void setCharacter(CHARACTER_TYPE character) {
		Sprite sprite = warriorSprite;
		Color color = Color.yellow;
		switch (character) {
			case CHARACTER_TYPE.WARRIOR:
				color = Color.yellow;
				sprite = warriorSprite;
				break;
			case CHARACTER_TYPE.THIEF:
				color = Color.green;
				sprite = thiefSprite;
				break;
			case CHARACTER_TYPE.MAGE:
				color = Color.blue;
				sprite = mageSprite;
				break;
		}
		spriteObj.GetComponent<SpriteRenderer>().sprite = sprite;
		var main = particleObj.GetComponent<ParticleSystem>().main;
		main.startColor = color;
	}

	public void stop() {
		controlEnabled = false;
		body.velocity = Vector3.zero;
	}

	public void start() {
		controlEnabled = true;
	}
}
