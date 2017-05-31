using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turns : MonoBehaviour
{
	public int numberOfPlayers = 1;
	public int playerTurn;
	public GameObject turnDisplay;

	// Use this for initialization
	void Start ()
	{
		playerTurn = 1;
		turnDisplay = GameObject.Find("TurnDisplay");
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(Input.GetKeyDown(KeyCode.E))
		{
			//if it was 2, and there are 2 players.
			//1+1 = 2%3 = 2
			playerTurn++;
			if(playerTurn > numberOfPlayers)
			{
				playerTurn = 1;
			}

			//set all characters who's turn it is to able to move
			Character[] chars = FindObjectsOfType<Character>();
			for(int i = 0; i < chars.Length; i++)
			{
				if(chars[i].playerNumber == playerTurn)
				{
					chars[i].canMove = true;

					if (chars[i].name == "Summoner")
					{
						((Summoner)chars[i]).mana += 5;
					}
				}
			}
		}
		turnDisplay.GetComponent<SpriteRenderer>().sprite = Resources.Load("Prefab/Turns/P"+playerTurn+"Turn",typeof(Sprite)) as Sprite;
	}
}
