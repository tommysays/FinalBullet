using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleController : MonoBehaviour {
	private ParticleSystem particles;
	private float lifetime = 1f;
	private float deathtime = 5f;
	private float startTime;

	// Use this for initialization
	void Start () {
		particles = GetComponent<ParticleSystem>();
		print(particles == null);
	}

	void Update() {
		float delta = Time.time - startTime;
		if (delta > lifetime) {
			particles.Stop();
			Destroy(this.gameObject, deathtime);
		}
	}
	
	public void play() {
		particles.Play();
		startTime = Time.time;
	}
	public void setColor(Color color) {
		particles = GetComponent<ParticleSystem>();
		if (particles == null) {
			Debug.Log("alskjdf");
		}
		var main = particles.main;
		main.startColor = color;
	}
}
