using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// Game cycle for battles.
public class GameController : MonoBehaviour {
	public GameObject bossHealthBar;
	private Player player;
	private Boss boss;
	private ScalingBar bossHpBar;

	// Use this for initialization
	void Start () {
		player = new Player();
		boss = new Boss();

		bossHpBar = bossHealthBar.GetComponent<ScalingBar>();
		bossHpBar.maxValue = boss.hp;
		bossHpBar.curValue = boss.hp;
	}
	
	// Update is called once per frame
	void Update () {
		
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
