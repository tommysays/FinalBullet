using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// The carrier game object "holds" bullets as children, making it easier to move them in sync.
/// While bullets can despawn by hitting the player, the carrier must despawn after a set amount of time, since it doesn't collide.
public class CarrierController : MonoBehaviour {
	/// Destroys the carrier after this many seconds.
	public float deathTimer = 25f;
	void Start () {
		GameObject.Destroy(gameObject, deathTimer);
	}
}
