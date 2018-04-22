using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// Superclass for objects spawned by commands (e.g. crosshairs, potions).
public abstract class CommandObjectController : MonoBehaviour {
	public GameController gameController;
	public static Vector3 slowSpin = new Vector3(0, 2.5f, 0);
	public static Vector3 fastSpin = new Vector3(0, 4f, 0);
}
