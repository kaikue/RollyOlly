using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{

	private const float ROLL_SPEED = 10f;
	private const float JUMP_SPEED = 10f;

	private const float JUMP_GRACE_TIME = 0.1f;

	public Transform rotationTransform;
	public Transform spriteTransform;

	private Rigidbody2D rb;
	private Animator animator;
	private Vector2 lastGroundAngle;
	private HashSet<GameObject> collisions;
	private bool canJump = false;

	private HashSet<GameObject> keys;

	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
		collisions = new HashSet<GameObject>();
		keys = new HashSet<GameObject>();
	}

	private void Update()
	{
		Quaternion spriteRot = spriteTransform.rotation;
		rotationTransform.rotation = Quaternion.FromToRotation(Vector3.up, lastGroundAngle);
		spriteTransform.rotation = spriteRot;

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
		animator.SetTrigger("Bounce");
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
		Door door = collision.gameObject.GetComponent<Door>();
		if (door != null && keys.Contains(door.key))
		{
			door.Open();
		}

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

	private void OnTriggerEnter2D(Collider2D collider)
	{
		if (collider.gameObject.CompareTag("Flag"))
		{
			//TODO finish level menu
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
		}
		else if (collider.gameObject.CompareTag("Star"))
		{
			//TODO collect star
			Destroy(collider.gameObject);
		}
		else if (collider.gameObject.CompareTag("Key"))
		{
			keys.Add(collider.gameObject);
			Destroy(collider.gameObject);
		}
	}
}
