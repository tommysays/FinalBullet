using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// Game cycle for battles.
public class GameController : MonoBehaviour {
	public GameObject bossHpBarObj;
	public GameObject playerHpBarObj;
	public GameObject turnBarObj;
	public GameObject randomSpawnerObj;
	public GameObject bulletSpawnerObj;
	public GameObject damageParticleSystem;
	public GameObject crosshairPrefab;
	public GameObject potionPrefab;
	public GameObject magicPrefab;
	public GameObject commandPanel;
	public GameObject attackButton;
	public GameObject itemButton;
	public GameObject magicButton;
	private Player player;
	private Boss boss;
	private ScalingBar bossHpBar;
	private ScalingBar playerHpBar;
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

	/// Keeps track of how many magic tokens have been activated
	private int magicCounter = 0;
	/// How many magic tokens need to be activated to cast a spell.
	private int magicCounterMax = 4;
	private List<MagicController> magicControllers;

	// Use this for initialization
	void Start () {
		player = new Player();
		boss = new Boss();

		bossHpBar = bossHpBarObj.GetComponent<ScalingBar>();
		bossHpBar.maxValue = boss.hp;
		bossHpBar.curValue = boss.hp;

		playerHpBar = playerHpBarObj.GetComponent<ScalingBar>();
		playerHpBar.maxValue = player.hp;
		playerHpBar.curValue = player.hp;

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
		magicCounter = 0;
		magicControllers = new List<MagicController>();
		randSpawner.spawn(magicPrefab, magicCounterMax);
		Debug.Log("Magic was selected");
	}

	public void magicActivation(MagicController controller) {
		magicControllers.Add(controller);
		magicCounter++;
		if (magicCounter >= magicCounterMax) {
			castSpell();
		}
	}

	/// Chooses a random spell to cast.
	private void castSpell() {
		float rand = Random.value;
		if (rand < 0.5f) {
			castHeal();
		} else {
			castFireball();
		}
	}

	private void castFireball() {
		// TODO Visual fancy things to indicate heal spell.
		foreach (MagicController controller in magicControllers) {
			GameObject particles = Instantiate(damageParticleSystem, controller.transform.position, Quaternion.identity);
			DamageParticleController particleController = particles.GetComponent<DamageParticleController>();
			particleController.setColor(Color.red);
			particleController.destination = bulletSpawnerObj.transform.position;
			Destroy(controller.gameObject);
		}
		damageBoss(60);
	}

	private void castHeal() {
		// TODO Visual fancy things to indicate fireball spell.
		foreach (MagicController controller in magicControllers) {
			GameObject particles = Instantiate(damageParticleSystem, controller.transform.position, Quaternion.identity);
			DamageParticleController particleController = particles.GetComponent<DamageParticleController>();
			particleController.setColor(Color.green);
			particleController.destination = playerHpBarObj.transform.position;
			Destroy(controller.gameObject);
		}
		healPlayer(20);
	}

	public void crosshairAttack(Vector3 origin, int amount) {
		if (crosshairCounter++ >= crosshairStrike) {
			crosshairCounter = 0;
			amount *= 3;
			StartCoroutine(criticalAttack(origin, bulletSpawnerObj.transform.position));
		} else {
			GameObject particles = Instantiate(damageParticleSystem, origin, Quaternion.identity);
			particles.GetComponent<DamageParticleController>().destination = bulletSpawnerObj.transform.position;
		}
		damageBoss(amount);
	}

	private IEnumerator criticalAttack(Vector3 origin, Vector3 destination) {
		GameObject particles = Instantiate(damageParticleSystem, origin, Quaternion.identity);
		particles.transform.localScale = new Vector3(5f, 5f, 5f);
		DamageParticleController controller = particles.GetComponent<DamageParticleController>();
		controller.destination = bulletSpawnerObj.transform.position;
		controller.setColor(Color.red);
		yield return new WaitForSeconds(0.1f);
		particles = Instantiate(damageParticleSystem, origin, Quaternion.identity);
		particles.transform.localScale = new Vector3(2f, 2f, 2f);
		controller = particles.GetComponent<DamageParticleController>();
		controller.destination = bulletSpawnerObj.transform.position;
		controller.setColor(new Color(1f, 0.5f, 0f));
		yield return new WaitForSeconds(0.1f);
		particles = Instantiate(damageParticleSystem, origin, Quaternion.identity);
		controller = particles.GetComponent<DamageParticleController>();
		controller.destination = bulletSpawnerObj.transform.position;
		controller.setColor(new Color(1f, 1f, 0f));
	}

	public void damageBoss(int amount) {
		boss.damage(amount);
		bossHpBar.setCurrentValue(boss.hp);
		Debug.Log("Boss is now at " + boss.hp + " hp.");
	}

	public void damagePlayer(int amount) {
		player.damage(amount);
		playerHpBar.setCurrentValue(player.hp);
		Debug.Log("Player is now at " + player.hp + " hp.");
		if (player.hp == 0) {
			Debug.Log("Player is ded :(");
		}
	}

	public void healPlayer(int amount) {
		player.heal(amount);
		playerHpBar.setCurrentValue(player.hp);
		Debug.Log("Player is now at " + player.hp + " hp.");
	}
}
