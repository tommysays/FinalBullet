using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// Game cycle for battles.
public class GameController : MonoBehaviour {
	public GameObject bossHpBarObj;
	public GameObject warriorHpBarObj;
	public GameObject thiefHpBarObj;
	public GameObject mageHpBarObj;
	public GameObject turnBarObj;
	public GameObject randomSpawnerObj;
	public GameObject bulletSpawnerObj;
	public GameObject damageParticleSystem;
	public GameObject explosionParticles;
	public GameObject crosshairPrefab;
	public GameObject potionPrefab;
	public GameObject bombPrefab;
	public GameObject magicPrefab;
	public GameObject commandPanel;
	public GameObject commandSelector;
	public GameObject characterSelector;
	public GameObject attackButton;
	public GameObject itemButton;
	public GameObject magicButton;
	public GameObject warriorPanel;
	public GameObject thiefPanel;
	public GameObject magePanel;
	private Boss boss;
	private ScalingBar bossHpBar;
	private ScalingBar currentCharacterHpBar;
	private ScalingBar turnBar;
	private RandomSpawner randSpawner;
	private PointerController pointer;
	private int crosshairCounter = 0;
	/// When the counter reaches this limit, the next crosshair attack deals extra damage.
	private int crosshairStrike = 2;
	/// The latest time we spanwed a crosshair automatically.
	private float autoattackTime;
	/// How long to wait between spawning crosshairs automatically.
	private float autoattackInterval = 4f;

	private float turnStartTime = 0f;
	private float turnDuration = 10f;
	/// Number of turns until Item command becomes available again.
	private int itemCooldownMax = 1;
	/// Current cooldown for Item command.
	private int itemCooldown = 0;
	/// Number of turns until Magic command becomes available again.
	private int magicCooldownMax = 2;
	/// Current cooldown for Magic command.
	private int magicCooldown = 0;
	/// Whether or not the player is taking their turn.
	private bool takingTurn = false;
	/// Whether or not the player can take their next turn.
	private bool turnAvailable = false;

	/// Keeps track of how many magic tokens have been activated
	private int magicCounter = 0;
	/// How many magic tokens need to be activated to cast a spell.
	private int magicCounterMax = 4;
	private List<CommandObjectController> magicControllers = new List<CommandObjectController>();

	/// 0 = command selector, 1 = character selector.
	private int selectorIndex = 0;
	/// 0 = attack, 1 = item, 2 = magic
	private int commandSelectorIndex = 0;
	/// 0 = warrior, 1 = thief, 2 = mage
	private int characterSelectorIndex = 0;
	private Vector2 attackButtonPosition;
	private Vector2 itemButtonPosition;
	private Vector2 magicButtonPosition;
	private Vector2 warriorPanelPosition;
	private Vector2 thiefPanelPosition;
	private Vector2 magePanelPosition;
	private RectTransform commandSelectorRect;
	private RectTransform characterSelectorRect;

	private CHARACTER_TYPE currentCharacter = CHARACTER_TYPE.WARRIOR;
	private List<Character> characters;

	// Use this for initialization
	void Start () {
		characters = new List<Character>();
		characters.Add(new Character());
		characters.Add(new Character());
		characters.Add(new Character());
		boss = new Boss();

		bossHpBar = bossHpBarObj.GetComponent<ScalingBar>();
		bossHpBar.maxValue = boss.hp;
		bossHpBar.curValue = boss.hp;

		currentCharacterHpBar = thiefHpBarObj.GetComponent<ScalingBar>();
		currentCharacterHpBar.maxValue = characters[1].hp;
		currentCharacterHpBar.curValue = characters[1].hp;
		currentCharacterHpBar = mageHpBarObj.GetComponent<ScalingBar>();
		currentCharacterHpBar.maxValue = characters[2].hp;
		currentCharacterHpBar.curValue = characters[2].hp;
		currentCharacterHpBar = warriorHpBarObj.GetComponent<ScalingBar>();
		currentCharacterHpBar.maxValue = characters[0].hp;
		currentCharacterHpBar.curValue = characters[0].hp;

		turnBar = turnBarObj.GetComponent<ScalingBar>();
		turnBar.maxValue = turnDuration;
		turnBar.curValue = 0;

		randSpawner = randomSpawnerObj.GetComponent<RandomSpawner>();

		attackButtonPosition =  attackButton.GetComponent<RectTransform>().anchoredPosition;
		itemButtonPosition =  itemButton.GetComponent<RectTransform>().anchoredPosition;
		magicButtonPosition =  magicButton.GetComponent<RectTransform>().anchoredPosition;
		warriorPanelPosition = warriorPanel.GetComponent<RectTransform>().anchoredPosition;
		thiefPanelPosition = thiefPanel.GetComponent<RectTransform>().anchoredPosition;
		magePanelPosition = magePanel.GetComponent<RectTransform>().anchoredPosition;

		commandSelectorRect = commandSelector.GetComponent<RectTransform>();
		characterSelectorRect = characterSelector.GetComponent<RectTransform>();

		pointer = FindObjectOfType<PointerController>();
		pointer.setCharacter(currentCharacter);
	}
	
	// Update is called once per frame
	void Update () {
		float delta = Time.time - turnStartTime;
		float autoattackDelta = Time.time - autoattackTime;
		turnBar.setCurrentValue(delta);
		if (takingTurn) {
			if (Input.GetButtonDown("Up")) {
				moveSelector(0);
			} else if (Input.GetButtonDown("Down")) {
				moveSelector(1);
			} else if (Input.GetButtonDown("Left")) {
				moveSelector(2);
			} else if (Input.GetButtonDown("Right")) {
				moveSelector(3);
			} else if (Input.GetButtonDown("Submit")) {
				evaluateSubmit();
			}
		} else {
			if (delta >= turnDuration) {
				turnAvailable = true;
			} else if (autoattackDelta > autoattackInterval) {
				autoattackTime = Time.time;
				randSpawner.spawn(crosshairPrefab, 1);
			}
		}
		if (turnAvailable && !takingTurn && Input.GetButtonDown("Submit")) {
			turnAvailable = false;
			nextTurn();
		}
	}

	private void evaluateSubmit() {
		if (selectorIndex == 0) {
			switch (commandSelectorIndex) {
				case 0:
					handleAttackClick();
					disableSelectors();
					break;
				case 1:
					handleItemClick();
					disableSelectors();
					break;
				case 2:
					handleMagicClick();
					disableSelectors();
					break;
			}
		} else {
			switch (characterSelectorIndex) {
				case 0:
					if (currentCharacter != CHARACTER_TYPE.WARRIOR) {
						changeCharacter(CHARACTER_TYPE.WARRIOR);
						disableSelectors();
					} else {
						// TODO Play brr brr chime
					}
					break;
				case 1:
					if (currentCharacter != CHARACTER_TYPE.THIEF) {
						changeCharacter(CHARACTER_TYPE.THIEF);
						disableSelectors();
					} else {
						// TODO Play brr brr chime
					}
					break;
				case 2:
					if (currentCharacter != CHARACTER_TYPE.MAGE) {
						changeCharacter(CHARACTER_TYPE.MAGE);
						disableSelectors();
					} else {
						// TODO Play brr brr chime
					}
					break;
			}
		}
	}

	/// Changes character and starts the next turn.
	private void changeCharacter(CHARACTER_TYPE character) {
		currentCharacter = character;
		pointer.setCharacter(character);
		switch (character) {
			case CHARACTER_TYPE.WARRIOR:
				currentCharacterHpBar = warriorHpBarObj.GetComponent<ScalingBar>();
				break;
			case CHARACTER_TYPE.THIEF:
				currentCharacterHpBar = thiefHpBarObj.GetComponent<ScalingBar>();
				break;
			case CHARACTER_TYPE.MAGE:
				currentCharacterHpBar = mageHpBarObj.GetComponent<ScalingBar>();
				break;
		}
		turnReset();
	}

	private void disableSelectors() {
		commandSelectorIndex = 0;
		commandSelector.SetActive(false);
		characterSelector.SetActive(false);
		characterSelector.SetActive(false);
	}

	private void moveSelector(int direction) {
		if (direction == 0) {
			if (selectorIndex == 0) {
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
				switch (characterSelectorIndex) {
					case 1:
						if (characters[(int)CHARACTER_TYPE.WARRIOR].hp > 0) {
							characterSelectorRect.anchoredPosition = warriorPanelPosition;
							characterSelectorIndex = 0;
						}
						break;
					case 2:
						if (characters[(int)CHARACTER_TYPE.THIEF].hp > 0) {
							characterSelectorRect.anchoredPosition = thiefPanelPosition;
							characterSelectorIndex = 1;
						} else if (characters[(int)CHARACTER_TYPE.WARRIOR].hp > 0) {
							characterSelectorRect.anchoredPosition = warriorPanelPosition;
							characterSelectorIndex = 0;
						}
						break;
				}
			}
		} else if (direction == 1) {
			if (selectorIndex == 0) {
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
			} else {
				switch (characterSelectorIndex) {
					case 0:
						if (characters[(int)CHARACTER_TYPE.THIEF].hp > 0) {
							characterSelectorRect.anchoredPosition = thiefPanelPosition;
							characterSelectorIndex = 1;
						} else if (characters[(int)CHARACTER_TYPE.MAGE].hp > 0) {
							characterSelectorRect.anchoredPosition = magePanelPosition;
							characterSelectorIndex = 2;
						}
						break;
					case 1:
						if (characters[(int)CHARACTER_TYPE.MAGE].hp > 0) {
							characterSelectorRect.anchoredPosition = magePanelPosition;
							characterSelectorIndex = 2;
						}
						break;
				}
			}
		} else if (direction == 2) {
			selectorIndex = 0;
			commandSelector.SetActive(true);
			characterSelector.SetActive(false);
		} else {
			selectorIndex = 1;
			commandSelector.SetActive(false);
			characterSelector.SetActive(true);
		}
	}

	private void nextTurn() {
		// TODO Play a jingle to signal turn.
		takingTurn = true;
		Time.timeScale = 0;
		commandPanel.SetActive(true);
		commandSelector.SetActive(true);
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

	/// Resets things in preparation for the next turn.
	private void turnReset() {
		commandSelectorIndex = 0;
		selectorIndex = 0;
		commandPanel.SetActive(false);
		turnStartTime = Time.time;
		takingTurn = false;
		turnAvailable = false;
		Time.timeScale = 1f;
	}
	public void handleAttackClick() {
		turnReset();
		randSpawner.spawn(crosshairPrefab, 6);
	}

	public void handleItemClick() {
		turnReset();
		itemCooldown = itemCooldownMax;
		float rand = Random.value;
		if (rand < 0.5f) {
			randSpawner.spawn(potionPrefab, 3);
		} else {
			randSpawner.spawn(bombPrefab, 3);
		}
	}

	public void handleMagicClick() {
		turnReset();
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
			particleController.destination = warriorHpBarObj.transform.position;
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

	public void bombAttack(Vector3 origin, int amount) {
		// TODO Clear bullets around bomb.
		GameObject particles = Instantiate(damageParticleSystem, origin, Quaternion.identity);
		DamageParticleController particleController = particles.GetComponent<DamageParticleController>();
		particleController.setColor(Color.yellow);
		particleController.destination = bulletSpawnerObj.transform.position;
		particleController.travelTime = 0.75f;
		explode(origin, Color.yellow);
		explode(origin, new Color(1f, 0.5f, 0f));
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

	public void explode(Vector3 origin, Color color) {
		GameObject explosion = Instantiate(explosionParticles, origin, explosionParticles.transform.rotation);
		ParticleController explosionController = explosion.GetComponent<ParticleController>();
		explosionController.setColor(color);
		explosionController.play();
	}

	public void damageBoss(int amount) {
		boss.damage(amount);
		bossHpBar.setCurrentValue(boss.hp);
		Debug.Log("Boss is now at " + boss.hp + " hp.");
	}

	public void damagePlayer(int amount) {
		Character character = characters[(int)currentCharacter];
		character.damage(amount);
		currentCharacterHpBar.setCurrentValue(character.hp);
		Debug.Log("Player is now at " + character.hp + " hp.");
		if (character.hp == 0) {
			Debug.Log("Player is ded :(");
		}
	}

	public void healPlayer(int amount) {
		Character character = characters[(int)currentCharacter];
		character.heal(amount);
		currentCharacterHpBar.setCurrentValue(character.hp);
		Debug.Log("Player is now at " + character.hp + " hp.");
	}
}
