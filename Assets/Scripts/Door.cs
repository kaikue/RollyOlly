using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
	public GameObject key;

	public void Open()
	{
		//TODO animate
		Destroy(gameObject);
	}
}
