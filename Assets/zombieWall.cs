using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class zombieWall : MonoBehaviour {

	public GameObject menuControl;
	bool inMenu = true;

	public GameObject mjDeath;

	public GameObject mainCamera;
	public float wallSpeed = 0;
	public float speedI = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		inMenu = menuControl.GetComponent<menuController> ().inMenu;

		if (!inMenu) {
			mainCamera.transform.position += Vector3.right * wallSpeed * Time.deltaTime;
			wallSpeed += speedI;	//*****wallspeed increases over time
		}
	}

	void OnTriggerEnter (Collider player)	{
		mjDeath.SetActive (true);	//******mjDeath will always follow player, this means on death the player is destroyed and the animation object just appears; when it finishes the games reloads the scene
		Destroy (player.gameObject);
		//alternatively: player.SetActive (false);
	}
}
