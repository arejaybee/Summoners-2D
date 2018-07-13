using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MainMenuController : MonoBehaviour {
	public GameObject defaultButton;

	public void playMultiplayer(){
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +1);
	}
	public void playSinglePlayer(){
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2); //TODO make a scene for single player and put it after multiplayer stuff in the build.
	}
	public void loadSettingsMenu(){

	}
	public void quitGame(){
		Application.Quit();
	}
	public void OnOpen()
	{
		EventSystem.current.SetSelectedGameObject(defaultButton);
	}
}
