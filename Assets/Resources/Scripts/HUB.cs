/*
 * I found that I was including a lot of different objects, and it was getting pretty cumbersome.
 * So I made this class to instantiate other classes.
 * That way I just need to make a HUB object in other classes, and if I want to grab the cursor from the game,
 * instead of having to find the cursor, I can just do "HUB.cursor". I think this is slightly faster? I dont know.
 */ 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUB : MonoBehaviour {
	public Cursor cursor;
	public Turns turn;
	public MoveMenuHandler moveMenuHandler;
	public MapGenerator mapGenerator;
	public CameraController cam;
	public ArrayList characters;
	public ArrayList characterPositions;
	public Summoner summoner1;
	public Summoner summoner2;

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
		turn = FindObjectOfType<Turns>();
		Summoner[] s = FindObjectsOfType<Summoner>();
		if(s[0].playerNumber == 1)
		{
			summoner1 = s[0];
			summoner2 = s[1];
		}
		else
		{
			summoner1 = s[1];
			summoner2 = s[0];
		}
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

			//get context of the summoners (if they werent found yet)
			if(summoner1 == null)
			{
				if(chars[i].name == "Summoner" && chars[i].playerNumber == 1)
				{
					summoner1 = (Summoner)chars[i];
				}
			}

			if (summoner2 == null)
			{
				if (chars[i].name == "Summoner" && chars[i].playerNumber == 2)
				{
					summoner2 = (Summoner)chars[i];
				}
			}
		}
	}

	public Summoner getCurrentSummoner()
	{

		if(turn.playerTurn == 1)
		{
			return summoner1;
		}
		return summoner2;
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
