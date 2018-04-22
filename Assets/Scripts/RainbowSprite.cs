using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// Cycles the albedo of a sprite through different colors over time.
[RequireComponent(typeof(SpriteRenderer))]
public class RainbowSprite : MonoBehaviour {
	public float speed = 4000f;
	private SpriteRenderer sprite;
	private float red = 1f;
	private float green = 1f;
	private float blue = 0f;
	private static float min = 0.7f;
	private float minRed = min;
	private float minGreen = min;
	private float minBlue = min;
	private int state = 0;
	// Use this for initialization
	void Start () {
		sprite = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		switch (state) {
			case 0:
				// Yellow to green
				red -= speed * Time.deltaTime;
				if (red < minRed) {
					red = minRed;
					state++;
				}
				break;
			case 1:
				// Green to teal
				blue += speed * Time.deltaTime;
				if (blue > 1) {
					blue = 1;
					state++;
				}
				break;
			case 2:
				// Teal to blue
				green -= speed * Time.deltaTime;
				if (green < minGreen) {
					green = minGreen;
					state++;
				}
				break;
			case 3:
				// Blue to purple
				red += speed * Time.deltaTime;
				if (red > 1) {
					red = 1;
					state++;
				}
				break;
			case 4:
				// Purple to red
				blue -= speed * Time.deltaTime;
				if (blue < minBlue) {
					blue = minBlue;
					state++;
				}
				break;
			case 5:
				// Red to yellow
				green += speed * Time.deltaTime;
				if (green > 1) {
					green = 1;
					state = 0;
				}
				break;
		}
		sprite.color = new Color(red, green, blue);
	}
}
