using UnityEngine;
using System.Collections;

public class attack_btn : MonoBehaviour {

	private GameObject main_obj;
	private main_control main_script;

	// Use this for initialization
	void Start () {
		main_obj = GameObject.Find ("Controller");
		main_script = main_obj.GetComponent<main_control>();
	}
	
	void OnMouseUpAsButton() {
		main_script.attackPressed();
	}
}
