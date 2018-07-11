using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : GeneralMenuController {

	private Button back;
	private Slider musicVolume;
	private Text mvDisplay;
	private AudioSource music;

	public GeneralMenuController Master{
		set{
			if (master == null){
				master = value;
			}
		}
		get { return master; }
	}
	private GeneralMenuController master;

	// Use this for initialization
	void Start (){
		back = GetComponentInChildren<Button> ();
		back.onClick.AddListener (ExitSettings);


		music = Component.FindObjectOfType<AudioSource> ();
		mvDisplay = transform.Find ("MusicText").GetComponent<Text> ();
		musicVolume = transform.Find ("MusicSlider").GetComponent<Slider> ();
		musicVolume.onValueChanged.AddListener (MusicChange);
		musicVolume.value = (int) (settings.musicVolume / 5);
	}

	private void ExitSettings (){
		master.ShowRootMenu (true);
		master.ShowSettingsMenu (false);
	}

	private void MusicChange (float val){
		settings.musicVolume = musicVolume.value * 5;
		mvDisplay.text = "Music Volume: " + (int) settings.musicVolume;
		music.volume = settings.musicVolume / 100f;
	}
}
