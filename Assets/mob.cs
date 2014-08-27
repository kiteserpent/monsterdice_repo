using UnityEngine;
using System.Collections;

public class mob : MonoBehaviour {

	private int hp;
	private int maxhp;
	private int level;
	private int attack;
	private int element;
	private const float Boxwidth = 100f, Boxheight = 14f;
	private static Texture2D redTexture, greenTexture;
	private static GUIStyle redStyle, greenStyle, smallfontstyle;
	private static Vector3 mypos;
	private Rect rr, baseRR;
	private SpriteRenderer elementrenderer = null;
	
	public Sprite mob0, mob1, mob2;
	public Sprite element0, element1, element2;

	public void levelup() {
		SpriteRenderer myrenderer = GetComponent<SpriteRenderer>();
		++level;
		maxhp = 10 * level;
		hp = maxhp;
		attack = 10 * level;
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
		baseRR = new Rect(mypos.x - Boxwidth/2f, Screen.height - mypos.y - 24f - Boxheight, Boxwidth, Boxheight);
		rr = new Rect();
		GameObject elementframe = (transform.Find("element_sprite")).gameObject;
		elementrenderer = elementframe.GetComponent<SpriteRenderer>();
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
		smallfontstyle.fontSize = 12;
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
		rr.x += 50f;
		rr.y += 40f;
		GUI.Label (rr, "ATK: " + attack.ToString(), smallfontstyle);
	}
}
