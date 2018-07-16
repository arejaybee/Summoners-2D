using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PauseMenuController : MonoBehaviour {

	public static bool isPaused = false;
	public GameObject pauseMenuUI;
	public GameObject defaultButton;
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			if (isPaused)
			{
				Resume();
			}
			else
			{
				Pause();
			}
		}
	}

	public void Resume()
	{
		pauseMenuUI.SetActive(false);
		Time.timeScale = 1f;
		isPaused = false;
	}

	public void Pause()
	{
		pauseMenuUI.SetActive(true);
		EventSystem.current.SetSelectedGameObject(defaultButton);
		Time.timeScale = 0f;
		isPaused = true;	
	}
}
