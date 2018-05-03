using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	SpriteRenderer sr;
	Rigidbody2D rb;
    Player_Controller player;

    public float speed;
    public int health;
    public int damagePerHit;

    private float startingSpeed;
    private bool patrolling = true;
    private int direction = 1;
    private float moveVelocity;

    private float size;

	public float recoveryTime; // how much time until they can be hit again;

	// Use this for initialization
	void Start ()
	{
        player = FindObjectOfType<Player_Controller>();
		sr = GetComponent<SpriteRenderer> ();
		rb = GetComponent<Rigidbody2D> ();
        startingSpeed = speed;

        size = this.transform.localScale.x;

        //StartCoroutine(patrol());
	}
	
	// Update is called once per frame
	void Update ()
	{
        if (health <= 0)
        {
            //Destroy (this.gameObject);
            kill();
        }

        // Patrol movement
        moveVelocity = speed * direction;
        rb.velocity = new Vector2(moveVelocity, rb.velocity.y);


        // Dont get rid of this...ever
    }


	// Called when this enemy is attacked by player
	public void Damage ()
	{
		StartCoroutine (takeDamage ());
	}

	private IEnumerator takeDamage ()
	{
		sr.color = Color.red;
        speed = speed*-1 +1;
        health -= damagePerHit;

		yield return new WaitForSeconds(recoveryTime);
		sr.color = Color.white;
        speed = startingSpeed;

		yield return 0;
	}

    void turn()
    {
        if (direction == 1)
        {
            direction = -1;
            transform.localScale = new Vector3(-1*size, size, 0);
        }
        else
        if (direction == -1)
        {
            direction = 1;
            transform.localScale = new Vector3(size, size, 0);
        }
    }

    private IEnumerator patrol()
    {
        yield return 0;
    }

    public void kill()
    {
        StartCoroutine(die());
    }

    private IEnumerator die()
    {
        speed = 0;
        patrolling = false;
        Physics2D.IgnoreCollision(this.GetComponent<Collider2D>(), player.GetComponent<Collider2D>(), true);
        sr.color = Color.black;


        yield return new WaitForSeconds(0.25f);
        Destroy(this.gameObject);
    }

    void OnTriggerEnter2D(Collider2D collisionInfo)
    {
        /*if (collisionInfo.gameObject.tag == "Ground")
        {
            jumps = numJumps;
            onGround = true;

            if (anim.GetInteger("AnimState") == 11)
                anim.SetInteger("AnimState", 0);
        }*/

        if (collisionInfo.gameObject.tag == "Block") 
        {
            turn();
        }
        if (collisionInfo.gameObject.tag == "Ground") 
        {
            turn();
        }
        if (collisionInfo.gameObject.tag == "Wall")
        {
            turn();
        }
        if (collisionInfo.gameObject.tag == "Edge")
        {
            turn();
        }
    }
}
