using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairSpawner : MonoBehaviour {
	public GameController gameController;
	public GameObject crosshairPrefab;
	public float xMin;
	public float xMax;
	public float zMin;
	public float zMax;

	private float xDelta;

	// Use this for initialization
	void Start () {
		gameController = FindObjectOfType<GameController>();
		xDelta = xMax - xMin;
	}

	/// Spawns a given number of crosshair attack targets.
	public void spawnAttack(int count) {
		// We want this to be randomized, but also somewhat uniformly distributed.
		// To do this, we split the field into count columns before placing one in each.
		float xSliceWidth = xDelta / count;
		for (int i = 1; i <= count; i++) {
			float xSliceMin = xMin + xSliceWidth * (i - 1);
			float xSliceMax = xMin + xSliceWidth * i;
			float x = Random.Range(xSliceMin, xSliceMax);
			float z = Random.Range(zMin, zMax);
			GameObject crosshair = Instantiate(crosshairPrefab, new Vector3(x, 1, z), Quaternion.identity);
			crosshair.transform.rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
			crosshair.GetComponent<Rigidbody>().angularVelocity = new Vector3(0, 60, 0);
			crosshair.GetComponent<CrosshairController>().gameController = gameController;
		}
	}
}
