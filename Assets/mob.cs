using UnityEngine;
using System.Collections;

public class mob : MonoBehaviour {

	public int hp;
	public int maxhp;
	public int level;
	public int attack;
	public int element;
	private float Boxwidth = 150f, Boxheight = 28f, Boxyoffset = -70f;
	private static Texture2D redTexture, greenTexture;
	private static GUIStyle redStyle, greenStyle, fontstyle;
	private static Vector3 mypos;
	private Rect textpos, basetextpos;
	private SpriteRenderer elementrenderer = null;
	
	public Sprite mob0, mob1, mob2;
	public Sprite element0, element1, element2;
	private float spriteScale = 1f;

	public void levelup() {
		SpriteRenderer myrenderer = GetComponent<SpriteRenderer>();
		++level;
		maxhp = 10 * level;
		hp = maxhp;
		attack = 5 * level;
		element = Random.Range (0, 3);
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

	void Awake () {
		float xScale = Screen.width / 480f;
		float yScale = Screen.height / 800f;
		spriteScale = Mathf.Min(xScale, yScale);

		if (spriteScale > 1f) {
			gameObject.transform.localScale = new Vector3(spriteScale, spriteScale, 1f);
		}
		Boxwidth *= spriteScale;
		Boxheight *= spriteScale;
		Boxyoffset *= spriteScale;
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
		fontstyle.fontSize = (int)(28f * spriteScale);
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
		textpos.x += 94f * spriteScale;
		textpos.y += 40f * spriteScale;
		GUI.Label (textpos, "Level: " + level.ToString(), fontstyle);
		textpos.y += 31f * spriteScale;
		GUI.Label (textpos, "ATK: " + attack.ToString(), fontstyle);
	}
}
