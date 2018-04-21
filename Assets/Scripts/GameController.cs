using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// Game cycle for battles.
public class GameController : MonoBehaviour {
	public GameObject bossHealthBar;
	public GameObject crosshairSpawnerObj;
	public GameObject bulletSpawnerObj;
	public GameObject damageParticleSystem;
	private Player player;
	private Boss boss;
	private ScalingBar bossHpBar;
	private CrosshairSpawner crosshairSpawner;
	private int crosshairCounter = 0;
	/// When the counter reaches this limit, the next crosshair attack deals extra damage.
	private int crosshairStrike = 2;

	// Use this for initialization
	void Start () {
		player = new Player();
		boss = new Boss();

		bossHpBar = bossHealthBar.GetComponent<ScalingBar>();
		bossHpBar.maxValue = boss.hp;
		bossHpBar.curValue = boss.hp;

		crosshairSpawner = crosshairSpawnerObj.GetComponent<CrosshairSpawner>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void handleAttackClick() {
		crosshairSpawner.spawnAttack(9);
	}

	public void handleItemClick() {
		Debug.Log("Item was selected");
	}

	public void handleMagicClick() {
		Debug.Log("Magic was selected");
	}

	public void crosshairAttack(Vector3 origin, int amount) {
		if (crosshairCounter++ >= crosshairStrike) {
			crosshairCounter = 0;
			amount *= 3;
		}
		damageBoss(origin, amount);
	}
	public void damageBoss(Vector3 origin, int amount) {
		GameObject particles = Instantiate(damageParticleSystem, origin, Quaternion.identity);
		particles.GetComponent<DamageParticleController>().destination = bulletSpawnerObj.transform.position;
		damageBoss(amount);
	}

	public void damageBoss(int amount) {
		boss.damage(amount);
		bossHpBar.setCurrentValue(boss.hp);
		Debug.Log("Boss is now at " + boss.hp + " hp.");
	}

	public void damagePlayer(int amount) {
		player.damage(amount);
		if (player.hp == 0) {
			Debug.Log("Player is ded :(");
		}
	}

	public void healPlayer(int amount) {
		player.heal(amount);
	}
}
