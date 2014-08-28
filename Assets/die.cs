using UnityEngine;
using System.Collections;

public class die : MonoBehaviour {

	private bool locked;
	private bool unlockable;
	public int pips;
	public int suit;
	public int multiplier;

	private Vector3 mypos;
	private Rect textpos;
	private GameObject lockframe = null;
	private SpriteRenderer suitrenderer = null;
	private GUIStyle bigfontstyle = null;
	private GUIStyle smallfontstyle = null;

	public Sprite suit0, suit1, suit2, suit3;

	public void lockme() {
		locked = true;
		lockframe.SetActive(locked);
	}

	public void unlockme() {
		locked = false;
		lockframe.SetActive(locked);
	}

	public void enableme() {
		unlockable = true;
	}
	
	public void disableme() {
		unlockable = false;
		lockme();
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
		mypos = Camera.main.WorldToScreenPoint(transform.position);
		textpos = new Rect(mypos.x, Screen.height - mypos.y, 0f, 0f);

		unlockme();
		enableme();
		rollme();
		multiplier = Random.Range(1, 11);
		bigfontstyle = new GUIStyle();
		bigfontstyle.normal.textColor = Color.black;
		bigfontstyle.fontSize = 24;
		bigfontstyle.alignment = TextAnchor.MiddleCenter;
		smallfontstyle = new GUIStyle();
		smallfontstyle.normal.textColor = Color.black;
		smallfontstyle.fontSize = 18;
		smallfontstyle.alignment = TextAnchor.MiddleCenter;
	}
	
	void OnMouseUpAsButton() {
		if (unlockable) {
			if (locked)
				unlockme ();
			else
				lockme ();
		}
	}

	void OnGUI () {
		textpos.x = mypos.x;
		textpos.y = Screen.height - mypos.y;
		string ss = pips.ToString();
		GUI.Label(textpos, ss, bigfontstyle);
		if (multiplier > 1) {
			textpos.x += 4f;
			textpos.y += 4f;
			GUI.Label(textpos, "x" + multiplier.ToString(), smallfontstyle);
		}
	}

	// Update is called once per frame
	void Update () {
	}
}
