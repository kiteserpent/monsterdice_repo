using UnityEngine;
using System.Collections;

public class main_control : MonoBehaviour {
	private int health;
	private int maxhealth;

	// Use this for initialization
	void Start () {
		health = maxhealth = 250;
		foreach(GameObject fooObj in GameObject.FindGameObjectsWithTag("die"))
		{
				//Do Something
		}	
	}
	
	void Update () {
	
	}
	
	void OnGUI () {
		
	}
}
