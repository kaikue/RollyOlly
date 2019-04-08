using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public GameObject pauseMenuPrefab;
	public TMPro.TextMeshProUGUI timerText;

	private bool paused = false;
	private GameObject pauseMenu;

	private bool timerEnabled = false;
	private float gameTime;
	private HashSet<GameObject> starsCollected;

	private void Awake()
	{
		GameObject[] others = GameObject.FindGameObjectsWithTag("GameManager");

		if (others.Length > 1)
		{
			Destroy(gameObject);
		}

		DontDestroyOnLoad(gameObject);
		gameTime = 0;
		starsCollected = new HashSet<GameObject>();
	}

	private void Start()
	{
		RefreshTimer();
	}

	private void RefreshTimer()
	{
		timerText.enabled = timerEnabled;
	}

	private void Update()
	{
		gameTime += Time.deltaTime;
		TimeSpan ts = TimeSpan.FromSeconds(gameTime);
		string timeString = ts.ToString(@"hh\:mm\:ss\.f");
		timerText.text = "Time: " + timeString;

		if (Input.GetButtonDown("Pause"))
		{
			if (paused)
			{
				Resume();
			}
			else
			{
				Pause();
			}
		}
	}

	private void Pause()
	{
		pauseMenu = Instantiate(pauseMenuPrefab);
		paused = true;
	}

	public void Resume()
	{
		Destroy(pauseMenu);
		Time.timeScale = 1;
		paused = false;
	}

	public void ShowTimer(bool show)
	{
		timerEnabled = show;
		RefreshTimer();
	}

	public bool IsTimerEnabled()
	{
		return timerEnabled;
	}

	public void CollectStar(GameObject star)
	{
		starsCollected.Add(star);
	}
}
