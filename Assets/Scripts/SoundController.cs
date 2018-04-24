using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SOUND {
	ERROR,
	TURN_READY
}
[RequireComponent(typeof(AudioSource))]
public class SoundController : MonoBehaviour {
	public AudioClip errorClip;
	public AudioClip turnReadyClip;
	public AudioClip battleMusic;
	public float sfxVolume = 0.2f;
	private AudioSource source;

	void Start() {
		source = GetComponent<AudioSource>();
	}

	public void playSound(SOUND sound) {
		switch (sound) {
			case SOUND.ERROR:
				source.PlayOneShot(errorClip, sfxVolume);
				break;
			case SOUND.TURN_READY:
				source.PlayOneShot(turnReadyClip, sfxVolume);
				break;
		}
	}
}
