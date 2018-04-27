using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class menuController : MonoBehaviour {

	bool inMenu = true;
	bool inGame = false;

	public GameObject titleText;
	public GameObject subTitles;
	public GameObject startPrompt;
	float blinkCounter = 0f;
	float blinkMax = 1.2f;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		if (inMenu) {
			//*********press X to start the game
			if (Input.GetKeyDown (KeyCode.JoystickButton0)) {
				inMenu = false;
				inGame = true;
				//disable gameobject holding menu art and stuff
			}
		}
	}

	void FixedUpdate ()	{
		if (inMenu) {
			//**********blinking startbutton
			startPrompt.SetActive (true);

			if (blinkCounter > blinkMax / 2) {
				startPrompt.SetActive (false);
			}

			blinkCounter += Time.deltaTime;
			if (blinkCounter > blinkMax) {
				blinkCounter = 0;
			}
		}
	}
}