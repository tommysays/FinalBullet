using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// Instantly turns the albedo of an image to a given color, then quickly restores original color.
[RequireComponent(typeof(Image))]
public class ColorFlasher : MonoBehaviour {
	public float flashSpeed = 0.2f;
	private Image image;
	private Color originalColor;
	private Color targetColor;
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
				image.color = Color.Lerp(targetColor, originalColor, delta / flashSpeed);
			}
		}
	}

	public void flashColor(Color color) {
		isFlashing = true;
		targetColor = color;
		flashStartTime = Time.time;
	}
}
