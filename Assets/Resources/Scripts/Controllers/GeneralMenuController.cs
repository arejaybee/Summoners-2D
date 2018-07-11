
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System;

public class SessionSettings{
	public int numPlayers = 2;
	public string mapChosen = "Multiplayer";

	//The number refers to which skin material to load ("seagullSkin#" is the name of each material)
	//the index is the player number
	public int[] skinNumbers = { 0, 0, 0, 0 };

	// 0-100 how pumped up are the jams?
	public float musicVolume = 100f;
}

public class GeneralMenuController : MonoBehaviour {
	public static SessionSettings settings = new SessionSettings();

	//rootMenu is the pause screen in-game and the main menu in the MainMenu scene
	public GameObject rootMenu, settingsMenu;

	// currentMenu is the currently open menu, if there is one
	// parentMenu is the menu from which this one was accessed, in the case of nesting
	protected GameObject currentMenu;

	protected EventSystem navigator;
	protected bool focused = false;

	// Use this for initialization
	void Start () {
		currentMenu = rootMenu;
		navigator = GameObject.Find("EventSystem").GetComponent<EventSystem>();
		GameObject.FindGameObjectWithTag("MainCamera").GetComponent<AudioSource>().volume = settings.musicVolume;
		focused = true;
	}

	public virtual void Focus(bool v){
		focused = v;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown("Cancel")){
			if (currentMenu == settingsMenu){
				ShowSettingsMenu(false);
			}
		}
	}

	//changes the scene to some new screen
	public void ChangeScene(string name){
		print(name);
		Scene myScene = SceneManager.GetSceneByName(name);
		if (!myScene.isLoaded){
			SceneManager.LoadScene(name);
		}

		if (myScene.isLoaded && myScene.IsValid()){
			SceneManager.SetActiveScene(myScene);
		}
	}

	public void Exit(){
		Application.Quit();
	}

	//Shows the pause menu
	public void ShowRootMenu(bool state){
		if (rootMenu != null){
			rootMenu.SetActive(state);
		}
	}

	//Displays the settings
	public void ShowSettingsMenu(bool state){
		if (settingsMenu != null){
			if (state){
				settingsMenu.SetActive(state);
				currentMenu = settingsMenu;
				settingsMenu.GetComponent<SettingsMenu>().Master = this;
				settingsMenu.GetComponent<SettingsMenu>().Focus(true);
				navigator.SetSelectedGameObject(settingsMenu.transform.GetChild(1).gameObject);
			}
			else{
				settingsMenu.GetComponent<SettingsMenu>().Focus(false);
				settingsMenu.SetActive(false);
				currentMenu = rootMenu;
				navigator.SetSelectedGameObject(rootMenu.transform.GetChild(1).gameObject);
			}
		}
	}

	public void changePlayerNumber(int num){
		settings.numPlayers = num;
	}

}
