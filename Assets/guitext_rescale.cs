﻿using UnityEngine;
using System.Collections;

public class guitext_rescale : MonoBehaviour {

	// Use this for initialization
	void Awake () {
		float xScale = Screen.width / 480f;
		float yScale = Screen.height / 800f;
		float spriteScale = Mathf.Min(xScale, yScale);
		gameObject.GetComponent<GUIText>().fontSize = (int)(0.9f + (float)gameObject.GetComponent<GUIText>().fontSize * spriteScale);
	}
}
