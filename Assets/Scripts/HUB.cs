using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUB : MonoBehaviour {
	public Cursor cursor;
	public MoveMenuHandler moveMenuHandler;
	public MapGenerator mapGenerator;
	public CameraController cam;
	public ArrayList characters;
	public ArrayList characterPositions;

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
		characters = new ArrayList();
		characterPositions = new ArrayList();
	}
	
	// Update is called once per frame
	void Update ()
	{
		characterPositions.Clear();
		characters.Clear();
		Character[] chars = GameObject.FindObjectsOfType<Character>();
		for(int i = 0; i < chars.Length; i++)
		{
			characters.Add((Character)chars[i]);
			characterPositions.Add(cursor.RoundPosition(chars[i].transform.position));
		}
	}

	int realRound(float f)
	{
		float tempF = f;
		tempF -= (int)f;
		tempF *= 10;
		if ((int)tempF > 4)
		{
			return (int)f + 1;
		}
		return (int)f;
	}
}
