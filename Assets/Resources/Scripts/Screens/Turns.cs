using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turns : AbstractScript
{ 
	public int numberOfPlayers = 1;
	public bool firstTurn = true;
	public static int playerTurn;
	public GameObject turnDisplay;
	private static Player player;

	// Use this for initialization
	void Start ()
	{
		playerTurn = 1;
		turnDisplay = GameObject.Find("TurnDisplay");
		player = getPlayer(playerTurn);
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		//keep track of who's turn it is
		player = getPlayer(playerTurn);
		while(player.hasLost())
		{
			playerTurn++;
			player = getPlayer(playerTurn);
		}

		//you cannot end your turn if you are summoning, attacking, or moving a unit
		if(player.getGamepad().isPressed("end") && hub.CURSOR.canSelect && !hub.CURSOR.summoning && !hub.CURSOR.attacking)
		{
			if(firstTurn)
			{
				firstTurn = false;
			}
			hub.RemoveTiles("SummonTile");
			hub.RemoveTiles("MoveTile");
			hub.RemoveTiles("EnemyTile");
			Character[] chars = FindObjectsOfType<Character>();
	
			//proc all of the end of turn abilities
			for(int i = 0; i < chars.Length; i++)
			{
				if(chars[i].playerNumber == playerTurn)
				{
					chars[i].EndTurn();
				}
			}

			//increment the turn count, and then set it to 1 if you go overboard
			playerTurn++;
			if(playerTurn > numberOfPlayers)
			{
				playerTurn = 1;
			}

			//set all characters who's turn it is to able to move
			for (int i = 0; i < chars.Length; i++)
			{
				if(chars[i].playerNumber == playerTurn)
				{
					chars[i].canMove = true;

					if (chars[i].name == "Summoner")
					{
						((Summoner)chars[i]).mana += 5;//you get 5 mana every turn! (subject to change)
						hub.CURSOR.transform.position = chars[i].transform.position;//put the cursor over the in-turn summoner
					}
				}
			}
		}
		//say whose turn it is
		turnDisplay.GetComponent<SpriteRenderer>().sprite = Resources.Load("Prefab/Turns/P"+playerTurn+"Turn",typeof(Sprite)) as Sprite;
	}

	public static Summoner getCurrentSummoner()
	{
		return getCurrentPlayer().getSummoner();
	}


	public static Player getCurrentPlayer()
	{
		return player;
	}
	public static int getCurrentPlayerTurn()
	{
		return playerTurn;
	}
}
