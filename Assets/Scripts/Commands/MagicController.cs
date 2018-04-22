using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicController : CommandObjectController {
	public GameObject magicObj;
	public GameObject glowObj;
	public GameObject whiteGlowObj;
	private Rigidbody body;
	private Rigidbody glowBody;
	private Rigidbody whiteGlowBody;
	private SpriteRenderer magicSprite;
	private SpriteRenderer glowSprite;
	private SpriteRenderer whiteSprite;
	private bool activated = false;
	private bool despawning = false;
	private float despawnStartTime;
	private float despawnDuration = 0.25f;

	void Start() {
		magicSprite = magicObj.GetComponent<SpriteRenderer>();
		glowSprite = glowObj.GetComponent<SpriteRenderer>();
		whiteSprite = whiteGlowObj.GetComponent<SpriteRenderer>();

		body = GetComponent<Rigidbody>();
		glowBody = glowObj.GetComponent<Rigidbody>();
		glowBody.angularVelocity = -fastSpin;
		glowObj.SetActive(false);
		whiteGlowBody = whiteGlowObj.GetComponent<Rigidbody>();
		whiteGlowBody.angularVelocity = -slowSpin;
		body.angularVelocity = slowSpin;
	}

	void Update() {
		if (despawning) {
			float ratio = (Time.time - despawnStartTime) / despawnDuration;
			if (ratio > 1) {
				ratio = 1;
				Destroy(this.gameObject, 0.1f);
			}

			Color color = magicSprite.color;
			color.a = 1 - ratio;
			magicSprite.color = color;

			color = glowSprite.color;
			color.a = 1 - ratio;
			glowSprite.color = color;

			color = whiteSprite.color;
			color.a = 1 - ratio;
			whiteSprite.color = color;
		}
	}
	
	void OnTriggerEnter(Collider other) {
		if (other.tag == "Pointer" && !activated && !despawning) {
			body.angularVelocity = fastSpin;
			glowObj.SetActive(true);
			whiteGlowObj.SetActive(false);
			activated = true;
			gameController.magicActivation(this);
		}
	}

	public override void despawn() {
		if (!despawning) {
			despawning = true;
			glowObj.GetComponent<RainbowSprite>().shouldChange = false;
			despawnStartTime = Time.time;
		}
	}
}
