using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTrigger : MonoBehaviour
{

	Player_Controller player;

	public int FuelPerHit; // how much fuel you get per hit;

	void Start ()
	{
		player = FindObjectOfType<Player_Controller> ();
	}

	void Update ()
	{
		
	}

	void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.isTrigger != true && collision.CompareTag ("Enemy"))
		{
			//Debug.Log ("Enemy Hit!");

			// Activate the enemy's "Damage" function;
			collision.SendMessageUpwards("Damage");

			// player power meter increased
			player.fuel.CurrentValue += FuelPerHit;

		}
	}
}
