﻿using UnityEngine;
using System.Collections;

public class mob : MonoBehaviour {

	public int hp;
	public int maxhp;
	public int level;
	public int attack;
	public int element;
	private const float Boxwidth = 150f, Boxheight = 28f, Boxyoffset = -60f;
	private static Texture2D redTexture, greenTexture;
	private static GUIStyle redStyle, greenStyle, fontstyle;
	private static Vector3 mypos;
	private Rect textpos, basetextpos;
	private SpriteRenderer elementrenderer = null;
	
	public Sprite mob0, mob1, mob2;
	public Sprite element0, element1, element2;

	public void levelup() {
		SpriteRenderer myrenderer = GetComponent<SpriteRenderer>();
		++level;
		maxhp = 10 * level;
		hp = maxhp;
		attack = 5 * level;
		element = Random.Range (0, 4);
		switch (element) {
		case 0:
			myrenderer.sprite = mob0;
			elementrenderer.sprite = element0;
			break;
		case 1:
			myrenderer.sprite = mob1;
			elementrenderer.sprite = element1;
			break;
		case 2:
			myrenderer.sprite = mob2;
			elementrenderer.sprite = element2;
			break;
		}
	}

	// Use this for initialization
	void Start () {
		mypos = Camera.main.WorldToScreenPoint(transform.position);
		basetextpos = new Rect(mypos.x - Boxwidth/2f, Screen.height - mypos.y + Boxyoffset - Boxheight, Boxwidth, Boxheight);
		textpos = new Rect();
		GameObject elementframe = (transform.Find("element_sprite")).gameObject;
		elementrenderer = elementframe.GetComponent<SpriteRenderer>();
		level = 0;
		maxhp = 1;
		if (redTexture == null) {
			redTexture = new Texture2D( 1, 1 );
			redTexture.SetPixel( 0, 0, new Color(1f, 0.1f, 0.1f ));
			redTexture.Apply();
		}
		if (redStyle == null) {
			redStyle = new GUIStyle();
			redStyle.normal.background = redTexture;
		}
		if (greenTexture == null) {
			greenTexture = new Texture2D( 1, 1 );
			greenTexture.SetPixel( 0, 0, new Color(0.1f, 1f, 0.1f ));
			greenTexture.Apply();
		}
		if (greenStyle == null) {
			greenStyle = new GUIStyle();
			greenStyle.normal.background = greenTexture;
		}
		fontstyle = new GUIStyle();
		fontstyle.normal.textColor = Color.black;
		fontstyle.fontSize = 26;
		fontstyle.alignment = TextAnchor.LowerCenter;

		levelup ();
	}
	
	// Update is called once per frame
	void Update () {
	}

	void OnGUI () {
		textpos = basetextpos;
		GUI.Box(textpos,  GUIContent.none, redStyle);
		textpos.width = Boxwidth * hp / maxhp;
		GUI.Box(textpos,  GUIContent.none, greenStyle);
		textpos.x = mypos.x;
		textpos.y = Screen.height - mypos.y + Boxyoffset;
		textpos.width = textpos.height = 0f;
		GUI.Label(textpos, hp.ToString() + " / " + maxhp.ToString(), fontstyle);
		textpos.x += 94f;
		textpos.y += 40f;
		GUI.Label (textpos, "Level: " + level.ToString(), fontstyle);
		textpos.y += 31f;
		GUI.Label (textpos, "ATK: " + attack.ToString(), fontstyle);
	}
}
