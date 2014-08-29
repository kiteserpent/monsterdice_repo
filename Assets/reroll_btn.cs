using UnityEngine;
using System.Collections;

public class reroll_btn : MonoBehaviour {

	private GameObject main_obj;
	private main_control main_script;
	private Color visible = new Color(1f, 1f, 1f, 1f);
	private Color faded = new Color(1f, 1f, 1f, 0.25f);
	private SpriteRenderer myRenderer;

	public bool pressable;

	// Use this for initialization
	void Start () {
		main_obj = GameObject.Find ("Controller");
		main_script = main_obj.GetComponent<main_control>();
		pressable = true;
		myRenderer = GetComponent<SpriteRenderer>();
	}
	
	void OnMouseUpAsButton() {
		if (pressable) {
			main_script.rerollPressed();
			pressable = false;
		}
	}
	
	// Update is called once per frame
	void Update () {
		myRenderer.color = pressable ? visible : faded;
	}
}
