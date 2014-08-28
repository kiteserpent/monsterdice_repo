using UnityEngine;
using System;
using System.Collections;

public class main_control : MonoBehaviour {
	private int health;
	private int maxhealth;
	private bool can_reroll;
	private string handname = "";

	private Rect textpos;
	private const float Boxwidth = 400f, Boxheight = 24f;
	private static Texture2D redTexture, greenTexture;
	private static GUIStyle redStyle, greenStyle, bigfontstyle, smallfontstyle;
	private int[] element_totals = new int[4];
	private int[] adjustment_multipliers = new int[4];
	private int[] adjusted_totals = new int[4];

	public die[] dice_scripts = new die[5];
	public GameObject mob_object;
	private mob mob_script;

	// Use this for initialization
	void Start () {
		health = maxhealth = 250;
		can_reroll = true;
		if (redTexture == null) {
			redTexture = new Texture2D( 1, 1 );
			redTexture.SetPixel( 0, 0, new Color(1f, 0.1f, 0.1f ));
			redTexture.Apply();
		}
		if (redStyle == null) {
			redStyle = new GUIStyle();
			redStyle.normal.background = redTexture;
			redStyle.normal.textColor = Color.red;
			redStyle.fontSize = 20;
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

		GameObject[] dda = GameObject.FindGameObjectsWithTag("die");
		int ii = 0;
		foreach ( GameObject dd in dda)
		{
			dice_scripts[ii++] = dd.GetComponent<die>();
		}
		mob_script = mob_object.GetComponent<mob>();

		calculate();
	}

	void calculate() {
		int element_index, die_index;
		for (element_index=0; element_index<4; ++element_index) {
			element_totals[element_index] = 0;
		}
		foreach ( die dd in dice_scripts ) {
			dd.multiplier = 1;
		}
		handname = "";

		// cheap sort
		int fd, sd;
		die tmp;
		for (fd=0; fd<4; ++fd) {
			for (sd=fd+1; sd<5; ++sd) {
				if (dice_scripts[fd].pips > dice_scripts[sd].pips) {
					tmp = dice_scripts[fd];
					dice_scripts[fd] = dice_scripts[sd];
					dice_scripts[sd] = tmp;
				}
			}
		}
		// analyze hands
		bool flush_flag =
				(dice_scripts[0].suit == dice_scripts[1].suit) &&
				(dice_scripts[0].suit == dice_scripts[2].suit) &&
				(dice_scripts[0].suit == dice_scripts[3].suit) &&
				(dice_scripts[0].suit == dice_scripts[4].suit);
		bool straight_flag =
				(dice_scripts[0].pips == dice_scripts[1].pips - 1) &&
				(dice_scripts[1].pips == dice_scripts[2].pips - 1) &&
				(dice_scripts[2].pips == dice_scripts[3].pips - 1) &&
				(dice_scripts[3].pips == dice_scripts[4].pips - 1);
		if (dice_scripts[0].pips == dice_scripts[4].pips) {
			handname = "Five of a Kind";
			foreach ( die dd in dice_scripts )
				dd.multiplier = 10;
		} else if (straight_flag && flush_flag) {
			handname = "Straight Flush";
			foreach ( die dd in dice_scripts )
				dd.multiplier = 10;
		} else if (dice_scripts[0].pips == dice_scripts[3].pips) {
			handname = "Four of a Kind";
			dice_scripts[0].multiplier = dice_scripts[1].multiplier = dice_scripts[2].multiplier = dice_scripts[3].multiplier = 8;
		} else if (dice_scripts[1].pips == dice_scripts[4].pips) {
			handname = "Four of a Kind";
			dice_scripts[1].multiplier = dice_scripts[2].multiplier = dice_scripts[3].multiplier = dice_scripts[4].multiplier = 8;
		} else if (((dice_scripts[0].pips == dice_scripts[1].pips) && (dice_scripts[2].pips == dice_scripts[4].pips)) ||
		           ((dice_scripts[0].pips == dice_scripts[2].pips) && (dice_scripts[3].pips == dice_scripts[4].pips))) {
			handname = "Full House";
			foreach ( die dd in dice_scripts )
				dd.multiplier = 8;
		} else if (straight_flag || flush_flag) {
			handname = straight_flag ? "Straight" : "Flush";
			foreach ( die dd in dice_scripts )
				dd.multiplier = 4;
		} else if (dice_scripts[0].pips == dice_scripts[2].pips) {
			handname = "Three of a Kind";
			dice_scripts[0].multiplier = dice_scripts[1].multiplier = dice_scripts[2].multiplier = 3;
		} else if (dice_scripts[1].pips == dice_scripts[3].pips) {
			handname = "Three of a Kind";
			dice_scripts[1].multiplier = dice_scripts[2].multiplier = dice_scripts[3].multiplier = 3;
		} else if (dice_scripts[2].pips == dice_scripts[4].pips) {
			handname = "Three of a Kind";
			dice_scripts[2].multiplier = dice_scripts[3].multiplier = dice_scripts[4].multiplier = 3;
		} else {
			int pair_count = 0;
			if (dice_scripts[0].pips == dice_scripts[1].pips) {
				++pair_count;
				dice_scripts[0].multiplier = dice_scripts[1].multiplier = 2;
			}
			if (dice_scripts[1].pips == dice_scripts[2].pips) {
				++pair_count;
				dice_scripts[1].multiplier = dice_scripts[2].multiplier = 2;
			}
			if (dice_scripts[2].pips == dice_scripts[3].pips) {
				++pair_count;
				dice_scripts[2].multiplier = dice_scripts[3].multiplier = 2;
			}
			if (dice_scripts[3].pips == dice_scripts[4].pips) {
				++pair_count;
				dice_scripts[3].multiplier = dice_scripts[4].multiplier = 2;
			}
			if (pair_count == 1)
				handname = "Pair";
			if (pair_count == 2)
				handname = "Two Pair";
		}

		foreach ( die dd in dice_scripts ) {
			element_totals[dd.suit] += dd.pips * dd.multiplier;
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
		calculate();
		textpos.x = 90f;
		textpos.y = 400f;
		textpos.width = textpos.height = 0f;
		GUI.Label(textpos, adjusted_totals[0].ToString(), adjusted_totals[0] < 0 ? redStyle : smallfontstyle);
		textpos.x = 235f;
		GUI.Label(textpos, adjusted_totals[1].ToString(), adjusted_totals[1] < 0 ? redStyle : smallfontstyle);
		textpos.x = 360f;
		GUI.Label(textpos, adjusted_totals[2].ToString(), adjusted_totals[2] < 0 ? redStyle : smallfontstyle);
		textpos.x = 235f;
		textpos.y = 450f;
		GUI.Label(textpos, adjusted_totals[3].ToString(), smallfontstyle);
		textpos.x = (Screen.width - Boxwidth) / 2f;
		textpos.y = Screen.height - 2f * Boxheight;
		textpos.width = Boxwidth;
		textpos.height = Boxheight;
		GUI.Box(textpos,  GUIContent.none, redStyle);
		textpos.width = Boxwidth * health / maxhealth;
		GUI.Box(textpos,  GUIContent.none, greenStyle);
		textpos.x = Screen.width / 2f;
		textpos.y = Screen.height - 34f;
		textpos.width = textpos.height = 0f;
		GUI.Label(textpos, health.ToString() + " / " + maxhealth.ToString(), smallfontstyle);
		textpos.y = 240f;
		GUI.Label(textpos, handname, bigfontstyle);
	}
}
