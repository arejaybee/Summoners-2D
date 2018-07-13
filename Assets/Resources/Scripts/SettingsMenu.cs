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

	// Use this for initialization
	void Start () {
		music = Component.FindObjectOfType<AudioSource>();
		mvDisplay = transform.Find("MusicText").GetComponent<Text>();
		musicVolume = transform.Find("MusicSlider").GetComponent<Slider>();
		musicVolume.onValueChanged.AddListener(MusicChange);
		musicVolume.value = (int)(Music.musicVolume / 5);
	}

	private void MusicChange(float val)
	{
		Music.musicVolume = musicVolume.value * 5;
		mvDisplay.text = "Music Volume: " + (int)Music.musicVolume;
		music.volume = Music.musicVolume / 100f;
	}

	public void OnOpen()
	{
		EventSystem.current.SetSelectedGameObject(defaultButton);
	}
}
