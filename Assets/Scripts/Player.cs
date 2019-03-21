using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

	private const float ROLL_SPEED = 10f;
	private const float JUMP_SPEED = 10f;

	private const float JUMP_GRACE_TIME = 0.1f;

	private Rigidbody2D rb;
	private Vector2 lastGroundAngle;
	private HashSet<GameObject> collisions;
	private bool canJump = false;

	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
		collisions = new HashSet<GameObject>();
	}

	private void Update()
	{
		if (Input.GetButton("Jump")) //TODO: visual indicator for bouncy
		{
			if (canJump)
			{
				Jump();
			}
		}
	}

	private void Jump()
	{
		rb.AddForce(lastGroundAngle.normalized * JUMP_SPEED, ForceMode2D.Impulse);
		canJump = false;
	}

	private void FixedUpdate()
	{
		float rotation = Input.GetAxis("Horizontal");
		if (rotation != 0)
		{
			rb.AddTorque(-rotation * ROLL_SPEED);
		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		collisions.Add(collision.gameObject);
		canJump = true;
		OnCollide(collision);
	}

	private void OnCollisionStay2D(Collision2D collision)
	{
		OnCollide(collision);
	}

	private void OnCollide(Collision2D collision)
	{
		lastGroundAngle = collision.GetContact(0).normal;
	}

	private void OnCollisionExit2D(Collision2D collision)
	{
		collisions.Remove(collision.gameObject);
		if (collisions.Count == 0)
		{
			Invoke("ClearJump", JUMP_GRACE_TIME);
		}
	}

	private void ClearJump()
	{
		if (collisions.Count == 0)
		{
			canJump = false;
		}
	}
}
