using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// Game cycle for battles.
public class GameController : MonoBehaviour {
	public GameObject bossHpBarObj;
	public GameObject turnBarObj;
	public GameObject randomSpawnerObj;
	public GameObject bulletSpawnerObj;
	public GameObject damageParticleSystem;
	public GameObject crosshairPrefab;
	public GameObject potionPrefab;
	public GameObject commandPanel;
	public GameObject attackButton;
	public GameObject itemButton;
	public GameObject magicButton;
	private Player player;
	private Boss boss;
	private ScalingBar bossHpBar;
	private ScalingBar turnBar;
	private RandomSpawner randSpawner;
	private int crosshairCounter = 0;
	/// When the counter reaches this limit, the next crosshair attack deals extra damage.
	private int crosshairStrike = 2;

	private float turnStartTime = -20f;
	private float turnDuration = 7f;
	private int itemCooldownMax = 1;
	private int itemCooldown = 0;
	private int magicCooldownMax = 2;
	private int magicCooldown = 0;
	private bool takingTurn = false;

	// Use this for initialization
	void Start () {
		player = new Player();
		boss = new Boss();

		bossHpBar = bossHpBarObj.GetComponent<ScalingBar>();
		bossHpBar.maxValue = boss.hp;
		bossHpBar.curValue = boss.hp;

		turnBar = turnBarObj.GetComponent<ScalingBar>();
		turnBar.maxValue = turnDuration;
		turnBar.curValue = 0;

		randSpawner = randomSpawnerObj.GetComponent<RandomSpawner>();
	}
	
	// Update is called once per frame
	void Update () {
		float delta = Time.time - turnStartTime;
		turnBar.setCurrentValue(delta);
		if (!takingTurn && delta >= turnDuration) {
			nextTurn();
		}
	}

	private void nextTurn() {
		takingTurn = true;
		commandPanel.SetActive(true);
		itemButton.SetActive(itemCooldown-- <= 0);
		magicButton.SetActive(magicCooldown-- <= 0);
		if (itemCooldown < 0) {
			itemCooldown = 0;
		}
		if (magicCooldown < 0) {
			magicCooldown = 0;
		}
	}

	public void handleAttackClick() {
		commandPanel.SetActive(false);
		turnStartTime = Time.time;
		takingTurn = false;
		randSpawner.spawn(crosshairPrefab, 9);
	}

	public void handleItemClick() {
		commandPanel.SetActive(false);
		turnStartTime = Time.time;
		takingTurn = false;
		itemCooldown = itemCooldownMax;
		randSpawner.spawn(potionPrefab, 5);
	}

	public void handleMagicClick() {
		commandPanel.SetActive(false);
		turnStartTime = Time.time;
		takingTurn = false;
		magicCooldown = magicCooldownMax;
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
		Debug.Log("Player is now at " + player.hp + " hp.");
		if (player.hp == 0) {
			Debug.Log("Player is ded :(");
		}
	}

	public void healPlayer(int amount) {
		player.heal(amount);
		Debug.Log("Player is now at " + player.hp + " hp.");
	}
}
