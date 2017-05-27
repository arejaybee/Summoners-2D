using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cursor : MonoBehaviour {
	public float size;
	private Character selectedCharacter;
	private bool select;//if this is true, we can pick up a unit
	private float maxX;
    private float maxY;
	private float orgX, orgY;
	private float spacer;
	// Use this for initialization
	void Start()
	{
		size = transform.localScale.x;
		spacer = size / 1.5f;
		//we do not start with a character started
		selectedCharacter = null;
		select = true;

		MapGenerator map = FindObjectOfType<MapGenerator>();
		maxX = spacer * (map.boundsX - 1);
		maxY = spacer * (map.boundsY - 1);
	}
	// Update is called once per frame
	void Update ()
	{
		orgX = transform.position.x;
		orgY = transform.position.y;
		if(!select)
		{
			orgX = selectedCharacter.transform.position.x;
			orgY = selectedCharacter.transform.position.y;
		}
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
				selectedCharacter = null;
				select = true;
				//remove the move tiles
				RemoveMoveTiles();
			}
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
				//print("Char found!");
				selectedCharacter = chars[i];
				select = false;
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

			//displays all of the possible spaces that character can move to
			for (int k = (int)(0 - selectedCharacter.move); k <= selectedCharacter.move; k++)
			{
				for (int j = (int)(0 - selectedCharacter.move); j <= selectedCharacter.move; j++)
				{
					//only put them on the map
					if (oX + k >= 0 && oX + k <= realRound(maxX / spacer) && oY + j <= realRound(maxY / spacer) && oY + j >= 0)
					{
						GameObject newPiece = (GameObject)Instantiate(Resources.Load("Prefab/Tiles/MoveTile"));
						newPiece.transform.position = new Vector3(spacer * (orgX + k), spacer * (orgY + j), 0);
						
					}
				}
			}
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
}
