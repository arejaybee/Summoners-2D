using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SettingsMenu : MonoBehaviour {
	private Slider musicVolume;
	private Text mvDisplay;
	private AudioSource music;
	public GameObject defaultButton;
	private GameObject lastButton;
	// Use this for initialization
	void Start () {
		music = Component.FindObjectOfType<AudioSource>();
		mvDisplay = GameObject.Find("MusicText").GetComponent<Text>();
		musicVolume = GameObject.Find("MusicSlider").GetComponent<Slider>();
		musicVolume.onValueChanged.AddListener(MusicChange);
		musicVolume.value = (int)(Music.musicVolume / 5);
	}

	void Update()
	{
		if (EventSystem.current.currentSelectedGameObject.Equals(musicVolume.gameObject))
		{
			mvDisplay.color = Color.gray;
		}
		else
		{
			mvDisplay.color = Color.white;
		}
	}

	private void MusicChange(float val)
	{
		Music.musicVolume = musicVolume.value * 5;
		mvDisplay.text = "Music Volume: " + (int)Music.musicVolume;
		music.volume = Music.musicVolume / 100f;
	}

	public void OnOpen()
	{
		lastButton = EventSystem.current.currentSelectedGameObject;
		EventSystem.current.SetSelectedGameObject(defaultButton);
	}

	public void OnClose()
	{
		EventSystem.current.SetSelectedGameObject(lastButton);
	}
}
