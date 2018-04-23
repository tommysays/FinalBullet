using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// Instantly turns the albedo of an image to a given color, then quickly restores original color.
/// Calling swapColor will instead fade the albedo from the original color to the target color.
[RequireComponent(typeof(Image))]
public class ColorFlasher : MonoBehaviour {
	public float flashSpeed = 0.2f;
	private Image image;
	private Color originalColor;
	private Color tempColor;
	private bool isFlashing = false;
	private float flashStartTime;

	// Use this for initialization
	void Start () {
		image = GetComponent<Image>();
		originalColor = image.color;
	}
	
	// Update is called once per frame
	void Update () {
		if (isFlashing) {
			float delta = Time.time - flashStartTime;
			if (delta > flashSpeed) {
				isFlashing = false;
				image.color = originalColor;
			} else {
				image.color = Color.Lerp(tempColor, originalColor, delta / flashSpeed);
			}
		}
	}

	public ColorFlasher setFlashSpeed(float speed) {
		flashSpeed = speed;
		return this;
	}

	public void flashColor(Color color) {
		isFlashing = true;
		tempColor = color;
		flashStartTime = Time.time;
	}

	public void swapColor(Color color) {
		tempColor = originalColor;
		originalColor = color;
		flashColor(tempColor);
	}
}
