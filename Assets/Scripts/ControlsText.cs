using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlsText : MonoBehaviour
{
	public string gamepadText;
	public string keyboardText;
	public TMPro.TextMeshProUGUI text;

	private GameManager gameManager;

	private void Start()
	{
		gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
	}
	
	private void Update()
	{
		text.text = gameManager.usingGamepad ? gamepadText : keyboardText;
	}
}
