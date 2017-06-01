using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cursor : MonoBehaviour {
	public float size;
	public float maxX;
	public float maxY;
	public float spacer;
	private bool select;//if this is true, we can pick up a unit
	private Character selectedCharacter;
	private float orgX, orgY;
	private float charOrgX, charOrgY;
	MapGenerator map;
	Turns turn;
	// Use this for initialization
	void Start()
	{
		size = transform.localScale.x;
		spacer = size / 1.5f;
		//we do not start with a character started
		selectedCharacter = null;
		select = true;

		map = FindObjectOfType<MapGenerator>();
		turn = FindObjectOfType<Turns>();
		maxX = spacer * (map.boundsX - 1);
		maxY = spacer * (map.boundsY - 1);
	}
	// Update is called once per frame
	void Update ()
	{
		orgX = transform.position.x;
		orgY = transform.position.y;
		DetectMovement();
		//MouseMovement();
		//MobileTouchMovement();
		if (select)
		{
			LimitToBounds();
		}
		//if there is a character selected, find where it can move, and then limit the cursor to that
		else
		{
			if(!LimitToMoveTiles())
			{ 
				//if there was no move tile, you cant move there
				transform.position = new Vector3(orgX, orgY, 0);
			}
		}
		if (selectedCharacter != null)
		{
			selectedCharacter.gameObject.transform.position = transform.position;
		}
		
	}

	//used to move the cursor
	void DetectMovement()
	{
		if (Input.GetKeyDown(KeyCode.UpArrow))
		{
			transform.position += new Vector3(0, spacer, 0);
		}
		if (Input.GetKeyDown(KeyCode.DownArrow))
		{
			transform.position -= new Vector3(0, spacer, 0);
		}
		if (Input.GetKeyDown(KeyCode.RightArrow))
		{
			transform.position += new Vector3(spacer, 0, 0);
		}
		if (Input.GetKeyDown(KeyCode.LeftArrow))
		{
			transform.position -= new Vector3(spacer, 0, 0);
		}

		if(Input.GetKeyDown(KeyCode.Z))
		{
			if (select)
			{
				DetectSelect();
			}
			else
			{
				selectedCharacter.canMove = false;
				selectedCharacter = null;
				select = true;
				//remove the move tiles
				RemoveMoveTiles();
			}
		}
		if(Input.GetKeyDown(KeyCode.X))
		{
			if (!select)
			{
				selectedCharacter.transform.position = new Vector3(charOrgX, charOrgY, 0);
			}
			selectedCharacter = null;
			select = true;
			//remove the move tiles
			RemoveMoveTiles();
		}
	}

	/*
	 * Detects a click of the mouse
	 * Sends the cursor to the square closest to wherever was clicked
	 */ 
	void MouseMovement()
	{
		if (Input.GetMouseButtonDown(0))
		{
			//print("Button down");
			Vector2 mousePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

			//clicking gives screen coordinates, so just convert that to world space
			Vector3 clickPos = Camera.main.ScreenToWorldPoint(mousePosition);

			//see next function header for why we do this 
			transform.position = RoundPosition(clickPos);
			if (selectedCharacter != null)
			{
				selectedCharacter.gameObject.transform.position = transform.position;
			}
			if (select)
			{
				DetectSelect();
				select = false;
			}
			else
			{
				selectedCharacter.canMove = false;
				selectedCharacter = null;
				select = true;
				RemoveMoveTiles();
			}
		}
	}
	
	/*
	 * Just incase this game DOES get adapted to mobile, 
	 * This should mimic the mouse movement function, but with a touch request
	 */ 
	void MobileTouchMovement()
	{
		try
		{
			Vector2 fingerPos = Input.GetTouch(0).position;
			transform.position = RoundPosition(fingerPos);
		}
		catch(Exception e)
		{
			e.ToString();
			//GetTouch seems to be out of bounds everytime we arent clicking,
			//This isnt needed, but it makes the false error go away
		}
	}

	/*
	 * This function will see if there is a character to select
	 * It is implied that when you get to this call you have tried selecting something
	 * */
	void DetectSelect()
	{
		Character[] chars = FindObjectsOfType<Character>();
		for(int i = 0; i < chars.Length; i++)
		{
			if(RoundPosition(chars[i].gameObject.transform.position) == RoundPosition(transform.position))
			{
				if (chars[i].playerNumber == turn.playerTurn)
				{
					//print("Char found!");
					if (chars[i].canMove)
					{
						selectedCharacter = chars[i];
						select = false;
						charOrgX = selectedCharacter.transform.position.x;
						charOrgY = selectedCharacter.transform.position.y;

					}
				}
			}
		}

		if (!select) 
		{
			//if you select a character, put down move tiles
			//put down the places this char can move
			//save the original cooridinates incase we cancel the movement
			orgX /= spacer;
			orgY /= spacer;

			int oX = realRound(orgX);
			int oY = realRound(orgY);
			//print("Character is at: " + oX + " , " + oY);

			//displays all of the possible spaces that character can move to
			MakeMoveTile(selectedCharacter.move+1, oX, oY,selectedCharacter);
		}
	}


	//limit the cursor to only move on the map
	void LimitToBounds()
	{
		float tempX = transform.position.x;
		float tempY = transform.position.y;
		if(transform.position.x < 0)
		{
		    tempX = 0;
		} 
		else if(transform.position.x > maxX)
		{
			tempX = maxX;
		}
		if (transform.position.y < 0)
		{
			tempY = 0;
		}
		else if (transform.position.y > maxY)
		{
			tempY = maxY;
		}
		transform.position = new Vector3(tempX, tempY, 0);
	}

	//limit the cursor to only move over movement tiles
	bool LimitToMoveTiles()
	{
		bool returnable = false;
		int tempX = realRound(transform.position.x/spacer);
		int tempY = realRound(transform.position.y/spacer);

		//may want to research more for a better find function here
		GameObject[] moveTiles = GameObject.FindGameObjectsWithTag("MoveTile");

		//see if there is a move tile beneath you
		for(int i = 0; i < moveTiles.Length; i++)
		{
		//	print("Found a move tile at: " +realRound(moveTiles[i].transform.position.x/spacer)+" , "+realRound(moveTiles[i].transform.position.y/spacer));
			if(realRound(moveTiles[i].transform.position.x/spacer) == tempX && realRound(moveTiles[i].transform.position.y/spacer) == tempY)
			{
				returnable = true;
			}
		}
		return returnable;
	}

	/*
	 * When clicking to move, the position doesnt necessarily stay grid like
	 * This function converts a clicked location into a grid location.
	 */
	Vector3 RoundPosition(Vector3 pos)
	{
		float xPos = 0;
		float yPos = 0;
		//find the first position that is greater than the one clicked
		while(xPos < Mathf.Abs(pos.x))
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
		if(pos.x < 0)
		{
			xPos *= -1;
		}
		if(pos.y < 0)
		{
			yPos *= -1;
		}
		Vector2 newPos = new Vector2(xPos, yPos);

		return newPos;
	}

	void RemoveMoveTiles()
	{
		GameObject[] moveTiles = GameObject.FindGameObjectsWithTag("MoveTile");
		for (int i = 0; i < moveTiles.Length; i++)
		{
			GameObject.Destroy(moveTiles[i]);
		}
	}

	int realRound(float f)
	{
		float tempF = f;
		tempF -= (int)f;
		tempF *= 10;
		if((int)tempF > 4)
		{
			return (int)f + 1;
		}
		return (int)f;
	}

	//recursively makes the tiles that show a user where they can move
	void MakeMoveTile(int move, int x, int y, Character charToMove)
	{
		//if move is < 0 we are done here.
		//also just check if you're out of bounds
		if (move < 0)
		{
			return;
		}

		//if there is a character here, you're done.
		Character[] chars = FindObjectsOfType<Character>();
		for(int i = 0; i < chars.Length; i++)
		{
			if (chars[i] != charToMove)
			{
				if (realRound(chars[i].transform.position.x / spacer) == x && realRound(chars[i].transform.position.y / spacer) == y)
				{
					return;
				}
			}
		}

		//don't overlap movetiles
		GameObject[] gobj = GameObject.FindGameObjectsWithTag("MoveTile");
		for(int i = 0; i < gobj.Length; i++)
		{
			if(realRound(gobj[i].transform.position.x) == x && realRound(gobj[i].transform.position.y) == y)
			{
				return;
			}
		}

		//find the tile at that position
		GameObject[] mt = GameObject.FindGameObjectsWithTag("MapTile");
		int index =-1;
		for(int i = 0; i < mt.Length; i++)
		{
			if(realRound(mt[i].transform.position.x/spacer) == x && realRound(mt[i].transform.position.y/spacer) == y)
			{
				index = i;
			}
		}
		//just error checking here
		if(index == -1)
		{
			print("Index was -1 while at: " + x + " , " + y);
		}
		int cost = mt[index].GetComponent<MapTile>().moveCost;

		//if the player cannot move now, just stop
		if(move - cost < 0)
		{
			return;
		}
		//move up,left,right,down
		if(y+1 <= realRound(maxY/spacer))
			MakeMoveTile(move - cost, x, y + 1,charToMove);
		if(x-1 >= 0)
			MakeMoveTile(move - cost, x - 1, y,charToMove);
		if(x+1 <= realRound(maxX/spacer))
			MakeMoveTile(move - cost, x + 1, y,charToMove);
		if(y-1 >= 0)
			MakeMoveTile(move - cost, x, y - 1,charToMove);
		
		//on the way back, make a thing here
		GameObject obj = (GameObject)Instantiate(Resources.Load("Prefab/Tiles/MoveTile"));
		obj.transform.position = new Vector3(x*spacer, y*spacer);
	}
	public int getIntX()
	{
		return realRound(transform.position.x/spacer);
	}
	public int getIntY()
	{
		return realRound(transform.position.y / spacer);
	}
}
