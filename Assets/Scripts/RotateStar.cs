using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateStar : MonoBehaviour
{
	public float rotationSpeed;

	private RectTransform rectTransform;
	private float a;

	private void Update()
	{
		transform.rotation = Quaternion.AngleAxis(a, Vector3.forward);
		a += Time.deltaTime * rotationSpeed;
	}
}
