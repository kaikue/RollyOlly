using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateUI : MonoBehaviour
{
	public float rotationSpeed;

	private RectTransform rectTransform;
	private float a;

	private void Start()
	{
		rectTransform = GetComponent<RectTransform>();
	}
	
	private void OnGUI()
	{
		rectTransform.rotation = Quaternion.AngleAxis(a, Vector3.forward);
		a += rotationSpeed;
	}
}
