using UnityEngine;
using System.Collections;

public class mob : MonoBehaviour {

	private int hp;
	private int maxhp;
	private int level;
	private int attack;
	private int element;
	private const float Boxwidth = 50f, Boxheight = 10f;
	private static Texture2D redTexture, greenTexture;
	private static GUIStyle redStyle, greenStyle, smallfontstyle;
	private static Vector3 mypos;
	private Rect rr, baseRR;

	public void levelup() {
		++level;
		maxhp = 10 * level;
		hp = maxhp;
		attack = 10 * level;
		element = Random.Range (0, 4);
	}

	// Use this for initialization
	void Start () {
		mypos = Camera.main.WorldToScreenPoint(transform.position);
		baseRR = new Rect(mypos.x - Boxwidth/2f, Screen.height - mypos.y - 24f - Boxheight, Boxwidth, Boxheight);
		rr = new Rect();
		level = 0;
		maxhp = 1;
		if (redTexture == null)
			redTexture = new Texture2D( 1, 1 );
		if (greenTexture == null)
			greenTexture = new Texture2D( 1, 1 );
		if (redStyle == null)
			redStyle = new GUIStyle();
		if (greenStyle == null)
			greenStyle = new GUIStyle();
		redTexture.SetPixel( 0, 0, new Color(1f, 0.1f, 0.1f ));
        redTexture.Apply();
		redStyle.normal.background = redTexture;
		greenTexture.SetPixel( 0, 0, new Color(0.1f, 1f, 0.1f ));
		greenTexture.Apply();
		greenStyle.normal.background = greenTexture;
		smallfontstyle = new GUIStyle();
		smallfontstyle.normal.textColor = Color.black;
		smallfontstyle.fontSize = 10;
		smallfontstyle.alignment = TextAnchor.LowerCenter;

		levelup ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI () {
		rr = baseRR;
		GUI.Box(rr,  GUIContent.none, redStyle);
		rr.width = Boxwidth * hp / maxhp;
		GUI.Box(rr,  GUIContent.none, greenStyle);
		rr.x = mypos.x;
		rr.y = Screen.height - mypos.y - 24f;
		rr.width = rr.height = 0f;
		GUI.Label(rr, hp.ToString() + " / " + maxhp.ToString(), smallfontstyle);
		rr.x += 40f;
		rr.y += 20f;
		GUI.Label (rr, "ATK: " + attack.ToString ());
	}
}
