using UnityEngine;

public enum CHARACTER_TYPE {
	WARRIOR = 0,
	THIEF = 1,
	MAGE = 2
}

/// Keeps track of a character's HP.
/// TODO Can add bonuses to damage, healing, cooldowns based on player "level".
public class Character {
	public int maxHp = 100;
	public int hp = 100;

	private float movespeed = 300f;

	public Character setMaxHp(int maxHp) {
		this.maxHp = maxHp;
		this.hp = maxHp;
		return this;
	}

	public Character setMovespeed(float movespeed) {
		this.movespeed = movespeed;
		return this;
	}

	public float getMovespeed() {
		return movespeed;
	}


	public void damage(int amount) {
		if (amount < 0) {
			Debug.LogError("Error: Damage should not be negative.");
			return;
		}
		hp -= amount;
		if (hp < 0) {
			hp = 0;
		}
	}
	public void heal(int amount) {
		if (amount < 0) {
			Debug.LogError("Error: Healing should not be negative.");
			return;
		}
		hp += amount;
		if (hp >= maxHp) {
			hp = maxHp;
		}
	}
}
