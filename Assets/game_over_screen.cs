using UnityEngine;
using System.Collections;

public class game_over_screen : MonoBehaviour {

	void OnMouseUpAsButton() {
		Application.LoadLevel("playfield");
	}

	void Update(){
		if (Input.GetKeyDown(KeyCode.Escape)) 
			Application.Quit(); 
	}	
}
