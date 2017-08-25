using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cursor : MonoBehaviour {
	public float size;
	public float maxX;
	public float maxY;
	public float spacer;
	public bool cursorCanMove;
	public bool select;//if this is true, we can pick up a unit
	public bool summoning;
	public bool attacking;
	public Character selectedCharacter;
	private float orgX, orgY;
	private float charOrgX, charOrgY;
	private Character unselectableSummoner;
	private Character fightingCharacter;
	HUB hub;
	Turns turn;

	// Use this for initialization
	void Start()
	{
		hub = GameObject.FindObjectOfType<HUB>();
		size = transform.localScale.x;
		spacer = size / 1.5f;
		//we do not start with a character started
		selectedCharacter = null;
		select = true;
		cursorCanMove = true;
		turn = FindObjectOfType<Turns>();
		maxX = spacer * (hub.mapGenerator.boundsX - 1);
		maxY = spacer * (hub.mapGenerator.boundsY - 1);
		summoning = false;
		attacking = false;
	}
	// Update is called once per frame
	void Update ()
	{
	
		//record the x,y cooridinate the cursor is at currently
		orgX = transform.position.x;
		orgY = transform.position.y;

		//if the cursor can move, see if it is being moved
		if (cursorCanMove)
		{
			DetectMovement();
		}
		//MouseMovement();
		//MobileTouchMovement();

		//if there is no character selected, the cursor can go anywhere within the map's bounds
		//If we are summoning, there will also be no limit to where the cursor can move. This was done because
		//there will be cases where some summon tiles are cut off from the rest, so moving to them will be harder with bounds.
		//I am doing the same thing I did with summoning for attacking. It just makes things easier on my end for now
		if (select || summoning || attacking)
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


		//when the cursor switches to attack mode, we need to know which 
		//character is attacking, but also dont want to move the character.
		if (attacking)
		{
			if (selectedCharacter != null)
			{
				selectedCharacter.canMove = false;
				fightingCharacter = selectedCharacter;
				selectedCharacter = null;
			}
		}

		//move the selected character with the cursor
		if (selectedCharacter != null)
		{
			selectedCharacter.gameObject.transform.position = transform.position;
		}


	}

	//used to move the cursor
	void DetectMovement()
	{
		//mv shows where the cursor has moved to. This is so I can move the camera with the cursor
		Vector3 mv= new Vector3(0,0,0);

		//Each key only gives input every tenth of a second. This keeps the cursor
		//moving smoothley

		//up
		if (Input.GetKey(KeyCode.UpArrow) && Time.time - hub.lastTimeUp >= 0.1f)
		{
			hub.lastTimeUp = Time.time;
			mv = new Vector3(0, spacer, 0);
		}
		//down
		if (Input.GetKey(KeyCode.DownArrow) && Time.time - hub.lastTimeDown >= 0.1f)
		{
			hub.lastTimeDown = Time.time;
			mv = new Vector3(0, -1*spacer, 0);
		}
		//right
		if (Input.GetKey(KeyCode.RightArrow) && Time.time - hub.lastTimeRight >= 0.1f)
		{
			hub.lastTimeRight = Time.time;
			mv = new Vector3(spacer, 0, 0);
		}
		//left
		if (Input.GetKey(KeyCode.LeftArrow) && Time.time - hub.lastTimeLeft >= 0.1f)
		{
			hub.lastTimeLeft = Time.time;
			mv = new Vector3(-1*spacer, 0, 0);
		}
		
		//for confirm and cancel, I wait a bit longer between inputs to help stop the game
		//from canceling and confirming between menus. Sometimes this can be noticable, but hardly..

		//on confirm
		if(Input.GetKeyDown(KeyCode.Z) && Time.time - hub.lastTimeZ >= 0.25f)
		{
			hub.lastTimeZ = Time.time;
			//if you have not selected a character, do so
			if (select && !summoning && !attacking)
			{
				DetectSelect();
			}

			//if the character hits confirm, check if there is a summon tile, if so, place the unit there
			else if(summoning)
			{
				if(isOnTile("SummonTile"))
				{
					select = true;
					summoning = false;
					hub.RemoveTiles("SummonTile");
					selectedCharacter = null;
				}
			}
			//if the player is currently trying to select an attack target
			else if (attacking)
			{
				if(isOnTile("EnemyTile"))
				{
					select = true;
					attacking = false;
					hub.RemoveTiles("EnemyTile");
					//print(fightingCharacter.name);
					fightingCharacter.fight(hub.findCharacterAt(transform.position));
					confirmFromMoveMenu();
				}
			}
			//if you have a character selected, give them options from this point
			else
			{
				GoToMoveMenu();
			}
		}

		//on cancel
		if(Input.GetKeyDown(KeyCode.X) && Time.time - hub.lastTimeX >= 0.25f && !summoning)
		{
			hub.lastTimeX = Time.time;
			if (!select)
			{
				selectedCharacter.transform.position = new Vector3(charOrgX, charOrgY, 0);
			}
			if (selectedCharacter != null)
			{
				selectedCharacter = null;
			}
			select = true;

			//remove the extra tiles
			hub.RemoveTiles("MoveTile");
			hub.RemoveTiles("EnemyTile");
		}

		hub.cam.moveCamera(transform.position+mv);
		transform.position += mv;
	}

	bool isOnTile(string name)
	{
		bool ret = false;
		switch(name)
		{
			case ("SummonTile"):
				if(hub.summonPositions.Contains(new Vector2(getIntX(),getIntY())))
				{
					ret = true;
				}
				break;
			case ("MoveTile"):
				break;
			case ("EnemyTile"):
				if(hub.enemyPositions.Contains(new Vector2(getIntX(), getIntY())))
				{
					print("The cursor knows it is on an enemy Tile");
					ret = true;
				}
				break;
			default:
				return false;
		}
		return ret;
	}

	//Enters the MoveMenu script.
	//Gives players options after they have moved
	void GoToMoveMenu()
	{
		ArrayList list = new ArrayList();
		if (selectedCharacter.name.Contains("Summoner") && hub.canSummon())
		{
			list.Add("Summon");
		}
		//add Attack
		if(hub.enemyInRange(selectedCharacter))
		{
			list.Add("Attack");
		}
		if(selectedCharacter.canUseZeal && hub.charactersInRange(selectedCharacter).Count > 0)
		{
			list.Add("Speak");
		}
		//add Speak
		//add Heal
		list.Add("Stop");
		cursorCanMove = false;
		Vector3 pos = transform.position + new Vector3(2 * spacer, spacer, 0);
		hub.moveMenuHandler.MakeMoveMenu(list, pos);
	}

	//this is set up to be called from the moveMenu so that this stuff is only set IF they confirm.
	//on cancel we just move around again
	public void confirmFromMoveMenu()
	{
		if (selectedCharacter != null && !summoning)
		{
			selectedCharacter.canMove = false;
			selectedCharacter = null;
		}
		hub.enemyPositions.Clear();
		select = true;
		cursorCanMove = true;
		//remove the move tiles
		hub.RemoveTiles("MoveTile");
	}
	/*
	 * This function will see if there is a character to select
	 * It is implied that when you get to this call you have tried selecting something
	 * */
	void DetectSelect()
	{
		Character c = hub.findCharacterAt(transform.position);
		//if you hit a character, and you can select something
		if (c != null && select)
		{
			//if its that characters turn
			if (c.playerNumber == turn.playerTurn)
			{
				//if they can still move
				if (c.canMove)
				{
					//select them with the cursor
					selectedCharacter = c;
					select = false;
					charOrgX = selectedCharacter.transform.position.x;
					charOrgY = selectedCharacter.transform.position.y;
				}
				//if theyre a summoner, give them the chance to summon even if they cannot move
				else if (c.name == "Summoner" && hub.canSummon())
				{
					cursorCanMove = false;
					ArrayList list = new ArrayList();
					list.Add("Summon");
					Vector3 pos = transform.position + new Vector3(2 * spacer, spacer, 0);
					hub.moveMenuHandler.MakeMoveMenu(list, pos);
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
			hub.FindMoveTile(selectedCharacter.move, oX, oY,selectedCharacter,false);
			hub.MakeTiles("MoveTile");
		}
	}


	//limit the cursor to only move on the map
	//x and y are in world space
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
	 * The function is currently being used to convert gridspace coordinates to world space.
	 * "pos" here is in world space
	 */
	public Vector3 RoundPosition(Vector3 pos)
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

	//"pos" here is in grid space. 
	public void MoveTo(Vector2 pos)
	{
		transform.position = new Vector2(pos.x * spacer, pos.y * spacer);
	}


	

	//Rounds each component of a vector2
	Vector2 realRound(Vector2 f)
	{
		return new Vector2(realRound(f.x), realRound(f.y));
	}
	//Rounds numbers
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

	//returns the in-grid x,y cooridnate of the cursor
	public int getIntX()
	{
		return realRound(transform.position.x/spacer);
	}
	public int getIntY()
	{
		return realRound(transform.position.y / spacer);
	}
}
