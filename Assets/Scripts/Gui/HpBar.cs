using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpBar : MonoBehaviour {
	public const float SLIDE_SPEED = 0.5f;
	public GameObject fullBar;
	public GameObject currentBar;
	private RectTransform currentRect;
	private float maxWidth;
	private float height;

	/// Should be replaced with max hp on battle start.
	public float maxValue = 10f;
	/// Should be replaced with max hp on battle start.
	public float curValue = 10f;
	public float lastValue;
	private bool shouldSlide = false;
	private float slideStartTime;

	// Use this for initialization
	void Start() {
		maxWidth = fullBar.GetComponent<RectTransform>().sizeDelta.x;
		Debug.Log(maxWidth);
		currentRect = currentBar.GetComponent<RectTransform>();
		height = currentRect.sizeDelta.y;
	}
	
	void Update() {
		if (shouldSlide) {
			float ratio = (Time.time - slideStartTime) / SLIDE_SPEED;
			float width = ((lastValue * (1 - ratio) + curValue * ratio) / maxValue) * maxWidth;
			if (ratio > 1) {
				shouldSlide = false;
			}
			if (width > maxWidth) {
				width = maxWidth;
				shouldSlide = false;
			}
			Debug.Log(width + ", " + curValue);
			currentRect.sizeDelta = new Vector2(width, height);
		}
	}

	public void setCurrentValue(float cur) {
		lastValue = curValue;
		curValue = cur;
		shouldSlide = true;
		slideStartTime = Time.time;
	}
}
