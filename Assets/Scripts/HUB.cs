using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUB : MonoBehaviour {
	public Cursor cursor;
	public MoveMenuHandler moveMenuHandler;
	public MapGenerator mapGenerator;
	public CameraController cam;

	//Time since the last time these keys were pressed
	public float lastTimeX;
	public float lastTimeZ;
	public float lastTimeDown;
	public float lastTimeRight;
	public float lastTimeUp;
	public float lastTimeLeft;
	// Use this for initialization
	void Start ()
	{
		cursor = GameObject.FindObjectOfType<Cursor>();
		moveMenuHandler = GameObject.FindObjectOfType<MoveMenuHandler>();
		mapGenerator = GameObject.FindObjectOfType<MapGenerator>();
		cam = GameObject.FindObjectOfType<CameraController>();
		lastTimeX = Time.time;
		lastTimeDown = Time.time;
		lastTimeUp = Time.time;
		lastTimeZ = Time.time;
		lastTimeLeft = Time.time;
		lastTimeRight = Time.time;
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}
}
