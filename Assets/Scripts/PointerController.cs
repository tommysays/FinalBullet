using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointerController : MonoBehaviour {
	public GameObject particleObj;
	public GameObject spriteObj;
	public Sprite warriorSprite;
	public Sprite thiefSprite;
	public Sprite mageSprite;
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
}
