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
	public const int MAX_NUM_PLAYERS = 4;
	public Cursor cursor;
	public Turns turn;
	public Player[] players;
	public MoveMenuHandler moveMenuHandler;
	public MapGenerator mapGenerator;
	public CameraController cam;
	public ArrayList characters; //an arraylist of all of the characters
	public ArrayList characterPositions;//world space coordinates
	public ArrayList enemyPositions;//grid space coordinates
	public float spacer;
	public ArrayList summonPositions;//gird space coordinates
	public ArrayList moveTilePositions;//grid space coordinates
	public ArrayList charsInRange;

	//Time since the last time these keys were pressed
	public float lastTimeX;
	public float lastTimeZ;
	public float lastTimeDown;
	public float lastTimeRight;
	public float lastTimeUp;
	public float lastTimeLeft;


	private ArrayList mapTiles;
	private ArrayList mapTilePos;
	private bool gameOver;


	// This is called once the user hits play. Before the first frame
	void Awake()
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
		enemyPositions = new ArrayList();
		summonPositions = new ArrayList();
		charsInRange = new ArrayList();
		players = new Player[2];
		gameOver = false;
		//get an understanding of the map
		spacer = cursor.transform.localScale.x / 1.5f;
		for (int i = 0; i < players.Length; i++)
		{
			players[i] = new Player(i+1);
			GameObject summoner = ((GameObject)Instantiate(Resources.Load("Prefab/Characters/Summoner")));
			summoner.GetComponent<Summoner>().setPlayerNum(i + 1);
			players[i].setSummoner(summoner.GetComponent<Summoner>());
		}
		if(Input.GetJoystickNames().Length > 0)
		{
			players[0].setGamePad(new Gamepad(Input.GetJoystickNames()[0], 1));
		}

		for (int i = players.Length; i < MAX_NUM_PLAYERS; i++)
		{
			GameObject.Destroy(GameObject.Find("PlayerDisplay" + (i+1)));
		}
		switch(players.Length){
			case 3:
				GameObject bar3 = GameObject.Find("PlayerDisplay1");
				bar3 = bar3.transform.Find("P1Bar").gameObject;
				bar3.transform.localScale = new Vector2(bar3.transform.localScale.x, 100);
				break;
			case 2:
				for(int i = 0; i < players.Length; i++)
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
			mapTilePos.Add(new Vector2(realRound(mt[i].transform.position.x / spacer), realRound(mt[i].transform.position.y / spacer)));
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		characterPositions.Clear();
		characters.Clear();
		Character[] chars = GameObject.FindObjectsOfType<Character>();
		for (int i = 0; i < chars.Length; i++)
		{
			characters.Add((Character)chars[i]);
			characterPositions.Add(roundPosition(chars[i].transform.position));
		}
	}

	public Summoner getCurrentSummoner()
	{
		return turn.getPlayer().getSummoner();
	}

	//checks if all but 1 player has lost
	public bool GameOver()
	{
		int count = 0;
		foreach(Player p in players)
		{
			if (p.hasLost())
				count++;
		}
		return (count == players.Length - 1);
	}

	//given a position, find a character that is at the position
	//where pos is in world space
	public Character findCharacterAt(Vector2 pos)
	{
		pos = roundPosition(pos);
		for (int i = 0; i < characterPositions.Count; i++)
		{
			//print("There is a " + ((Character)characters[i]).name + " at " + (Vector2)characterPositions[i]);
			if((Vector2)characterPositions[i] == pos)
			{
				return ((Character)characters[i]);
			}
		}
		return null;
	}

	/*
	 * Given a character, returns all characters that share a position with it (this should be 0)
	 */ 
	public ArrayList findCharactersOn(Character c)
	{
		ArrayList charactersToReturn = new ArrayList();
		for (int i = 0; i < characters.Count; i++)
		{
			//if the is a character that isnt c
			if ((Character)characters[i] != c)
			{
				//if it occupies the same place as c
				if ((Vector2)characterPositions[i] == roundPosition(new Vector2(c.getIntX(), c.getIntY())))
				{
					charactersToReturn.Add((Character)characters[i]);
				}
			}
		}
		return charactersToReturn;
	}

	//if there are any enemies within the range of a character, return true
	public bool enemyInRange(Character c)
	{
		bool flag = false;
		ArrayList mapPositions = new ArrayList();

		//fill mapPositions with positions of all characters within the range
		findCharacters(mapPositions, c.attkRange+1, c.getIntX(), c.getIntY(), c.playerNumber); 

		//fills in the enemyPositions global variable when used. also sets a flag to let the "attack" option show
		for(int i = 0; i < mapPositions.Count; i++)
		{
			if(findCharactersWithDifferentPlayerNumber(((Vector2)mapPositions[i]), c))
			{
				flag = true;
			}
		}
		return flag;
	}

	//if there are any characters within the range of a character, return true
	//This is used for "speak" so I excluded Summoner explicitly
	public ArrayList charactersInRange(Character c)
	{
		ArrayList mapPositions = new ArrayList();
		findCharacters(mapPositions, c.attkRange + 1, c.getIntX(), c.getIntY(), c.playerNumber);

		charsInRange.Clear();
		for(int i = 0; i < characters.Count; i++)
		{
			Character chara = (Character)characters[i];

			//if the character isnt c, isnt a summoner, and was within the range of findCharacters
			if (chara != c && chara.name != "Summoner" && mapPositions.Contains(new Vector2(chara.getIntX(), chara.getIntY()) ))
			{
				charsInRange.Add(chara);
			}
		}
		return charsInRange;
	}

	//finds characters at a position and checks if they share a player num with a given palyer number
	bool findCharactersWithDifferentPlayerNumber(Vector2 pos, Character c)
	{
		bool flag = false;
		//find the character at the given position
		Character chara = findCharacterAt(gridToWorld(pos));

		//if there was a character
		if (chara != null)
		{
			//see if its an enemy
			if (chara.playerNumber != c.playerNumber)
			{
				flag = true;
				enemyPositions.Add(new Vector2(chara.getIntX(), chara.getIntY()));
			}
		}
		return flag;
	}

	//recursively makes the tiles that show a user where they can move
	//THIS FUNCTION IS A MESS BUT IT FINALLY WORKS AND IS PRETTY QUICK!
	//x and y here are in grid space
	void findCharacters(ArrayList characterPos, float range, int x, int y, float playerNumber)
	{
		Vector2 pos = new Vector2(x, y);

		//stop when you cant move
		if (range <= 0)
			return;

		//dont overlap positions
		if (!characterPos.Contains(pos))
		{
			//iif so, add this postion for spawning
			characterPos.Add(pos);
		}


		//move up,left,right,down
		if (y + 1 <= realRound(cursor.maxY / spacer))
		{
			findCharacters(characterPos,range-1, x, y + 1, playerNumber);
		}
		if (x - 1 >= 0)
		{
			findCharacters(characterPos, range-1, x - 1, y, playerNumber);
		}
		if (x + 1 <= realRound(cursor.maxX / spacer))
		{
			findCharacters(characterPos, range-1, x + 1, y, playerNumber);
		}
		if (y - 1 >= 0)
		{
			findCharacters(characterPos, range-1, x, y - 1, playerNumber);
		}
	}

	//recursively makes the tiles that show a user where they can move
	//THIS FUNCTION IS A MESS BUT IT FINALLY WORKS AND IS PRETTY QUICK!
	//x and y are in grid space
	public void FindMoveTile(int move, int x, int y, Character charToMove, bool hasMoved)
	{
		Vector2 pos = new Vector2(x, y);

		//find the map tile at this spot
		int index = mapTilePos.IndexOf(pos);

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

		//stop when you cant move
		if (move-cost < 0)
			return;

		Vector2 worldSpaceLoc = (gridToWorld(pos));
	
		//if there is a character on this square that is not your ally, you cannotmove there.
		if (hasMoved && characterPositions.Contains(worldSpaceLoc) && !findCharacterAt(worldSpaceLoc).isAllyTo(charToMove))
		{
			return;
		}

	
		
		//dont overlap positions
		if (!moveTilePositions.Contains(pos))
		{
			if (!hasMoved || (hasMoved && !characterPositions.Contains(worldSpaceLoc)))
			{
				//add this postion for spawning
				moveTilePositions.Add(pos);
			}
		}
		
		//move up,left,right,down
		if (y + 1 <= realRound(cursor.maxY / spacer))
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
					obj.transform.position = gridToWorld((Vector2)moveTilePositions[i]);
					break;
				case ("EnemyTile"):
					obj.transform.position = gridToWorld((Vector2)enemyPositions[i]);
					break;
				case ("SummonTile"):
					obj.transform.position = gridToWorld((Vector2)summonPositions[i]);
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
			default:
				print("This should not print.");
				break;
		}
	}
	public Player getPlayer(int num)
	{
		return players[num - 1];
	}
	public bool canSummon()
	{
		//must have the minimum mana to summon a unit
		if(getCurrentSummoner().mana < 2)
		{
			return false;
		}
		//must be places in range to summon to.
		findPlacesToSummon(summonPositions,getCurrentSummoner().summonRange,getCurrentSummoner().getIntX(), getCurrentSummoner().getIntY());
		if(summonPositions.Count > 0)
		{
			return true;
		}
		return false;
	}

	//recursively makes the tiles that show a user where they can move
	//THIS FUNCTION IS A MESS BUT IT FINALLY WORKS AND IS PRETTY QUICK!
	//x and y are in grid space here
	void findPlacesToSummon(ArrayList summonPos, float range, int x, int y)
	{
		Vector2 pos = new Vector2(x, y);
		//stop when you cant move
		if (range < 0)
			return;

		//print("Can we put a tile down at: " + x + " , " + y + " ? With a range of: "+range);
		//dont overlap positions and only add if the characterPossitinon array doesnt have this position
		if (!summonPos.Contains(pos) && !characterPositions.Contains(gridToWorld(pos)))
		{
			//if so, add this postion for spawning
			summonPos.Add(pos);
		}


		//move up,left,right,down
		if (y + 1 <= realRound(cursor.maxY / cursor.spacer))
		{
			findPlacesToSummon(summonPos, range - 1, x, y + 1);
		}
		if (x - 1 >= 0)
		{
			findPlacesToSummon(summonPos, range - 1, x - 1, y);
		}
		if (x + 1 <= realRound(cursor.maxX / cursor.spacer))
		{
			findPlacesToSummon(summonPos, range - 1, x + 1, y);
		}
		if (y - 1 >= 0)
		{
			findPlacesToSummon(summonPos, range - 1, x, y - 1);
		}
	}

	Vector2 gridToWorld(Vector2 gridCoord)
	{
		return roundPosition(new Vector2(gridCoord.x * spacer, gridCoord.y * spacer));
	}
	Vector2 worldToGrid(Vector2 worldCoord)
	{
		int x = realRound(worldCoord.x / spacer);
		int y = realRound(worldCoord.y / spacer);
		return new Vector2(x, y);
	}

	/*
	 * When clicking to move, the position doesnt necessarily stay grid like
	 * This function converts a clicked location into a grid location.
	 * The function is currently being used to convert gridspace coordinates to world space.
	 * "pos" here is in world space
	 */
	public Vector2 roundPosition(Vector2 pos)
	{
		float xPos = 0;
		float yPos = 0;

		//find the first position that is greater than the one clicked
		while (xPos < Mathf.Abs(pos.x))
		{
			xPos += spacer;
		}
		while (yPos < Mathf.Abs(pos.y))
		{
			yPos += spacer;
		}
		//look at the last position for both
		float lxPos = xPos - spacer;
		float lyPos = yPos - spacer;

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

	//for some reason the contains function does not work as it should, so I wrote my own
	public bool listHasVector(ArrayList list, Vector2 pos)
	{
		pos = roundPosition(pos);
		for (int i = 0; i < characterPositions.Count; i++)
		{
			//print("There is a " + ((Character)characters[i]).name + " at " + (Vector2)characterPositions[i]);
			if ((Vector2)characterPositions[i] == pos)
			{
				return true;
			}
		}
		return false;
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
