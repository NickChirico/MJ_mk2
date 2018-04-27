using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	SpriteRenderer sr;
	Rigidbody2D rb;

	public float recoveryTime; // how much time until they can be hit again;

	// Use this for initialization
	void Start ()
	{
		sr = GetComponent<SpriteRenderer> ();
		rb = GetComponent<Rigidbody2D> ();
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}


	// Called when this enemy is attacked by player
	public void Damage ()
	{
		StartCoroutine (takeDamage ());
	}

	private IEnumerator takeDamage ()
	{
		sr.color = Color.red;

		yield return new WaitForSeconds(recoveryTime);
		sr.color = Color.white;

		yield return 0;
	}
}
