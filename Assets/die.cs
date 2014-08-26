using UnityEngine;
using System.Collections;

public class die : MonoBehaviour {

	private bool locked;
	private int pips;
	private int suit;
	private GameObject lockframe = null;
	private SpriteRenderer suitrenderer = null;
	private GUIStyle guistyle = null;

	public Sprite suit0, suit1, suit2, suit3;

	public void lockme() {
		locked = true;
		lockframe.SetActive(locked);
	}

	public void unlockme() {
		locked = false;
		lockframe.SetActive(locked);
	}

	public void rollme() {
		pips = Random.Range(1, 7);
		suit = Random.Range(0, 4);
		switch (suit) {
		case 0: suitrenderer.sprite = suit0;
			break;
		case 1: suitrenderer.sprite = suit1;
			break;
		case 2: suitrenderer.sprite = suit2;
			break;
		case 3: default: suitrenderer.sprite = suit3;
			break;
		}
	}
	
	// Use this for initialization
	void Start () {
		lockframe = (transform.Find("locked_sprite")).gameObject;
		GameObject suitframe = (transform.Find("suit_sprite")).gameObject;
		suitrenderer = suitframe.GetComponent<SpriteRenderer>();
		unlockme();
		rollme();
		guistyle = new GUIStyle();
		guistyle.normal.textColor = Color.black;
		guistyle.fontSize = 18;
		guistyle.alignment = TextAnchor.MiddleCenter;
	}
	
	void OnGUI () {
		Vector3 mypos = Camera.main.WorldToScreenPoint(transform.position);
		//		Rect rr = new Rect(transform.position.x, transform.position.y, 20, 20);
		Rect rr = new Rect(mypos.x, Screen.height - mypos.y, 0f, 0f);
		string ss = pips.ToString();
		GUI.Label(rr, ss, guistyle);
	}

	// Update is called once per frame
	void Update () {
	}
}
