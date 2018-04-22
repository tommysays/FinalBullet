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
	public GameObject commandSelector;
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
	/// The latest time we spanwed a crosshair automatically.
	private float autoattackTime;
	/// How long to wait between spawning crosshairs automatically.
	private float autoattackInterval = 4f;

	private float turnStartTime = 0f;
	private float turnDuration = 10f;
	private int itemCooldownMax = 1;
	private int itemCooldown = 0;
	private int magicCooldownMax = 2;
	private int magicCooldown = 0;
	private bool takingTurn = false;

	/// Keeps track of how many magic tokens have been activated
	private int magicCounter = 0;
	/// How many magic tokens need to be activated to cast a spell.
	private int magicCounterMax = 4;
	private List<CommandObjectController> magicControllers = new List<CommandObjectController>();

	private int commandSelectorIndex = 0;
	private Vector2 attackButtonPosition;
	private Vector2 itemButtonPosition;
	private Vector2 magicButtonPosition;
	private RectTransform commandSelectorRect;

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

		attackButtonPosition =  attackButton.GetComponent<RectTransform>().anchoredPosition;
		itemButtonPosition =  itemButton.GetComponent<RectTransform>().anchoredPosition;
		magicButtonPosition =  magicButton.GetComponent<RectTransform>().anchoredPosition;
		commandSelectorRect = commandSelector.GetComponent<RectTransform>();
	}
	
	// Update is called once per frame
	void Update () {
		float delta = Time.time - turnStartTime;
		float autoattackDelta = Time.time - autoattackTime;
		turnBar.setCurrentValue(delta);
		if (takingTurn) {
			if (Input.GetButtonDown("Up")) {
				moveSelector(true);
			} else if (Input.GetButtonDown("Down")) {
				moveSelector(false);
			} else if (Input.GetButtonDown("Submit")) {
				switch (commandSelectorIndex) {
					case 0:
						handleAttackClick();
						break;
					case 1:
						handleItemClick();
						break;
					case 2:
						handleMagicClick();
						break;
				}
				commandSelectorIndex = 0;
			}
		} else {
			if (delta >= turnDuration) {
				nextTurn();
			} else if (autoattackDelta > autoattackInterval) {
				autoattackTime = Time.time;
				randSpawner.spawn(crosshairPrefab, 1);
			}
		}
	}

	private void moveSelector(bool up = false) {
		if (up) {
			switch (commandSelectorIndex) {
				case 0:
					break;
				case 1:
					commandSelectorRect.anchoredPosition = attackButtonPosition;
					commandSelectorIndex = 0;
					break;
				case 2:
					if (itemButton.activeInHierarchy) {
						commandSelectorRect.anchoredPosition = itemButtonPosition;
						commandSelectorIndex = 1;
					} else {
						commandSelectorRect.anchoredPosition = attackButtonPosition;
						commandSelectorIndex = 0;
					}
					break;
			}
		} else {
			switch (commandSelectorIndex) {
				case 0:
					if (itemButton.activeInHierarchy) {
						commandSelectorRect.anchoredPosition = itemButtonPosition;
						commandSelectorIndex = 1;
					} else if (magicButton.activeInHierarchy) {
						commandSelectorRect.anchoredPosition = magicButtonPosition;
						commandSelectorIndex = 2;
					}
					break;
				case 1:
					if (magicButton.activeInHierarchy) {
						commandSelectorRect.anchoredPosition = magicButtonPosition;
						commandSelectorIndex = 2;
					}
					break;
				case 2:
					break;
			}
		}
	}

	private void nextTurn() {
		// TODO Play a jingle to signal turn.
		takingTurn = true;
		Time.timeScale = 0;
		commandPanel.SetActive(true);
		commandSelectorRect.anchoredPosition = attackButtonPosition;
		itemButton.SetActive(itemCooldown-- <= 0);
		magicButton.SetActive(magicCooldown-- <= 0);
		if (itemCooldown < 0) {
			itemCooldown = 0;
		}
		if (magicCooldown < 0) {
			magicCooldown = 0;
		}

		// Clear out all magic tokens.
		foreach (CommandObjectController controller in magicControllers) {
			controller.despawn();
		}
	}

	private void turnStart() {
		commandPanel.SetActive(false);
		turnStartTime = Time.time;
		takingTurn = false;
		Time.timeScale = 1f;
	}
	public void handleAttackClick() {
		turnStart();
		randSpawner.spawn(crosshairPrefab, 9);
	}

	public void handleItemClick() {
		turnStart();
		itemCooldown = itemCooldownMax;
		randSpawner.spawn(potionPrefab, 5);
	}

	public void handleMagicClick() {
		turnStart();
		magicCooldown = magicCooldownMax;
		magicCounter = 0;
		magicControllers = randSpawner.spawn(magicPrefab, magicCounterMax);
	}

	public void magicActivation(MagicController controller) {
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
		foreach (CommandObjectController controller in magicControllers) {
			GameObject particles = Instantiate(damageParticleSystem, controller.transform.position, Quaternion.identity);
			DamageParticleController particleController = particles.GetComponent<DamageParticleController>();
			particleController.setColor(Color.red);
			particleController.destination = bulletSpawnerObj.transform.position;
			controller.despawn();
		}
		damageBoss(60);
	}

	private void castHeal() {
		// TODO Visual fancy things to indicate fireball spell.
		foreach (CommandObjectController controller in magicControllers) {
			GameObject particles = Instantiate(damageParticleSystem, controller.transform.position, Quaternion.identity);
			DamageParticleController particleController = particles.GetComponent<DamageParticleController>();
			particleController.setColor(Color.green);
			particleController.destination = playerHpBarObj.transform.position;
			controller.despawn();
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
