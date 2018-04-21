using UnityEngine;

/// Keeps track of the player's HP.
/// TODO Can add bonuses to damage, healing, cooldowns based on player "level".
public class Player {
	private const int MAX_HP = 100;
	public int hp = 100;

	public void damage(int amount) {
		if (amount < 0) {
			Debug.LogError("Error: Damage should not be negative.");
			return;
		}
		hp -= amount;
		if (hp < 0) {
			hp = 0;
		}
		if (hp == 0) {
			Debug.Log("Player lost all hp! GG");
		}
	}
	public void heal(int amount) {
		if (amount < 0) {
			Debug.LogError("Error: Healing should not be negative.");
			return;
		}
		hp += amount;
		if (hp >= MAX_HP) {
			hp = MAX_HP;
		}
	}
}
