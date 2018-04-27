using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller : MonoBehaviour
{

	private SpriteRenderer sr;
	private Rigidbody2D rb;
	private Animator anim;

	public Collider2D attack1_hitbox;

	[SerializeField]
	public Stat fuel;
	public Stat energy;


	public float speedX;
	public float jumpVelocity; 
	public int numJumps; // value for how many double-jumps you get. 2 = double jump, 3 = triple jump etc.
	public float attackDuration; // how long the attack animation lasts;
	public int chargeRate; // how many frames pass to gain 1 charge (0 is once per frame)

	private float moveVelocity;
	private float size;
	private bool facingRight = true;
	private int jumps;
	private bool canAttack = true;
	private bool isCharging = false;
	private int chargeCounter;


	private void Awake ()
	{
		fuel.Initialize ();
		energy.Initialize ();
	}

	void Start ()	
	{
		sr = GetComponent<SpriteRenderer> ();
		rb = GetComponent<Rigidbody2D> ();
		anim = GetComponent<Animator> ();
		size = transform.localScale.x;

		jumps = numJumps;
		attack1_hitbox.enabled = false;
		fuel.CurrentValue = 0;
		energy.CurrentValue = 0;

	}
	
	void Update ()
	{
		// Horizontal Walking Movement
		UpdateMovement();
		// Jump Logic
		UpdateJump();
		// Attacking
		UpdateAttack1 ();
		// Check if you are able to Charge Up
		UpdateCharging ();

	}

	private void UpdateMovement()
	{
		if(!isCharging)
			moveVelocity = speedX * Input.GetAxis ("Horizontal");

		if (moveVelocity > 0)
		{
			// Flip sprite when you turn around;
			transform.localScale = new Vector3 (size, size, size);
			facingRight = true;
		}
		else
		if (moveVelocity < 0)
		{
			// Flip sprite when you turn around;
			transform.localScale = new Vector3 (-size, size, size);
			facingRight = false;
		}

		rb.velocity = new Vector2 (moveVelocity, rb.velocity.y);
	}

	private void UpdateJump()
	{
		if (jumps > 0 && Input.GetKeyDown (KeyCode.Space))
		{
			rb.velocity = Vector2.up * jumpVelocity;
			jumps--;
		}
	}

	private void UpdateAttack1()
	{
		if(Input.GetKeyDown(KeyCode.Q))
		{
			if(canAttack)
				StartCoroutine (attack1 ());
		}
	}

	private IEnumerator attack1 ()
	{
		anim.SetInteger ("AnimState", 1);
		canAttack = false;
		attack1_hitbox.enabled = true;

		yield return new WaitForSeconds (attackDuration);
		anim.SetInteger ("AnimState", 0);
		canAttack = true;
		attack1_hitbox.enabled = false;


		// can put a cooldown on the attack HERE

		yield return 0;
	}

	private void UpdateCharging()
	{
		if (Input.GetKey (KeyCode.R))
		{
			if (fuel.CurrentValue > 0 && energy.CurrentValue != energy.MaxValue)
			{
				moveVelocity = 0;
				isCharging = true;

				anim.SetInteger ("AnimState", 2);

				if (chargeCounter >= chargeRate)
				{
					fuel.CurrentValue--;
					energy.CurrentValue++;
					chargeCounter = 0;
				}
				else
				{
					chargeCounter++;
				}
			}
			else
			{
				anim.SetInteger ("AnimState", 0);
				isCharging = false;
			}

		}
		else
		{
			isCharging = false;
		}

		if (Input.GetKeyUp (KeyCode.R))
		{
			anim.SetInteger ("AnimState", 0);
		}
	}


	void OnCollisionEnter2D (Collision2D collisionInfo)
	{
		// Touching the Ground resets double jump;
												 
		if (collisionInfo.gameObject.tag == "Ground")
		{
			jumps = numJumps;

		}
	}



}
