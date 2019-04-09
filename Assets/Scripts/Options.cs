using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Options : MonoBehaviour
{

	public Toggle timerToggle;
	public GameObject selected;

	[HideInInspector]
	public GameObject parentMenu;

	private GameManager gameManager;
	//private EventSystem eventSystem;
	//private GameObject lastSelected;

	private void Awake()
	{
		gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
		//eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
	}

	private void Start()
	{
		//lastSelected = eventSystem.currentSelectedGameObject;
		//eventSystem.SetSelectedGameObject(selected);
		if (timerToggle != null)
		{
			timerToggle.isOn = gameManager.IsTimerEnabled();
		}
	}

	public void Close()
	{
		parentMenu.SetActive(true);
		//eventSystem.SetSelectedGameObject(lastSelected);
		Destroy(gameObject);
	}
}
