using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class menuController : MonoBehaviour {

	public bool inMenu = true;

	public GameObject titleText;
	public GameObject subTitles;
	public GameObject startPrompt;
	float blinkCounter = 0f;
	float blinkMax = 1.2f;

	float creditsSpeed = 0;
	public GameObject credits;
	public Vector3 creditsOnScreen = new Vector3 (0, 0, 0);
	public Vector3 creditsOffScreen = new Vector3 (0, 0, 0);

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		if (inMenu) {
			//*********hold R1 to see credits
			if (Input.GetKey (KeyCode.JoystickButton5)) {
				creditsSpeed = (Mathf.Abs (creditsOnScreen.x - credits.transform.position.x)) * 2.75f + 5f;

				if (credits.transform.position.x > creditsOnScreen.x) { 
					credits.transform.position -= Vector3.right * creditsSpeed * Time.deltaTime;
				}
			} else {
				creditsSpeed = (Mathf.Abs (creditsOffScreen.x - credits.transform.position.x)) * 2.75f + 5f;

				if (credits.transform.position.x < creditsOffScreen.x) { 
					credits.transform.position += Vector3.right * creditsSpeed * Time.deltaTime;
				} else
					return;
			}

			//*********press X to start the game
			if (Input.GetKeyDown (KeyCode.JoystickButton0)) {
				inMenu = false;
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