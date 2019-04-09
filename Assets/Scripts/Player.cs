using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{

	private const float ROLL_SPEED = 10f;
	private const float BOUNCE_SPEED = 10f;

	private const float BOUNCE_GRACE_TIME = 0.1f;

	private const float PITCH_ADJUST = 0.2f;

	public Transform rotationTransform;
	public Transform spriteTransform;
	public Sprite bouncySprite;

	public AudioClip bounceClip;
	public AudioClip collectClip;
	public AudioClip unlockClip;

	public GameObject particlesPrefab;

	private Rigidbody2D rb;
	private Animator animator;
	private SpriteRenderer sr;
	private AudioSource audioSrc;
	private Vector2 lastGroundAngle;
	private Vector2 lastVel;
	private HashSet<GameObject> collisions;
	private bool canBounce = false;
	private Sprite regularSprite;

	private HashSet<GameObject> keys;

	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
		audioSrc = GetComponent<AudioSource>();
		collisions = new HashSet<GameObject>();
		keys = new HashSet<GameObject>();
		sr = spriteTransform.GetComponent<SpriteRenderer>();
		regularSprite = sr.sprite;
	}

	private void Update()
	{
		Quaternion spriteRot = spriteTransform.rotation;
		rotationTransform.rotation = Quaternion.FromToRotation(Vector3.up, lastGroundAngle);
		spriteTransform.rotation = spriteRot;

		sr.sprite = Input.GetButton("Jump") ? bouncySprite : regularSprite;
	}

	private void Bounce()
	{
		//print("bounce");
		float incomingForce = Vector2.Dot(lastVel, lastGroundAngle.normalized);
		float defaultForce = BOUNCE_SPEED;
		//float force = Mathf.Max(incomingForce, defaultForce);
		float force = defaultForce;
		//print("incoming: " + incomingForce + ", default: " + defaultForce + ", force: " + force + " // velocity: " + lastVel);
		rb.AddForce(lastGroundAngle.normalized * force, ForceMode2D.Impulse);
		canBounce = false;
		animator.SetTrigger("Bounce");
		audioSrc.pitch = Random.Range(1 - PITCH_ADJUST, 1 + PITCH_ADJUST);
		audioSrc.PlayOneShot(bounceClip);
	}

	private void FixedUpdate()
	{
		float rotation = Input.GetAxis("Horizontal");
		if (rotation != 0)
		{
			rb.AddTorque(-rotation * ROLL_SPEED);
		}

		if (Input.GetButton("Jump") && canBounce)
		{
			Bounce();
		}
		//lastVel = rb.velocity;
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		//print("enter");
		Door door = collision.gameObject.GetComponent<Door>();
		if (door != null && keys.Contains(door.key))
		{
			audioSrc.pitch = 1;
			audioSrc.PlayOneShot(unlockClip);
			door.Open();
			Instantiate(particlesPrefab, collision.transform.position, Quaternion.identity);
		}
		
		lastVel = collision.relativeVelocity;
		collisions.Add(collision.gameObject);
		canBounce = true;
		OnCollide(collision);
	}

	private void OnCollisionStay2D(Collision2D collision)
	{
		//print("stay");
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
			Invoke("ClearJump", BOUNCE_GRACE_TIME);
		}
	}

	private void ClearJump()
	{
		if (collisions.Count == 0)
		{
			canBounce = false;
		}
	}

	private void OnTriggerEnter2D(Collider2D collider)
	{
		if (collider.gameObject.CompareTag("Flag"))
		{
			//TODO finish level menu
			GameManager gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
			gameManager.LevelComplete();
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
		}
		else if (collider.gameObject.CompareTag("Star"))
		{
			Instantiate(particlesPrefab, collider.transform.position, Quaternion.identity);
			audioSrc.pitch = 1;
			audioSrc.PlayOneShot(collectClip);
			GameManager gm = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
			gm.CollectStar(collider.gameObject);
			Destroy(collider.gameObject);
		}
		else if (collider.gameObject.CompareTag("Key"))
		{
			Instantiate(particlesPrefab, collider.transform.position, Quaternion.identity);
			audioSrc.pitch = 1;
			audioSrc.PlayOneShot(collectClip);
			keys.Add(collider.gameObject);
			Destroy(collider.gameObject);
		}
	}
}
