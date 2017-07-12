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
	public float spacer;
	public ArrayList summonPositions;

	//Time since the last time these keys were pressed
	public float lastTimeX;
	public float lastTimeZ;
	public float lastTimeDown;
	public float lastTimeRight;
	public float lastTimeUp;
	public float lastTimeLeft;
	Character[] chars;


	private ArrayList mapTiles;
	private ArrayList mapTilePos;
	private ArrayList moveTilePositions;

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
		summonPositions = new ArrayList();

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

		spacer = cursor.size / 1.5f;
		mapTiles = new ArrayList();
		mapTilePos = new ArrayList();

		GameObject[] mt = GameObject.FindGameObjectsWithTag("MapTile");
		moveTilePositions = new ArrayList();
		for (int i = 0; i < mt.Length; i++)
		{
			mapTiles.Add(mt[i]);
			mapTilePos.Add(new Vector2(realRound(mt[i].transform.position.x / spacer), realRound(mt[i].transform.position.y / spacer)));
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
			//get context of the summoners (if they werent found yet)
			if(chars[i].name == "Summoner" && chars[i].playerNumber == 1)
				{
					summoner1 = (Summoner)chars[i];
				}
			if (chars[i].name == "Summoner" && chars[i].playerNumber == 2)
				{
					summoner2 = (Summoner)chars[i];
				}
			}
	}

	public Summoner getCurrentSummoner()
	{

		Summoner[] sm = GameObject.FindObjectsOfType<Summoner>();
		if (sm[0].playerNumber == turn.playerTurn)
		{
			return sm[0];
		}
		else
			return sm[1];
	}

	//given a position, find a character that is at the position
	public Character findCharacterAt(Vector2 pos)
	{
		for(int i = 0; i < chars.Length; i++)
		{
			if(chars[i].getIntX() == realRound(pos.x/spacer) && chars[i].getIntY() == realRound(pos.y/spacer))
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

	//recursively makes the tiles that show a user where they can move
	//THIS FUNCTION IS A MESS BUT IT FINALLY WORKS AND IS PRETTY QUICK!
	public void FindMoveTile(int move, int x, int y, Character charToMove, bool hasMoved)
	{
		//stop when you cant move
		if (move <= 0)
			return;
		
		//find the map tile at this spot
		int index = mapTilePos.IndexOf(new Vector2(x, y));

		//calculate cost to move their
		int cost;
		if (hasMoved)
		{
			cost = ((GameObject)mapTiles[index]).GetComponent<MapTile>().moveCost;
		}
		//first step is free (Its the spot the character is already on)
		else
		{
			cost = 0;
		}
		
		//dont overlap positions
		if (!moveTilePositions.Contains(new Vector2(x, y)))
		{
			//add this postion for spawning
			moveTilePositions.Add(new Vector2(x, y));
		}
		
		//move up,left,right,down
		if (y + 1 <= realRound(cursor.maxY / cursor.spacer))
		{

			FindMoveTile(move - cost, x, y + 1, charToMove, true);
		}
		if (x - 1 >= 0)
		{
			FindMoveTile(move - cost, x - 1, y, charToMove, true);
		}
		if (x + 1 <= realRound(cursor.maxX / cursor.spacer))
		{
			FindMoveTile(move - cost, x + 1, y, charToMove, true);
		}
		if (y - 1 >= 0)
		{
			FindMoveTile(move - cost, x, y - 1, charToMove, true);
		}
	}

	//spawn move tiles into the spots that were given in previous function
	public void MakeTiles(string name)
	{
		int length = 0;
		switch(name)
		{
			case ("MoveTile"):
				length = moveTilePositions.Count;
				break;
			case ("AttackTile"):
				length = enemyPositions.Count;
				break;
			case ("SummonTile"):
				length = summonPositions.Count;
				break;
		}
		for (int i = 0; i < length; i++)
		{
			//on the way back, make a thing here
			GameObject obj = (GameObject)Instantiate(Resources.Load("Prefab/Tiles/" + name));
			switch (name)
			{
				case ("MoveTile"):
					obj.transform.position = cursor.RoundPosition((Vector2)moveTilePositions[i] * cursor.spacer);
					break;
				case ("AttackTile"):
					obj.transform.position = cursor.RoundPosition((Vector2)enemyPositions[i] * cursor.spacer);
					break;
				case ("SummonTile"):
					print("Summon pos has a length of: " + summonPositions.Count);
					obj.transform.position = cursor.RoundPosition((Vector2)summonPositions[i] * cursor.spacer);
					break;
			}
		}
	}

	//Finds and deletes each move tile generated from trying to mvoe a character
	public void RemoveTiles(string name)
	{
		GameObject[] tiles = GameObject.FindGameObjectsWithTag(name);
		for (int i = 0; i < tiles.Length; i++)
		{
			GameObject.Destroy(tiles[i]);
		}
		switch(name)
		{
			case ("SummonTile"):
				summonPositions.Clear();
				break;
			case ("MoveTile"):
				moveTilePositions.Clear();
				break;
			case ("EnemyTile"):
				enemyPositions.Clear();
				break;
		}
	}

	public bool canSummon()
	{
		findPlacesToSummon(summonPositions,getCurrentSummoner().summonRange,getCurrentSummoner().getIntX(), getCurrentSummoner().getIntY());
		if(summonPositions.Count > 0)
		{
			return true;
		}
		return false;
	}

	//recursively makes the tiles that show a user where they can move
	//THIS FUNCTION IS A MESS BUT IT FINALLY WORKS AND IS PRETTY QUICK!
	void findPlacesToSummon(ArrayList characterPos, float range, int x, int y)
	{
		//stop when you cant move
		if (range < 0)
			return;

		print("Can we put a tile down at: " + x + " , " + y + " ? With a range of: "+range);
		//dont overlap positions and only add if the characterPossitino array doesnt have this position
		if (!characterPos.Contains(new Vector2(x, y)) && !characterPositions.Contains(cursor.RoundPosition(new Vector2(x*spacer, y*spacer))))
		//if(!characterPositions.Contains(cursor.RoundPosition(new Vector2(x * spacer, y * spacer))))
		{
			//if so, add this postion for spawning
			characterPos.Add(new Vector2(x, y));
		}


		//move up,left,right,down
		if (y + 1 <= realRound(cursor.maxY / cursor.spacer))
		{
			findPlacesToSummon(characterPos, range - 1, x, y + 1);
		}
		if (x - 1 >= 0)
		{
			findPlacesToSummon(characterPos, range - 1, x - 1, y);
		}
		if (x + 1 <= realRound(cursor.maxX / cursor.spacer))
		{
			findPlacesToSummon(characterPos, range - 1, x + 1, y);
		}
		if (y - 1 >= 0)
		{
			findPlacesToSummon(characterPos, range - 1, x, y - 1);
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
