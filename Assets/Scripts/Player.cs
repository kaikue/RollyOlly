using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

	private Rigidbody2D rb;

	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
	}
	
	private void FixedUpdate()
	{
		float rotation = Input.GetAxis("Horizontal");
		rb.AddTorque(rotation);
	}
}
