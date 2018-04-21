using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : MonoBehaviour {
	public GameObject bulletPrefab;
	public GameObject carrierPrefab;
	public GameObject pointer;
	/// Time since we last shot a volley.
	private float lastVolleyTime = 0;
	/// Time (seconds) between volleys. Can change based on difficulty, but must stay the same during a battle.
	/// In-battle changes should be multipliers based on this value.
	private float volleyInterval = 1f;
	
	void Update() {
		if (Time.time - lastVolleyTime > volleyInterval) {
			lastVolleyTime = Time.time;
			Debug.Log("Spawning volley!");
			SpawnBullets(transform.position);
		}
	}

	/// Selects a volley type and spawns bullets.
	private void SpawnBullets(Vector3 spawnpoint) {
		Vector3 direction = pointer.transform.position - spawnpoint;
		GameObject carrier = Instantiate(carrierPrefab, spawnpoint, Quaternion.LookRotation(direction));
		SpawnBurst(carrier, Random.Range(2, 5));
	}

	/// Spawns a shotgun burst of bullets that aim at the pointer, with a spread based on number of bullets.
	private void SpawnBurst(GameObject parent, int count) {
		Vector3 parentDirection = parent.transform.forward;
		int offset = 5;
		int evensOffset = 0;
		if (count % 2 == 0) {
			evensOffset = offset / 2;
		}
		for (int i = 1; i <= count; i++) {
			int angle = offset * (i / 2) + evensOffset;
			Vector3 direction;
			if (i % 2 == 0) {
				direction = Quaternion.Euler(0, angle, 0) * parentDirection;
			} else {
				direction = Quaternion.Euler(0, -angle, 0) * parentDirection;
			}
			GameObject bullet = Instantiate(bulletPrefab, parent.transform.position, Quaternion.LookRotation(direction));
			bullet.transform.SetParent(parent.transform);
		}
	}

	/// Spawns a circle of bullets that aim at the pointer.
	private void SpawnCircle(float radius, int count) {

	}
}
