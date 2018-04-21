using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : MonoBehaviour {
	private GameController gameController;
	public GameObject bulletPrefab;
	public GameObject carrierPrefab;
	public GameObject pointer;
	/// Time since we last shot a volley.
	private float lastVolleyTime = 0;
	/// Time (seconds) between volleys. Can change based on difficulty, but must stay the same during a battle.
	/// In-battle changes should be multipliers based on this value.
	private float volleyInterval = 1f;
	private float bulletSpeed = 250f;
	private float carrierSpeed = 130f;
	private float expandSpeed = 60f;
	/// Spawns an undirected circle every few volleys.
	private int circleCounter = 0;
	private int circleCounterMax = 3;
	
	void Start() {
		gameController = FindObjectOfType<GameController>();
	}

	void Update() {
		if (Time.time - lastVolleyTime > volleyInterval) {
			lastVolleyTime = Time.time;
			SpawnBullets(transform.position);
		}
	}

	/// Selects a volley type and spawns bullets.
	private void SpawnBullets(Vector3 spawnpoint) {
		Vector3 direction = pointer.transform.position - spawnpoint;
		direction = Vector3.Normalize(direction);
		float rand = Random.Range(0f, 1f);
		if (rand < 0.6f) {
			SpawnBurst(spawnpoint, direction, Random.Range(4,8));
		} else {
			SpawnDirectedCircle(spawnpoint, direction, Random.Range(8, 15));
		}
		if (circleCounter++ > circleCounterMax) {
			circleCounter = 0;
			SpawnCircle(spawnpoint, direction, Random.Range(15, 25));
		}
	}

	/// Spawns a shotgun burst of bullets with a spread based on number of bullets.
	/// These bullets don't use a carrier, since they move independent of each other after spawn.
	private void SpawnBurst(Vector3 spawnpoint, Vector3 direction, int count) {
		int offset = 5;
		// TODO Even-numbered bursts are a little off angled.
		float evensOffset = 0;
		if (count % 2 == 0) {
			evensOffset = offset / 2.0f;
		}
		for (int i = 1; i <= count; i++) {
			float angle = offset * (i / 2);
			Vector3 forward;
			if (i % 2 == 0) {
				forward = Quaternion.Euler(0, angle - evensOffset, 0) * direction;
			} else {
				forward = Quaternion.Euler(0, -angle - evensOffset, 0) * direction;
			}
			GameObject bullet = Instantiate(bulletPrefab, spawnpoint, Quaternion.LookRotation(forward));
			bullet.GetComponent<Rigidbody>().velocity = forward * bulletSpeed;
			bullet.GetComponent<BulletController>().gameController = gameController;
		}
	}

	/// Spawns an expanding circle not aimed at anything in particular.
	private void SpawnCircle(Vector3 spawnpoint, Vector3 direction, int count) {
		if (count == 1) {
			GameObject bullet = Instantiate(bulletPrefab, spawnpoint, Quaternion.LookRotation(direction));
			bullet.GetComponent<Rigidbody>().velocity = direction * bulletSpeed;
			bullet.GetComponent<BulletController>().gameController = gameController;
			return;
		}
		if (count == 0) {
			Debug.LogError("Cannot spawn circle of fewer than 1 bullet.");
			return;
		}
		float partition = 360f / count;
		for (int i = 1; i <= count; i++) {
			Vector3 rotation = new Vector3(0, i * partition, 0);
			GameObject bullet = Instantiate(bulletPrefab, spawnpoint, Quaternion.Euler(rotation));
			bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * expandSpeed;
			bullet.GetComponent<BulletController>().gameController = gameController;
		}
	}

	/// Spawns an expanding circle of bullets aimed at the player.
	private void SpawnDirectedCircle(Vector3 spawnpoint, Vector3 direction, int count) {
		if (count == 1) {
			GameObject bullet = Instantiate(bulletPrefab, spawnpoint, Quaternion.LookRotation(direction));
			bullet.GetComponent<Rigidbody>().velocity = direction * bulletSpeed;
			bullet.GetComponent<BulletController>().gameController = gameController;
			return;
		}
		if (count == 0) {
			Debug.LogError("Cannot spawn circle of fewer than 1 bullet.");
			return;
		}
		float partition = 360f / count;
		for (int i = 1; i <= count; i++) {
			Vector3 rotation = new Vector3(0, i * partition, 0);
			GameObject bullet = Instantiate(bulletPrefab, spawnpoint, Quaternion.Euler(rotation));
			bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * expandSpeed + direction * carrierSpeed;
			bullet.GetComponent<BulletController>().gameController = gameController;
		}
	}
}
