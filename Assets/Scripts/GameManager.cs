﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	public GameObject pauseMenuPrefab;
	public TMPro.TextMeshProUGUI timerText;
	public AudioClip completeClip;

	public GameObject levelCompletePrefab;

	[HideInInspector]
	public bool usingGamepad = false;

	private bool paused = false;
	private GameObject pauseMenu;

	private bool timerEnabled = false;
	private float gameTime;
	private HashSet<GameObject> starsCollected;

	private AudioSource audioSrc;

	private void Awake()
	{
		GameObject[] others = GameObject.FindGameObjectsWithTag("GameManager");

		if (others.Length > 1)
		{
			Destroy(gameObject);
			return;
		}

		DontDestroyOnLoad(gameObject);
		gameTime = 0;
		starsCollected = new HashSet<GameObject>();
		audioSrc = GetComponent<AudioSource>();

	}
	
	private void OnEnable()
	{
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	private void OnDisable()
	{
		SceneManager.sceneLoaded -= OnSceneLoaded;
	}

	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		CheckWin();
		RefreshTimer();
		PlayMusic();
	}
	
	private void RefreshTimer()
	{
		timerText.enabled = timerEnabled;
	}

	private void PlayMusic()
	{
		GameObject liObj = GameObject.FindWithTag("LevelInfo");
		if (liObj != null)
		{
			LevelInfo li = liObj.GetComponent<LevelInfo>();
			AudioClip newMus = li.backgroundMusic;
			if (newMus != audioSrc.clip)
			{
				audioSrc.clip = newMus;
				audioSrc.Play();
			}
		}
	}

	private void CheckWin()
	{
		GameObject endObj = GameObject.FindWithTag("EndInfo");
		if (endObj != null)
		{
			EndInfo endInfo = endObj.GetComponent<EndInfo>();
			endInfo.SetInfo(GetTimeString(), starsCollected.Count);
			timerEnabled = false;
		}
	}

	private string GetTimeString()
	{
		TimeSpan ts = TimeSpan.FromSeconds(gameTime);
		return ts.ToString(@"hh\:mm\:ss\.f");
	}

	private void Update()
	{
		gameTime += Time.deltaTime;
		
		timerText.text = "Time: " + GetTimeString();

		if (Input.GetKey(KeyCode.JoystickButton0))
		{
			usingGamepad = true;
		}
		else if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.Return) || Input.GetKey(KeyCode.Mouse0))
		{
			usingGamepad = false;
		}

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

	public void LevelComplete()
	{
		Instantiate(levelCompletePrefab);
		audioSrc.PlayOneShot(completeClip);
	}

	public void ResetTimer()
	{
		gameTime = 0;
	}
}
