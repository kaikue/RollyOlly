using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
	public float speed;
	public Transform[] nodes;

	private Rigidbody2D rb;
	private int targetNode = 0;

	private void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		NextNode();
	}

	private void NextNode()
	{
		transform.position = nodes[targetNode].position;
		int oldNode = targetNode;
		targetNode = (targetNode + 1) % nodes.Length;
		Vector2 diff = nodes[targetNode].position - nodes[oldNode].position;
		rb.velocity = diff.normalized * speed;

		float time = diff.magnitude / speed;
		Invoke("NextNode", time);
	}
}
