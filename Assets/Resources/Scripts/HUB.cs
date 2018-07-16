/*
 * I found that I was including a lot of different objects, and it was getting pretty cumbersome.
 * So I made this class to instantiate other classes.
 * That way I just need to make a HUB object in other classes, and if I want to grab the cursor from the game,
 * instead of having to find the cursor, I can just do "cursor". I think this is slightly faster? I dont know.
 */ 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUB : MonoBehaviour{

	public int MAX_NUM_PLAYERS = 4;
	public const float SIZE = 4;
	public float MAX_X;
	public float MAX_Y;
	public const float SPACER = SIZE/1.5f;

	public Cursor CURSOR;
	public MoveMenuController MOVE_MENU_CONTROLLER;
	public MapGenerator MAP_GENERATOR;
	public CameraController CAMERA_CONTROLLER;
	public TopBar TOP_BAR;

	public float LAST_TIME_CANCEL;
	public float LAST_TIME_CONFIRM;
	public float LAST_TIME_DOWN;
	public float LAST_TIME_RIGHT;
	public float LAST_TIME_UP;
	public float LAST_TIME_LEFT;

	public bool gameOver;
	public Player[] players;
	public ArrayList mapTiles;
	public ArrayList mapTilePos;

	public ArrayList characters; //an arraylist of all of the characters
	public ArrayList enemyPositions;//grid space coordinates
	public ArrayList summonPositions;//gird space coordinates
	public ArrayList moveTilePositions;//grid space coordinates
	public ArrayList charsInRange;

	// This is called once the user hits play. Before the first frame
	void Awake()
	{
		CURSOR = GameObject.FindObjectOfType<Cursor>();
		MOVE_MENU_CONTROLLER = GameObject.FindObjectOfType<MoveMenuController>();
		MAP_GENERATOR = GameObject.FindObjectOfType<MapGenerator>();
		CAMERA_CONTROLLER = GameObject.FindObjectOfType<CameraController>();
		TOP_BAR = GameObject.FindObjectOfType<TopBar>();
		LAST_TIME_CANCEL = Time.time;
			LAST_TIME_CONFIRM = Time.time;
			LAST_TIME_UP = Time.time;
			LAST_TIME_DOWN = Time.time;
			LAST_TIME_LEFT = Time.time;
			LAST_TIME_RIGHT = Time.time;
			characters = new ArrayList();

			enemyPositions = new ArrayList();
			summonPositions = new ArrayList();
			moveTilePositions = new ArrayList();
			charsInRange = new ArrayList();
			players = new Player[2];
			gameOver = false;

			//get an understanding of the map
			for (int i = 0; i < players.Length; i++)
			{
				players[i] = new Player(i + 1);
				Summoner summoner = makeSummoner(i + 1);
				players[i].setSummoner(summoner.GetComponent<Summoner>());
				players[i].setGamePad(new Gamepad("keyboard"));
			}

			for (int i = players.Length; i < MAX_NUM_PLAYERS; i++)
			{
				GameObject.Destroy(GameObject.Find("PlayerDisplay" + (i + 1)));
			}
			switch (players.Length)
			{
				case 3:
					GameObject bar3 = GameObject.Find("PlayerDisplay1");
					bar3 = bar3.transform.Find("P1Bar").gameObject;
					bar3.transform.localScale = new Vector2(bar3.transform.localScale.x, 100);
					break;
				case 2:
					for (int i = 0; i < players.Length; i++)
					{
						GameObject bar2 = GameObject.Find("PlayerDisplay" + (i + 1));
						bar2 = bar2.transform.Find("P" + (i + 1) + "Bar").gameObject;
						bar2.transform.localScale = new Vector2(bar2.transform.localScale.x, 100);
					}
					break;
				default:
					break;
			}
		}
	//called once the game is loaded, on the first frame
	void Start()
	{
		mapTiles = new ArrayList();
		mapTilePos = new ArrayList();

		GameObject[] mt = GameObject.FindGameObjectsWithTag("MapTile");
		moveTilePositions = new ArrayList();
		for (int i = 0; i < mt.Length; i++)
		{
			mapTiles.Add(mt[i]);
			mapTilePos.Add(new Vector2(realRound(mt[i].transform.position.x / SPACER), realRound(mt[i].transform.position.y / SPACER)));
		}

		MAX_X = SPACER * (MAP_GENERATOR.boundsX - 1);
		MAX_Y = SPACER * (MAP_GENERATOR.boundsY - 1);
	}
	
	// Update is called once per frame
	void Update ()
	{
		characters.Clear();
		Character[] chars = GameObject.FindObjectsOfType<Character>();
		for (int i = 0; i < chars.Length; i++)
		{
			characters.Add((Character)chars[i]);
		}
	}

	public void moveCharacter(Character c, MapTile mt)
	{
		if (c.onMapTile != null && c.onMapTile.getCharacterOnTile() != null)
		{
			c.onMapTile.setCharacterOnTile(null);
		}
		mt.setCharacterOnTile(c);
		c.onMapTile = mt;
		c.transform.position = mt.transform.position;
	}

	public Character makeCharacter(string name, MapTile mt)
	{
		GameObject characterObj = ((GameObject)Instantiate(Resources.Load("Prefab/Characters/Units/" + name)));
		characterObj.transform.position = mt.transform.position;
		Character c = characterObj.GetComponent<Character>();
		c.CreateCharacter();
		characters.Add(c);
		return c;
	}

	public Summoner makeSummoner(int playerNum)
	{
		GameObject summoner = ((GameObject)Instantiate(Resources.Load("Prefab/Characters/Summoner")));
		Summoner s = summoner.GetComponent<Summoner>();
		s.setPlayerNum(playerNum);
		characters.Add(s);
		return s;
	}

	public bool currentSummonerCanSummon()
	{
		//must have the minimum mana to summon a unit
		if (Turns.getCurrentSummoner().mana < 2)
		{
			return false;
		}
		//must be places in range to summon to.
		findPlacesToSummon(summonPositions, Turns.getCurrentSummoner().summonRange, Turns.getCurrentSummoner().onMapTile);
		if (summonPositions.Count > 0)
		{
			return true;
		}
		return false;
	}


	//spawn move tiles into the spots that were given in previous function
	public void MakeTiles(string name)
	{
		int length = 0;
		switch (name)
		{
			case ("MoveTile"):
				length = moveTilePositions.Count;
				break;
			case ("EnemyTile"):
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
					obj.transform.position = ((MapTile)moveTilePositions[i]).transform.position;
					break;
				case ("EnemyTile"):
					obj.transform.position = ((MapTile)enemyPositions[i]).transform.position;
					break;
				case ("SummonTile"):
					obj.transform.position = ((MapTile)summonPositions[i]).transform.position;
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
		switch (name)
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
			default:
				print("This should not print.");
				break;
		}
	}

	//checks if all but 1 player has lost
	public bool GameOver()
	{
		int count = 0;
		foreach (Player p in players)
		{
			if (p.hasLost())
				count++;
		}
		return (count == players.Length - 1);
	}

	public ArrayList charactersInRange(Character c)
	{
		ArrayList inRangeCharacters = new ArrayList();
		MapTile mt = c.onMapTile;
		findCharactersInRange(mt, c.attkRange, inRangeCharacters);
		inRangeCharacters.Remove(c);
		return inRangeCharacters;
	}

	public bool enemyInRange(Character c)
	{
		ArrayList charsInRange = charactersInRange(c);
		enemyPositions.Clear();
		bool flag = false;
		print("Enemies in range: "+charsInRange.Count);
		for(int i = 0; i < charsInRange.Count; i++)
		{
			if (!((Character)charsInRange[i]).isAllyTo(c))
			{
				enemyPositions.Add(((Character)charsInRange[i]).onMapTile);
				flag = true;
			}
		}
		return flag;
	}

	public void findCharactersInRange(MapTile mt, int range, ArrayList inRangeCharacters)
	{
		if(range < 0 || mt == null)
		{
			return;
		}
		if(mt.getCharacterOnTile() != null && !inRangeCharacters.Contains(mt.getCharacterOnTile()))
		{
			inRangeCharacters.Add(mt.getCharacterOnTile());
		}
		if(mt.getNorthTile() != null)
			findCharactersInRange(mt.getNorthTile(), range - 1, inRangeCharacters);
		if (mt.getEastTile() != null)
			findCharactersInRange(mt.getEastTile(), range - 1, inRangeCharacters);
		if (mt.getSouthTile() != null)
			findCharactersInRange(mt.getSouthTile(), range - 1, inRangeCharacters);
		if (mt.getWestTile() != null)
			findCharactersInRange(mt.getWestTile(), range - 1, inRangeCharacters);
	}


	public void FindMoveTiles(int move, MapTile mt, Character charToMove)
	{
		moveTilePositions.Clear();
		FindMoveTile(move, mt, charToMove, false);
	}
	//recursively makes the tiles that show a user where they can move
	//THIS FUNCTION IS A MESS BUT IT FINALLY WORKS AND IS PRETTY QUICK!
	//x and y are in grid space
	public void FindMoveTile(int move, MapTile mt, Character charToMove,bool hasMoved)
	{
		//calculate cost to move their
		int cost;
		if (hasMoved)
		{
			cost = mt.moveCost;
		}
		//first step is free (Its the spot the character is already on)
		else
		{
			cost = 0;
		}

		//stop when you cant move
		if (move - cost < 0)
			return;
		

		//if there is a character on this square that is not your ally, you cannotmove there.
		if (hasMoved && mt.getCharacterOnTile() != null && mt.getCharacterOnTile().isAllyTo(charToMove))
		{
			return;
		}

		//dont overlap positions
		if (!moveTilePositions.Contains(mt))
		{
			//add this postion for spawning
			moveTilePositions.Add(mt);
		}

		if(mt.getNorthTile() != null)
			FindMoveTile(move - cost, mt.getNorthTile(), charToMove, true);
		if (mt.getEastTile() != null)
			FindMoveTile(move - cost, mt.getEastTile(), charToMove, true);
		if (mt.getSouthTile() != null)
			FindMoveTile(move - cost, mt.getSouthTile(), charToMove, true);
		if (mt.getWestTile() != null)
			FindMoveTile(move - cost, mt.getWestTile(), charToMove, true);
	}
	
	public void findPlacesToSummon(ArrayList summonPos, float range, MapTile mt)
	{
		//stop when you cant move
		if (range < 0)
			return;
		
		//dont overlap positions and only add if the tile has no character on it
		if (!summonPos.Contains(mt) && mt.getCharacterOnTile() == null)
		{
			//if so, add this postion for spawning
			summonPos.Add(mt);
		}

		//move up,left,right,down
		if (mt.getNorthTile() != null)
		{
			findPlacesToSummon(summonPos, range - 1, mt.getNorthTile());
		}
		if (mt.getEastTile() != null)
		{
			findPlacesToSummon(summonPos, range - 1, mt.getEastTile());
		}
		if (mt.getSouthTile() != null)
		{
			findPlacesToSummon(summonPos, range - 1, mt.getSouthTile());
		}
		if (mt.getWestTile() != null)
		{
			findPlacesToSummon(summonPos, range - 1, mt.getWestTile());
		}

	}

	//change grid coordinates to world coordinates
	protected Vector2 gridToWorld(Vector2 gridCoord)
	{
		return roundPosition(new Vector2(gridCoord.x * SPACER, gridCoord.y * SPACER));
	}

	//change world coordinates to grid coordinates
	protected Vector2 worldToGrid(Vector2 worldCoord)
	{
		int x = realRound(worldCoord.x / SPACER);
		int y = realRound(worldCoord.y / SPACER);
		return new Vector2(x, y);
	}

	protected int realRound(float f)
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

	/*
	 * When clicking to move, the position doesnt necessarily stay grid like
	 * This function converts a clicked location into a grid location.
	 * The function is currently being used to convert gridspace coordinates to world space.
	 * "pos" here is in world space
	 */
	protected Vector2 roundPosition(Vector2 pos)
	{
		float xPos = 0;
		float yPos = 0;

		//find the first position that is greater than the one clicked
		while (xPos < Mathf.Abs(pos.x))
		{
			xPos += SPACER;
		}
		while (yPos < Mathf.Abs(pos.y))
		{
			yPos += SPACER;
		}
		//look at the last position for both
		float lxPos = xPos - SPACER;
		float lyPos = yPos - SPACER;

		//set xPos to the closer position to wherever they clicked
		if (Mathf.Abs(lxPos - Mathf.Abs(pos.x)) < Mathf.Abs(xPos - Mathf.Abs(pos.x)))
		{
			xPos = lxPos;
		}
		//same for y
		if (Mathf.Abs(lyPos - Mathf.Abs(pos.y)) < Mathf.Abs(yPos - Mathf.Abs(pos.y)))
		{
			yPos = lyPos;
		}

		//the algorithm I used here only really works for finding the best positive numbers,
		//so I just made everything positive, then multiply by -1 if it was negative (y)
		if (pos.x < 0)
		{
			xPos *= -1;
		}
		if (pos.y < 0)
		{
			yPos *= -1;
		}
		Vector2 newPos = new Vector2(xPos, yPos);

		return newPos;
	}

}
