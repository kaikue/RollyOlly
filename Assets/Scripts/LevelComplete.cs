using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelComplete : MonoBehaviour
{

	private const float WAIT_TIME = 2.0f;

	private void Awake()
	{
		Time.timeScale = 0;
		StartCoroutine(Wait());
	}
	
	private IEnumerator Wait()
	{
		float start = Time.realtimeSinceStartup;
		while (Time.realtimeSinceStartup < start + WAIT_TIME)
		{
			yield return null;
		}
		NextLevel();
	}

	private void NextLevel()
	{
		Time.timeScale = 1;
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}
}
