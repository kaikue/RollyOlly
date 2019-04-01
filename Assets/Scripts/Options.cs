using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Options : MonoBehaviour
{

	public Toggle timerToggle;

	private GameManager gameManager;

	private void Awake()
	{
		gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
	}

	private void Start()
	{
		//TODO event system stuff
		timerToggle.isOn = gameManager.IsTimerEnabled();
	}
}
