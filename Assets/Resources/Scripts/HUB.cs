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
	public ArrayList enemyPositions;

	//Time since the last time these keys were pressed
	public float lastTimeX;
	public float lastTimeZ;
	public float lastTimeDown;
	public float lastTimeRight;
	public float lastTimeUp;
	public float lastTimeLeft;
	Character[] chars;

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
		enemyPositions = new ArrayList();
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
		chars = GameObject.FindObjectsOfType<Character>();
		for(int i = 0; i < chars.Length; i++)
		{
			characters.Add((Character)chars[i]);
			characterPositions.Add(cursor.RoundPosition(chars[i].transform.position));
			print("There is a " + chars[i].name + " at position: " + characterPositions[i]);
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

	//given a position, find a character that is at the position
	public Character findCharacterAt(Vector2 pos)
	{
		for(int i = 0; i < chars.Length; i++)
		{
			if(chars[i].getIntX() == realRound(pos.x/cursor.spacer) && chars[i].getIntY() == realRound(pos.y/cursor.spacer))
			{
				return chars[i];
			}
		}
		return null;
	}

	//if there are any enemies within the range of a character, return true
	public bool enemyInRange(Character c)
	{
		ArrayList mapPositions = new ArrayList();
		findEnemies(mapPositions, c.attkRange, c.getIntX(), c.getIntY(), c.playerNumber);
		for(int i = 0; i < mapPositions.Count; i++)
		{
			 if(charaHasSamePlayerNum(((Vector2)mapPositions[i]), c))
			{
				return true;
			}
		}
		return false;
	}

	//finds characters at a position and checks if they share a player num with a given palyer number
	bool charaHasSamePlayerNum(Vector2 pos, Character c)
	{
		bool flag = false;
		for(int i = 0; i < chars.Length; i++)
		{
			if(chars[i].getIntX() == pos.x && chars[i].getIntY() == pos.y)
			{
				if(chars[i].playerNumber != c.playerNumber)
				{
					flag = true;
					enemyPositions.Add((Vector2)chars[i].transform.position);
				}
			}
		}
		return flag;
	}
	//recursively makes the tiles that show a user where they can move
	//THIS FUNCTION IS A MESS BUT IT FINALLY WORKS AND IS PRETTY QUICK!
	void findEnemies(ArrayList characterPos, float range, int x, int y, float playerNumber)
	{
		//stop when you cant move
		if (range <= 0)
			return;

		//dont overlap positions
		if (!characterPos.Contains(new Vector2(x, y)))
		{
			//iif so, add this postion for spawning
			characterPos.Add(new Vector2(x, y));
		}


		//move up,left,right,down
		if (y + 1 <= realRound(cursor.maxY / cursor.spacer))
		{
			findEnemies(characterPos,range-1, x, y + 1, playerNumber);
		}
		if (x - 1 >= 0)
		{
			findEnemies(characterPos, range-1, x - 1, y, playerNumber);
		}
		if (x + 1 <= realRound(cursor.maxX / cursor.spacer))
		{
			findEnemies(characterPos, range-1, x + 1, y, playerNumber);
		}
		if (y - 1 >= 0)
		{
			findEnemies(characterPos, range-1, x, y - 1, playerNumber);
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
