using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
	public GameObject optionsMenuPrefab;
	public GameObject creditsMenuPrefab;
	public GameObject loadingText;

	private GameManager gameManager;

	private void Awake()
	{
		Time.timeScale = 0;
		gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
	}

	public void StartGame()
	{
		Time.timeScale = 1;
		loadingText.SetActive(true);
		NextLevel();
	}

	public void Options()
	{
		GameObject optionsMenu = Instantiate(optionsMenuPrefab);
		optionsMenu.GetComponentInChildren<Options>().parentMenu = gameObject;
		gameObject.SetActive(false);
	}

	public void Credits()
	{
		GameObject creditsMenu = Instantiate(creditsMenuPrefab);
		creditsMenu.GetComponentInChildren<Options>().parentMenu = gameObject;
		gameObject.SetActive(false);
	}

	public void Resume()
	{
		gameManager.Resume();
	}

	public void Close()
	{
		Destroy(gameObject);
	}

	public void QuitToTitle()
	{
		gameManager.ResetTimer();
		SceneManager.LoadScene(0);
	}

    public void Exit()
	{
		Application.Quit();
	}

	public void ShowTimer(bool show)
	{
		gameManager.ShowTimer(show);
	}

	public void NextLevel()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}

	public void RestartLevel()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}
}
