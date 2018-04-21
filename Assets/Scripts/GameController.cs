using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// Game cycle for battles.
public class GameController : MonoBehaviour {
	public GameObject bossHealthBar;
	public GameObject crosshairSpawnerObj;
	private Player player;
	private Boss boss;
	private ScalingBar bossHpBar;
	private CrosshairSpawner crosshairSpawner;

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
		crosshairSpawner.spawnAttack(3);
	}

	public void handleItemClick() {
		Debug.Log("Item was selected");
	}

	public void handleMagicClick() {
		Debug.Log("Magic was selected");
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
