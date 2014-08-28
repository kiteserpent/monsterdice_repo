using UnityEngine;
using System.Collections;

public class main_control : MonoBehaviour {
	private int health;
	private int maxhealth;

	private Rect textpos;
	private const float Boxwidth = 400f, Boxheight = 24f;
	private static Texture2D redTexture, greenTexture;
	private static GUIStyle redStyle, greenStyle, bigfontstyle, smallfontstyle;
	private int[] element_totals = new int[4];
	private int[] adjustment_multipliers = new int[4];
	private int[] adjusted_totals = new int[4];

	public GameObject[] dice_objects;
	public GameObject mob_object;
	private die[] dice_scripts = new die[5];
	private mob mob_script = new mob();

	// Use this for initialization
	void Start () {
		health = maxhealth = 250;
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
		textpos = new Rect();
		bigfontstyle = new GUIStyle();
		bigfontstyle.normal.textColor = Color.black;
		bigfontstyle.fontSize = 24;
		bigfontstyle.alignment = TextAnchor.MiddleCenter;
		smallfontstyle = new GUIStyle();
		smallfontstyle.normal.textColor = Color.black;
		smallfontstyle.fontSize = 20;
		smallfontstyle.alignment = TextAnchor.MiddleCenter;
		dice_scripts[0] = dice_objects[0].GetComponent<die>();
		dice_scripts[1] = dice_objects[1].GetComponent<die>();
		dice_scripts[2] = dice_objects[2].GetComponent<die>();
		dice_scripts[3] = dice_objects[3].GetComponent<die>();
		dice_scripts[4] = dice_objects[4].GetComponent<die>();
		mob_script = mob_object.GetComponent<mob>();
	}

	void calculate() {
		int element_index, die_index;
		for (element_index=0; element_index<4; ++element_index) {
			element_totals[element_index] = 0;
		}
		for (die_index=0; die_index<5; ++die_index) {
			element_totals[dice_scripts[die_index].suit] +=
				dice_scripts[die_index].pips * dice_scripts[die_index].multiplier;
		}
		switch (mob_script.element) {
		case 0:
			adjustment_multipliers[0] = 1;
			adjustment_multipliers[1] = 2;
			adjustment_multipliers[2] = -1;
			break;
		case 1:
			adjustment_multipliers[1] = 1;
			adjustment_multipliers[2] = 2;
			adjustment_multipliers[0] = -1;
			break;
		case 2:
			adjustment_multipliers[2] = 1;
			adjustment_multipliers[0] = 2;
			adjustment_multipliers[1] = -1;
			break;
		}
		adjustment_multipliers[3] = 1; // healing is constant
		for (element_index=0; element_index<4; ++element_index) {
			adjusted_totals[element_index] =
				element_totals[element_index] * adjustment_multipliers[element_index];
		}
	}


	void Update () {
	
	}
	
	void OnGUI () {
		textpos.x = (Screen.width - Boxwidth) / 2f;
		textpos.y = Screen.height - 2f * Boxheight;
		textpos.width = Boxwidth;
		textpos.height = Boxheight;
		GUI.Box(textpos,  GUIContent.none, redStyle);
		textpos.width = Boxwidth * health / maxhealth;
		GUI.Box(textpos,  GUIContent.none, greenStyle);
		textpos.x = Screen.width / 2f;
		textpos.y = Screen.height - 24f;
		textpos.width = textpos.height = 0f;
		GUI.Label(textpos, health.ToString() + " / " + maxhealth.ToString(), smallfontstyle);
	}
}
