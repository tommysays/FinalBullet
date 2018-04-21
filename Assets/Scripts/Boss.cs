using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss {
	private const int MAX_HP = 1000;
	public int hp = MAX_HP;

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
			Debug.Log("Boss lost all hp. Good job!");
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