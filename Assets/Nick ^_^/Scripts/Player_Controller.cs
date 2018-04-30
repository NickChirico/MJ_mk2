using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller : MonoBehaviour
{

	private SpriteRenderer sr;
	private Rigidbody2D rb;
	private Animator anim;
	private TrailRenderer ultraTrail;
	private PlayerJump jumpScript;

	public Collider2D attack1_hitbox;
	private Collider2D playerHitbox;
	private Collider2D slideHitbox;


	[SerializeField]
	public Stat fuel;
	public Stat energy;


	public float speedX;
	public float jumpVelocity;
	public int numJumps;
	// value for how many double-jumps you get. 2 = double jump, 3 = triple jump etc.
	public float attackDuration;
	// how long the attack animation lasts;
	public int chargeRate;
	// how many frames pass to gain 1 charge (0 is once per frame)
	public float dashDuration;
	// how long the dash/slide last for.
	public float dashCD;
	// dash/slide cooldown
	public float ultraSpeed; // speed during ultra
	public int ultraExpendRate; // how many frames pass to expend 1 energy (0 is once per frame)
	public float wallSlideSpeed; // how fast you slide down a wall (gravity)

	public Vector2 dashRight = new Vector2 (14, 0);
	public Vector2 dashLeft = new Vector2 (14, 0);

	[HideInInspector]
	public bool onGround;
	[HideInInspector]
	public bool onWall;


	private float moveVelocity;
	private float speed;
	private float size;
	private float startingGrav;
	private bool facingRight = true;
	private int jumps;
	private bool canAttack = true;
	private bool isCharging = false;
	private int chargeCounter;
	private bool canDash = true;
	private int ultraCounter;
	private bool isUltra = false;
	private int direction = 1; // 1 = right, -1 = left;



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
		jumpScript = GetComponent<PlayerJump> ();
		size = transform.localScale.x;

		playerHitbox = GetComponent<CapsuleCollider2D> ();
		slideHitbox = GetComponent<BoxCollider2D> ();
		playerHitbox.enabled = true;
		slideHitbox.enabled = false;

		ultraTrail = GetComponent<TrailRenderer> ();
		ultraTrail.enabled = false;

		jumps = numJumps;
		speed = speedX;
		attack1_hitbox.enabled = false;
		fuel.CurrentValue = 0;
		energy.CurrentValue = 0;
		startingGrav = rb.gravityScale;

	}

	void Update ()
	{
		// Horizontal Walking Movement
		UpdateMovement ();

		// Jump Logic
		UpdateJump ();

		// Attacking
		UpdateAttack1 ();

		// Sliding
		UpdateSlide ();

		// Check if you are able to Charge Up
		UpdateCharging ();

		// Check if you are able to cast the ultra (moonwalk)
		UpdateUltra ();

	}

	private void UpdateMovement ()
	{
		if (!isCharging)
			moveVelocity = speed * Input.GetAxis ("Horizontal");

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

		if (facingRight)
			direction = 1;
		else
			direction = -1;
	}

	private void UpdateJump ()
	{
		if (!onWall && jumps > 0 && Input.GetKeyDown (KeyCode.Space))
		{
			rb.velocity = Vector2.up * jumpVelocity;
			jumps--;
		}

		if (onWall && Input.GetKeyDown (KeyCode.Space))
		{
			rb.velocity = new Vector2 ((direction * jumpVelocity), jumpVelocity);
		}
	}

	private void UpdateAttack1 ()
	{
		if (Input.GetKeyDown (KeyCode.Q))
		{
			if (canAttack)
				StartCoroutine (attack1 ());
		}
	}
		
	private void UpdateCharging ()
	{
		if (Input.GetKey (KeyCode.R))
		{
			if (fuel.CurrentValue > 0 && energy.CurrentValue != energy.MaxValue)
			{
				canDash = false;
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
			canDash = true;
		}
	}

	private void UpdateSlide ()
	{
		if (Input.GetKeyDown (KeyCode.E) && canDash && onGround)
		{
			//anim.SetInteger ("AttackState", 13);
			StartCoroutine (Dash (dashDuration));

		}
	}

	private void UpdateUltra()
	{
		if (Input.GetKey (KeyCode.T) && (energy.CurrentValue == energy.MaxValue))
		{
			anim.SetInteger ("AnimState", 4);
			ultraTrail.enabled = true;
			//StartCoroutine (Moonwalk ());
			isUltra = true;

			sr.color = Color.magenta;
			speed = ultraSpeed;

			/* THIS IS WHERE TO PUT LOGIC FOR INVINCIBILITY ETC
			 * 
			 * 
			 * 
			 */

		}

		if (isUltra && energy.CurrentValue > 0)
		{
			if (ultraCounter >= ultraExpendRate)
			{
				energy.CurrentValue--;
				ultraCounter = 0;
			}
			else
			{
				ultraCounter++;
			}
		}

		if (isUltra && energy.CurrentValue == 1)
		{
			anim.SetInteger ("AnimState", 0);
			ultraTrail.enabled = false;
			isUltra = false;

			sr.color = Color.white;
			speed = speedX;

			/* THIS IS WHERE TO PUT LOGIC TO END INVINCIBILITY ETC
			 * 
			 * 
			 * 
			 */
		}

	}

	private IEnumerator Dash (float dashDuration)
	{
		anim.SetInteger ("AnimState", 3);
		slideHitbox.enabled = true;
		playerHitbox.enabled = false;

		float time = 0f;
		canDash = false;

		bool facingRightDash = facingRight; // cannot change direction while dashing

		while (dashDuration > time)
		{ 
			//while theres still time left in the dash according to the dashLength
			time += Time.deltaTime;

			if (onGround)
			{
				if (facingRightDash)
				{
					rb.velocity = dashRight; 
				}
				else
				if (!facingRightDash)
				{
					rb.velocity = dashLeft;
				}
			}

			yield return 0; //go to next frame

		}

		/*if (onGround)
			anim.SetInteger ("AttackState", 0); // Back to Idle if you wait too long, or Air Idle
		else
			anim.SetInteger ("AttackState", 11);*/

		anim.SetInteger ("AnimState", 0);

		playerHitbox.enabled = true;
		slideHitbox.enabled = false;

		Time.timeScale = 1;
		yield return new WaitForSeconds (dashCD); //Cooldown time for being able to boost again

		canDash = true; //set back to true so that we can boost again.
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
		
	void OnCollisionEnter2D (Collision2D collisionInfo)
	{
		// Touching the Ground resets double jump;
												 
		if (collisionInfo.gameObject.tag == "Ground")
		{
			jumps = numJumps;
			onGround = true;

		}

		if (collisionInfo.gameObject.tag == "Wall")
		{
			onWall = true;
			float temp = 0;

			jumpScript.enabled = false;
			rb.velocity = new Vector2 (0, 0);
			rb.gravityScale = wallSlideSpeed;
			Debug.Log ("on Wall");

		}
	}

	void OnCollisionExit2D (Collision2D collisionInfo)
	{
		if (collisionInfo.gameObject.tag == "Wall")
		{
			onWall = false;
			jumpScript.enabled = true;
			rb.gravityScale = startingGrav;
		}
	}


}
