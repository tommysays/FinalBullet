using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageParticleController : MonoBehaviour {
	public GameObject particles;
	public Vector3 start;
	public Vector3 destination;
	private float travelTime = 0.5f;
	private float startTime;
	private const float DEATH_DELAY = 2f;
	
	void Start() {
		start = transform.position;
		startTime = Time.time;
	}

	void Update() {
		float ratio = (Time.time - startTime) / travelTime;
		if (ratio > 1) {
			ratio = 1f;
			particles.GetComponent<ParticleSystem>().Stop();
			Destroy(this.gameObject, DEATH_DELAY);
		}
		transform.position = start * (1 - ratio) + destination * ratio;
	}
	
	public void setColor(Color color) {
		var main = particles.GetComponent<ParticleSystem>().main;
		main.startColor = color;
	}
}
