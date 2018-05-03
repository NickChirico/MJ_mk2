﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
	private Vector2 velocity;

	public float smoothTimeX;
	public float smoothTimeY;

	public Player_Controller player;

	void Start ()
	{
		player = FindObjectOfType<Player_Controller> ();
	}

	void Update ()
	{
		float posX = Mathf.SmoothDamp (transform.position.x, player.transform.position.x, ref velocity.x, smoothTimeX);
		//float posY = Mathf.SmoothDamp (transform.position.y, player.transform.position.y+2.5f, ref velocity.y, smoothTimeY); AT BOTTOM, NOT CENTER
		float posY = Mathf.SmoothDamp (transform.position.y, player.transform.position.y, ref velocity.y, smoothTimeY);


		this.transform.position = new Vector3 (posX, posY, transform.position.z);
	}
}
