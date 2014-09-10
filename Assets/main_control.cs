using UnityEngine;
using System;
using System.Collections;

public class main_control : MonoBehaviour {
	private int health;
	private int maxhealth;
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
	private GameObject reroll_btn_obj;
	private reroll_btn reroll_btn_script;
	public GameObject fire_obj;
	public GameObject water_obj;
	public GameObject wood_obj;
	public GameObject immune_obj;
	public GameObject vuln_obj;
	public GameObject crossout_obj;
	private BoxCollider2D my_collider;

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
			redStyle.normal.textColor = Color.red;
			redStyle.fontSize = 22;
			redStyle.alignment = TextAnchor.MiddleCenter;
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
		bigfontstyle.fontSize = 28;
		bigfontstyle.alignment = TextAnchor.MiddleCenter;
		smallfontstyle = new GUIStyle();
		smallfontstyle.normal.textColor = Color.black;
		smallfontstyle.fontSize = 22;
		smallfontstyle.alignment = TextAnchor.MiddleCenter;

		GameObject[] dda = GameObject.FindGameObjectsWithTag("die");
		int ii = 0;
		foreach ( GameObject dd in dda)
		{
			dice_scripts[ii++] = dd.GetComponent<die>();
		}
		mob_script = mob_object.GetComponent<mob>();
		reroll_btn_obj = GameObject.Find("reroll_sprite");
		reroll_btn_script = reroll_btn_obj.GetComponent<reroll_btn>();
		my_collider = GetComponent<BoxCollider2D>();

		calculate();
	}

	void calculate() {
		int element_index;
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
			adjustment_multipliers[2] = 0;
			vuln_obj.transform.position = water_obj.transform.position;
			immune_obj.transform.position = wood_obj.transform.position;
			break;
		case 1:
			adjustment_multipliers[1] = 1;
			adjustment_multipliers[2] = 2;
			adjustment_multipliers[0] = 0;
			vuln_obj.transform.position = wood_obj.transform.position;
			immune_obj.transform.position = fire_obj.transform.position;
			break;
		case 2:
			adjustment_multipliers[2] = 1;
			adjustment_multipliers[0] = 2;
			adjustment_multipliers[1] = 0;
			vuln_obj.transform.position = fire_obj.transform.position;
			immune_obj.transform.position = water_obj.transform.position;
			break;
		}
		adjustment_multipliers[3] = 1; // healing is constant
		for (element_index=0; element_index<4; ++element_index) {
			adjusted_totals[element_index] =
				element_totals[element_index] * adjustment_multipliers[element_index];
		}
	}

	public void rerollPressed() {
		foreach ( die dd in dice_scripts ) {
			if (!dd.locked) {
				dd.rollme();
				dd.lockme();
			}
			dd.unlockable = false;
		}
		calculate();
	}

	public void attackPressed() {
		StartCoroutine("attackPressedCoroutine");
	}

	IEnumerator attackPressedCoroutine() {
		my_collider.enabled = true;		// eat all taps
		int newMobHP = mob_script.hp - adjusted_totals[0] - adjusted_totals[1] - adjusted_totals[2];
		mob_script.hp = Math.Min( Math.Max(0, newMobHP), mob_script.maxhp);
		health = Math.Min (maxhealth, health + adjusted_totals[3]);
		mob_object.transform.Translate(Vector3.up * 0.15f);
		if (mob_script.hp <= 0) {
			crossout_obj.SetActive(true);
		}
		yield return new WaitForSeconds((mob_script.hp <= 0) ? 0.5f : 0.25f);
		mob_object.transform.Translate(Vector3.down * 0.15f);
		crossout_obj.SetActive(false);
		if (mob_script.hp <= 0) {
			mob_script.levelup();
		} else {
			yield return new WaitForSeconds(1.0f);
			mob_object.transform.Translate(Vector3.down * 0.25f);
			health = Math.Max (0, health - mob_script.attack);
			if (health <= 0) {
				Application.LoadLevel("game_over_scene");
			}
			yield return new WaitForSeconds(0.25f);
			mob_object.transform.Translate(Vector3.up * 0.25f);
		}
		foreach ( die dd in dice_scripts ) {
			dd.unlockable = true;
			dd.unlockme();
			dd.rollme();
		}
		reroll_btn_script.pressable = true;
		calculate();
		my_collider.enabled = false;
	}

	void Update(){
		if (Input.GetKeyDown(KeyCode.Escape)) 
			Application.Quit(); 
	}

	void OnGUI () {
		calculate();
		textpos.x = 195f;
		textpos.y = 382f;
		textpos.width = textpos.height = 0f;
		if (adjustment_multipliers[0] <= 0) {
			GUI.Label(textpos, "  Imm.", redStyle);
		} else {
			GUI.Label(textpos, adjusted_totals[0].ToString(), bigfontstyle);
		}
		textpos.x = 305f;
		if (adjustment_multipliers[1] <= 0) {
			GUI.Label(textpos, "  Imm.", redStyle);
		} else {
			GUI.Label(textpos, adjusted_totals[1].ToString(), bigfontstyle);
		}
		textpos.x = 425f;
		if (adjustment_multipliers[2] <= 0) {
			GUI.Label(textpos, "  Imm.", redStyle);
		} else {
			GUI.Label(textpos, adjusted_totals[2].ToString(), bigfontstyle);
		}
		textpos.x = 195f;
		textpos.y = 446f;
		GUI.Label(textpos, adjusted_totals[3].ToString(), bigfontstyle);
		textpos.x = (Screen.width - Boxwidth) / 2f;
		textpos.y = Screen.height - 2f * Boxheight;
		textpos.width = Boxwidth;
		textpos.height = Boxheight;
		GUI.Box(textpos,  GUIContent.none, redStyle);
		textpos.width = Boxwidth * health / maxhealth;
		GUI.Box(textpos,  GUIContent.none, greenStyle);
		textpos.x = Screen.width / 2f;
		textpos.y = Screen.height - 35f;
		textpos.width = textpos.height = 0f;
		GUI.Label(textpos, health.ToString() + " / " + maxhealth.ToString(), smallfontstyle);
		textpos.y = 498f;
		GUI.Label(textpos, handname, bigfontstyle);
	}
}
