using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

	private const float ROLL_SPEED = 10f;
	private const float BOUNCE_SPEED = 10f;

	private const float BOUNCE_GRACE_TIME = 0.1f;
	private const float BOUNCE_COOLDOWN_TIME = 0.03f;

	private const float PITCH_ADJUST = 0.2f;

	public Transform rotationTransform;
	public Transform spriteTransform;
	public Sprite bouncySprite;

	public AudioClip bounceClip;
	public AudioClip landClip;
	public AudioClip collectClip;
	public AudioClip unlockClip;

	public GameObject collectParticlesPrefab;
	public GameObject moveParticlesPrefab;

	public AudioSource audioSrcRandom;
	public AudioSource audioSrcFixed;

	private Rigidbody2D rb;
	private Animator animator;
	private SpriteRenderer sr;
	private Vector2 lastGroundAngle;
	private Vector2 lastVel;
	private HashSet<GameObject> collisions;
	private bool canBounce = false;
	private bool bounceCooldown = false;
	private Sprite regularSprite;

	private HashSet<GameObject> keys;

	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
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
		bounceCooldown = true;
		StartCoroutine(BounceCooldown());
		animator.SetTrigger("Bounce");
		PlaySoundRandomized(bounceClip);
	}

	private IEnumerator BounceCooldown()
	{
		yield return new WaitForSeconds(BOUNCE_COOLDOWN_TIME);
		bounceCooldown = false;
	}

	private void PlaySoundRandomized(AudioClip audioClip)
	{
		audioSrcRandom.pitch = Random.Range(1 - PITCH_ADJUST, 1 + PITCH_ADJUST);
		audioSrcRandom.PlayOneShot(audioClip);
	}

	private void PlaySoundFixed(AudioClip audioClip)
	{
		audioSrcFixed.PlayOneShot(audioClip);
	}

	private void FixedUpdate()
	{
		float rotation = Input.GetAxis("Horizontal");
		if (rotation != 0)
		{
			rb.AddTorque(-rotation * ROLL_SPEED);
		}

		if (Input.GetButton("Jump") && canBounce && !bounceCooldown)
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
			PlaySoundFixed(unlockClip);
			door.Open();
			SpawnParticles(collectParticlesPrefab, collision.transform.position);
		}

		SpawnParticles(moveParticlesPrefab, collision.GetContact(0).point);

		PlaySoundRandomized(landClip);
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
		}
		else if (collider.gameObject.CompareTag("Star"))
		{
			SpawnParticles(collectParticlesPrefab, collider.transform.position);
			PlaySoundFixed(collectClip);
			GameManager gm = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
			gm.CollectStar(collider.gameObject);
			Destroy(collider.gameObject);
		}
		else if (collider.gameObject.CompareTag("Key"))
		{
			SpawnParticles(collectParticlesPrefab, collider.transform.position);
			PlaySoundFixed(collectClip);
			keys.Add(collider.gameObject);
			Destroy(collider.gameObject);
		}
	}

	private void SpawnParticles(GameObject particles, Vector3 position)
	{
		GameObject spawned = Instantiate(particles, position, Quaternion.identity);
		Destroy(spawned, 2.0f);
	}
}
